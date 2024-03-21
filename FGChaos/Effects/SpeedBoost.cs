using FGClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace FGChaos.Effects
{
    public class SpeedBoost : Effect
    {
        public SpeedBoost()
        {
            Name = "Speed Boost";
            ID = "SpeedBoost";
            Duration = 15;
        }

        public override void Run()
        {
            StartCoroutine(Speed());
            base.Run();
        }

        IEnumerator Speed() // very stupid way of doing this
        {
            COMMON_SpeedArch speedArch;
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("11374594082ca994d8f12cfff47429da");
            yield return handle;
            if (handle.Result != null)
            {
                speedArch = handle.Result.GetComponent<COMMON_SpeedArch>();
                speedArch.CreateSpeedBoostDataInstance();
                SpeedBoostData speedBoostData = speedArch.SpeedBoostData;
                speedBoostData._duration = actualDuration;
                speedBoostData._instantVelocityBoost = 5.0f;
                speedBoostData._turnSpeedMultiplier = 5;
                speedBoostData._moveSpeedMultiplier = 5;
                chaos.fallGuy.SpeedBoostManager.AddSpeedBoost(speedBoostData, null, GlobalGameStateClient.Instance.GameStateView.SimulationTime, true);
                speedArch.PlayBoostTriggerVFX(chaos.fallGuy);
                AudioManager.PlayOneShot(AudioManager.EventMasterData.SpeedArchesEnter3D, chaos.fallGuy.transform.position);
            }
        }

        public override void End()
        {
            chaos.fallGuy.SpeedBoostManager.ClearSpeedBoost();
            base.End();
        }
    }
}
