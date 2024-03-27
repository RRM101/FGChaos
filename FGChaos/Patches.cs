using FG.Common.Character;
using FG.Common.Character.MotorSystem;
using FGClient;
using FGClient.OfflinePlayground;
using FGClient.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FGChaos
{
    public class Patches
    {
        [HarmonyPatch(typeof(OfflinePlaygroundManager), "OnIntroCamerasComplete")]
        [HarmonyPatch(typeof(ClientGameManager), "OnIntroCountdownEnded")]
        [HarmonyPrefix]
        static bool StartChaos()
        {
            ChaosPluginBehaviour.instance.EnableChaos();            
            return true;
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

        [HarmonyPatch(typeof(StateVictoryScreen), "ProceedToNextState")]
        [HarmonyPrefix]
        static bool StateVictoryScreenProceedToNextState(StateVictoryScreen __instance)
        {
            __instance._victoryScreenViewModel.HideScreen();
            Addressables.LoadSceneAsync("MainMenu");
            return false;
        }

        [HarmonyPatch(typeof(TitleScreenViewModel), "HandlePlayButtonDown")]
        [HarmonyPrefix]
        static bool TitleScreenViewModelHandlePlayButtonDown(TitleScreenViewModel __instance)
        {
            ChaosPluginBehaviour.UnloadBank("BNK_SFX_WinnerScreen");
            return true;
        }
    }
}
