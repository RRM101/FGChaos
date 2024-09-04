using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class SlideEverywhere : Effect
    {
        public SlideEverywhere()
        {
            Name = "Slide Everywhere";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(SlideEverywhere), typeof(SlipperyFloor) };
        }

        public static bool active;

        public override void Run()
        {
            active = true;
            chaos.fallGuy.DefaultSurfaceModifier.SupportSliding = true;
            chaos.fallGuy.DefaultSurfaceModifier.ShouldOverrideSlideAngleCheck = true;
            chaos.fallGuy.DefaultSurfaceModifier.SlideTurningVelocityModifier = 1.19f;
            chaos.fallGuy.DefaultSurfaceModifier.SlideVelocityModifier = 1.1f;
            base.Run();
        }

        public override void End()
        {
            active = false;
            if (chaos != null)
            {
                chaos.fallGuy.DefaultSurfaceModifier.SupportSliding = false;
                chaos.fallGuy.DefaultSurfaceModifier.ShouldOverrideSlideAngleCheck = false;
                chaos.fallGuy.DefaultSurfaceModifier.SlideTurningVelocityModifier = 1;
                chaos.fallGuy.DefaultSurfaceModifier.SlideVelocityModifier = 0.9f;
                chaos.fallGuy.DefaultSurfaceModifier.VelocityCurveModifier = 1;
            }
            base.End();
        }
    }
}
