﻿using FG.Common.Character;
using FG.Common.Character.MotorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class HandsInTheAir : Effect
    {
        new public string Name = "Hands In The Air";

        new public int Duration = 5;

        MotorFunctionRagdollStateRollOver stateRollOver;

        public override void Run()
        {
            stateRollOver = chaos.motorAgent.GetMotorFunction<MotorFunctionRagdoll>().GetState<MotorFunctionRagdollStateRollOver>();
            stateRollOver.Begin(0);
            WaitTillEnd();
            isActive = true;
        }

        public override void End()
        {
            stateRollOver.End(0);
            base.End();
        }
    }
}