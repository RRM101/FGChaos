﻿using FG.Common.Character;
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

        /*[HarmonyPatch(typeof(COMMON_SpeedArch), "CreateSpeedBoostDataInstance")]
        [HarmonyPrefix]
        static bool COMMON_SpeedArchCreateSpeedBoostDataInstance(COMMON_SpeedArch __instance)
        {
            if (__instance.SpeedBoostData == null && __instance._movementModifier != null)
            {
                __instance.SpeedBoostData = __instance._movementModifier.SpeedBoostData;
            }

            return false;
        }

        [HarmonyPatch(typeof(SpeedBoostData), MethodType.Constructor, new Type[] { typeof(SpeedBoostData) }, new ArgumentType[] {ArgumentType.Ref} )] // WHY DOESNT THE CONSTRUCTOR PATCH
        [HarmonyPrefix]
        static bool SpeedBoostData(ref SpeedBoostData other)
        {
            try
            {
                Debug.Log(other._allowsStacking);
            }
            catch
            {

            }
            Application.Quit();
            return false;
        }*/
    }
}
