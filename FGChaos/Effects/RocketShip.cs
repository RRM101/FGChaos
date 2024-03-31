using System;

namespace FGChaos.Effects
{
    public class RocketShip : Effect
    {
        public RocketShip()
        {
            Name = "Rocket Ship";
            ID = "RocketShip";
            Duration = 15;
            BlockedEffects = new Type[] { typeof(Jetpack) };
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
