using BepInEx.Unity.IL2CPP.Utils.Collections;
using FG.Common;
using FG.Common.Character;
using FG.Common.Character.MotorSystem;
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
            float delay = Chaos.switchMode ? 0.3f : 0;
            yield return new WaitForSeconds(delay);
            instance._motorFunctionMovement.ApplyNormalMovement(direction, magnitude, MotorFunctionMovement.UpdateVelocityMode.ReduceWithAngle);
        }

        [HarmonyPatch(typeof(OfflinePlaygroundManager), "OnIntroCamerasComplete")]
        [HarmonyPatch(typeof(ClientGameManager), "OnIntroCountdownEnded")]
        [HarmonyPostfix]
        static void StartChaos()
        {
            ChaosPluginBehaviour.instance.EnableChaos();
        }

        [HarmonyPatch(typeof(GameplayTimerViewModel), "Initialise")]
        [HarmonyPrefix]
        static bool GameplayTimerViewModelInitialise(GameplayTimerViewModel __instance)
        {
            GameObject.Destroy(__instance.gameObject);
            return false;
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
        static void InvertedControlsPatch(Player __instance, ref float __result)
        {
            if (Chaos.invertedControls)
            {
                __result *= -1;
            }
        }

        [HarmonyPatch(typeof(MotorFunctionDiveStateSlide), "Begin")]
        [HarmonyPrefix]
        static bool MotorFunctionDiveStateSlideBegin(MotorFunctionDiveStateSlide __instance)
        {
            if (Chaos.slideEverywhere)
            {
                __instance.MotorAgent.Character.DefaultSurfaceModifier.VelocityCurveModifier = 0.25f;
            }
            return true;
        }

        [HarmonyPatch(typeof(MotorFunctionDiveStateSlide), "End")]
        [HarmonyPrefix]
        static bool MotorFunctionDiveStateSlideEnd(MotorFunctionDiveStateSlide __instance)
        {
            if (Chaos.slideEverywhere)
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
                ChaosPluginBehaviour.instance.StartCoroutine(InputDelay(__instance, direction, magnitude).WrapToIl2Cpp());
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
    }
}
