﻿using System;

namespace FGChaos.Effects
{
    public class RocketShip : Effect
    {
        public RocketShip()
        {
            Name = "Rocket Ship";
            Duration = 15;
            BlockedEffects = new Type[] { typeof(RocketShip), typeof(Jetpack), typeof(PiracyIsNoFalling), typeof(InfiniteJumps) };
        }

        public override void Run()
        {
            Chaos.rocketShip = true;
            base.Run();
        }

        public override void End()
        {
            Chaos.rocketShip = false;
            base.End();
        }
    }
}
