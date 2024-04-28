using FG.Common.Character;
using Levels.Progression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    /*
        Effect Idea by DriftBoneYT
    */

    public class RespawnAtLastCheckpoint : Effect
    {
        public RespawnAtLastCheckpoint()
        {
            Name = "Respawn at the Last Checkpoint";
        }

        public override void Run()
        {
            CheckpointManager checkpointManager = GameObject.FindObjectOfType<CheckpointManager>();
            CheckpointZone checkpointZone = checkpointManager._checkpointZones.Last();
            checkpointZone.GetNextSpawnPositionAndRotation(out Vector3 position, out Quaternion rotation);
            chaos.fallGuy.MotorAgent.GetMotorFunction<MotorFunctionTeleport>().RequestTeleport(position, rotation);

            base.Run();
        }
    }
}
