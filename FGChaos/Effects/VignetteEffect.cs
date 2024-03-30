using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.PostProcessing;

namespace FGChaos.Effects
{
    public class VignetteEffect : Effect
    {
        public VignetteEffect()
        {
            Name = "Vignette";
            ID = "VignetteEffect";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(VignetteEffect) };
        }

        PostProcessProfile postProcessProfile;

        public override void Run()
        {
            postProcessProfile = chaos.postProcessVolume.profile;
            Vignette vignette = postProcessProfile.AddSettings<Vignette>();
            vignette.intensity.Override(0.6f);
            vignette.roundness.Override(0.75f);
            base.Run();
        }

        public override void End()
        {
            postProcessProfile.RemoveSettings<Vignette>();
            base.End();
        }
    }
}
