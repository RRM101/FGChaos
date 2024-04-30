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
    public class RespawnAtRandomCheckpoint : Effect
    {
        public RespawnAtRandomCheckpoint()
        {
            Name = "Respawn at a Random Checkpoint";
        }

        public override void Run()
        {
            CheckpointManager checkpointManager = GameObject.FindObjectOfType<CheckpointManager>();
            CheckpointZone checkpointZone = checkpointManager._checkpointZones[UnityEngine.Random.Range(0, checkpointManager._checkpointZones.Count)];
            checkpointZone.GetNextSpawnPositionAndRotation(out Vector3 position, out Quaternion rotation);
            chaos.fallGuy.MotorAgent.GetMotorFunction<MotorFunctionTeleport>().RequestTeleport(position, rotation);

            base.Run();
        }
    }
}
