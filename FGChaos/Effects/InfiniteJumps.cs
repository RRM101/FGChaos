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

        public override void Run()
        {
            Chaos.infiniteJumps = true;
            base.Run();
        }

        public override void End()
        {
            Chaos.infiniteJumps = false;
            base.End();
        }
    }
}
