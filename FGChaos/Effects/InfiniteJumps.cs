using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class InfiniteJumps : Effect
    {
        public InfiniteJumps()
        {
            Name = "Infinite Jumps";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(RocketShip), typeof(Jetpack), typeof(PiracyIsNoFalling), typeof(InfiniteJumps) };
        }

        public static bool active;

        public override void Run()
        {
            active = true;
            base.Run();
        }

        public override void End()
        {
            active = false;
            base.End();
        }
    }
}
