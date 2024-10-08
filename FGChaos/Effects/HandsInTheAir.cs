﻿using FG.Common.Character;

namespace FGChaos.Effects
{
    public class HandsInTheAir : Effect
    {
        public HandsInTheAir()
        {
            Name = "Hands In The Air";
            Duration = 10;
        }

        MotorFunctionRagdollStateRollOver stateRollOver;

        public override void Run()
        {
            stateRollOver = chaos.motorAgent.GetMotorFunction<MotorFunctionRagdoll>().GetState<MotorFunctionRagdollStateRollOver>();
            stateRollOver.Begin(0);
            base.Run();
        }

        public override void End()
        {
            if (chaos != null)
            {
                if (chaos.fallGuy != null)
                {
                    stateRollOver.End(0);
                }
            }
            base.End();
        }
    }
}
