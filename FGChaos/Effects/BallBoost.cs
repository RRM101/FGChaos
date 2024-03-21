using FG.Common.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class BallBoost : Effect
    {
        public BallBoost()
        {
            Name = "Ball Boost";
            ID = "BallBoost";
        }

        public override void Run()
        {
            MotorFunctionPowerup motorFunctionPowerup = chaos.fallGuy.MotorAgent.GetMotorFunction<MotorFunctionPowerup>();
            motorFunctionPowerup.EquippedPowerupData._powerup = Resources.FindObjectsOfTypeAll<RollingBallPowerup>().FirstOrDefault();
            motorFunctionPowerup.EquippedPowerupData._hasInfiniteStacks = true;
            motorFunctionPowerup.EquippedPowerupData._duration = -1;
            motorFunctionPowerup.SetStateTo(MotorFunctionPowerup._useStateId);
            chaos.fgrb.velocity = chaos.fallGuy.transform.rotation * new Vector3(0, 300, 500);
            base.Run();
        }
    }
}
