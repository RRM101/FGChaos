using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class SlipperyFloor : Effect
    {
        public SlipperyFloor()
        {
            Name = "Slippery Floor";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(SlideEverywhere), typeof(SlipperyFloor) };
        }

        public override void Run()
        {
            chaos.fallGuy.DefaultSurfaceModifier.VelocityCurveModifier = 0.25f;
            base.Run();
        }

        public override void End()
        {
            chaos.fallGuy.DefaultSurfaceModifier.VelocityCurveModifier = 1;
            base.End();
        }
    }
}
