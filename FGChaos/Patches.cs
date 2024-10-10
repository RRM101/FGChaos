using BepInEx.Unity.IL2CPP.Utils.Collections;
using FG.Common;
using FG.Common.Character;
using FG.Common.CMS;
using FGChaos.Effects;
using FGClient;
using FGClient.OfflinePlayground;
using FGClient.UI;
using HarmonyLib;
using Rewired;
using System;
using System.Collections;
using UnityEngine;

namespace FGChaos
{
    public class Patches
    {
        static IEnumerator InputDelay(MotorFunctionMovementStateMove instance, Vector3 direction, float magnitude)
        {
            float delay = SwitchMoment.active ? 0.3f : 0;
            yield return new WaitForSeconds(delay);
            if (instance != null)
            {
                instance._motorFunctionMovement.ApplyNormalMovement(direction, magnitude, MotorFunctionMovement.UpdateVelocityMode.ReduceWithAngle);
            }
        }

        [HarmonyPatch(typeof(CMSLoader), "InitItemsFromContent")]
        [HarmonyPostfix]
        static void CMSLoaderInitItemsFromContent(CMSLoader __instance)
        {
            ChaosManager.instance.HandleCMSDataParsedEvent();
        }

        [HarmonyPatch(typeof(OfflinePlaygroundManager), "OnIntroCamerasComplete")]
        [HarmonyPatch(typeof(ClientGameManager), "OnIntroCountdownEnded")]
        [HarmonyPostfix]
        static void StartChaos()
        {
            if (!Plugin.Disable.Value && !Plugin.tempDisable)
            {
                ChaosManager.instance.EnableChaos();
            }
        }

        [HarmonyPatch(typeof(GameplayTimerViewModel), "Initialise")]
        [HarmonyPostfix]
        static void GameplayTimerViewModelInitialise(GameplayTimerViewModel __instance)
        {
            if (!Plugin.Disable.Value)
            {
                GameObject.Destroy(__instance.gameObject);
            }
        }

        [HarmonyPatch(typeof(MotorFunctionJumpStateInactive), "UpdateState")]
        [HarmonyPrefix]
        static bool MotorFunctionJumpStateInactiveUpdateState(MotorFunctionJumpStateInactive __instance, ref int __result)
        {
            int h = -1;

            if (__instance._jumpTask.isRequested || __instance._timeOfQueue + 0.1f > Time.time)
            {
                if (Chaos.CanJump(__instance._motorFunctionJump))
                {
                    __instance._timeOfQueue = float.MinValue;
                    h = MotorFunctionJumpStateInactive.jumpLiftOffStateID;
                }
                if (__instance._jumpTask.isRequested)
                {
                    __instance._timeOfQueue = Time.time;
                }
            }

            if (Chaos.rocketShip)
            {
                h = MotorFunctionJumpStateInactive.jumpLiftOffStateID;
            }

            __result = h;
            return false;
        }

        [HarmonyPatch(typeof(Player), "GetAxis", argumentTypes: new Type[] {typeof(string)})]
        [HarmonyPostfix]
        static void InvertedControlsPatch(Player __instance, string actionName, ref float __result)
        {
            if (actionName == "Move Vertical" && WKeyStuck.active)
            {
                __result = 1;
            }

            if (InvertedControls.active)
            {
                __result *= -1;
            }
        }

        [HarmonyPatch(typeof(MotorFunctionDiveStateSlide), "Begin")]
        [HarmonyPrefix]
        static bool MotorFunctionDiveStateSlideBegin(MotorFunctionDiveStateSlide __instance)
        {
            if (SlideEverywhere.active)
            {
                __instance.MotorAgent.Character.DefaultSurfaceModifier.VelocityCurveModifier = 0.25f;
            }
            return true;
        }

        [HarmonyPatch(typeof(MotorFunctionDiveStateSlide), "End")]
        [HarmonyPrefix]
        static bool MotorFunctionDiveStateSlideEnd(MotorFunctionDiveStateSlide __instance)
        {
            if (SlideEverywhere.active)
            {
                __instance.MotorAgent.Character.DefaultSurfaceModifier.VelocityCurveModifier = 1;
            }
            return true;
        }

        [HarmonyPatch(typeof(MotorFunctionMovementStateMove), "OnManagedFixedUpdate_Local")]
        [HarmonyPrefix]
        static bool MotorFunctionMovementStateMove(MotorFunctionMovementStateMove __instance)
        {
            if (__instance._moveTask.ShouldMove)
            {
                __instance.CalculateDirectionAndMagnitude(out Vector3 direction, out float magnitude);
                if (SwitchMoment.active)
                {
                    ChaosManager.instance.StartCoroutine(InputDelay(__instance, direction, magnitude).WrapToIl2Cpp());
                }
                else
                {
                    __instance._motorFunctionMovement.ApplyNormalMovement(direction, magnitude, MotorFunctionMovement.UpdateVelocityMode.ReduceWithAngle);
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(StateVictoryScreen), "ProceedToNextState")]
        [HarmonyPrefix]
        static bool StateVictoryScreenProceedToNextState(StateVictoryScreen __instance)
        {
            __instance._victoryScreenViewModel.HideScreen();
            __instance._gsm.ReplaceCurrentState(new StateMainMenu(__instance._gsm, __instance._gameStateData, false).Cast<GameStateMachine.IGameState>());
            return false;
        }

        [HarmonyPatch(typeof(MotorFunctionJumpStateLiftOff), "Begin")]
        [HarmonyPostfix]
        static void OnJump()
        {
            foreach (Action action in Chaos.OnJumpActions)
            {
                action.Invoke();
            }
        }

        [HarmonyPatch(typeof(PlayerCameraController), "HandleRelativeAxisMovement")]
        [HarmonyPatch(typeof(PlayerCameraController), "HandleJoystickCameraMovement")]
        [HarmonyPrefix]
        static bool InvertCameraControlsPatch(PlayerCameraController __instance, ref Vector2 lookInput)
        {
            if (InvertedCameraControls.active)
            {
                lookInput *= -1;
            }
            return true;
        }

        [HarmonyPatch(typeof(LocalisedStrings), "GetString", argumentTypes: new Type[] { typeof(string) })]
        [HarmonyPostfix]
        static void LocalisedStringsGetString(ref string __result)
        {
            if (__result != null && EvilFallGuys.isEvil)
            {
                __result = "EVIL " + __result;
            }
        }

        [HarmonyPatch(typeof(Prefab_UI_Intro_Overlay), "InitTexts")]
        [HarmonyPostfix]
        static void Prefab_UI_Intro_OverlayInitTexts(Prefab_UI_Intro_Overlay __instance)
        {
            if (EvilFallGuys.isEvil)
            {
                __instance.LevelNameText = "EVIL " + __instance.LevelNameText;
                __instance.LevelDescriptionText = "EVIL " + __instance.LevelDescriptionText;
            }
        }

        [HarmonyPatch(typeof(LoadingGameScreenViewModel), "InitTexts")]
        [HarmonyPostfix]
        static void LoadingGameScreenViewModelInitTexts(LoadingGameScreenViewModel __instance)
        {
            if (EvilFallGuys.isEvil)
            {
                __instance.RoundNameText = "EVIL " + __instance.RoundNameText;
                __instance.RoundDescriptionText = "EVIL " + __instance.RoundDescriptionText;
            }
        }

        [HarmonyPatch(typeof(PlayerInfoDisplayCanvas), "SetText")]
        [HarmonyPrefix]
        static bool PlayerInfoDisplayCanvasSetText(ref string text)
        {
            if (EvilFallGuys.isEvil)
            {
                text = "EVIL " + text;
            }

            return true;
        }
    }
}
