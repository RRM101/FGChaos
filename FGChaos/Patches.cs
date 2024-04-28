using FG.Common;
using FG.Common.Character;
using FGClient;
using FGClient.OfflinePlayground;
using FGClient.UI;
using HarmonyLib;
using Rewired;
using System;
using UnityEngine;

namespace FGChaos
{
    public class Patches
    {
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
                __result = __result * -1;
            }
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
