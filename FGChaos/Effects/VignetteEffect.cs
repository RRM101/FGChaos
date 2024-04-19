using System;
using UnityEngine.Rendering.PostProcessing;

namespace FGChaos.Effects
{
    public class VignetteEffect : Effect
    {
        public VignetteEffect()
        {
            Name = "Vignette";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(VignetteEffect), typeof(Creepypasta) };
        }

        PostProcessProfile postProcessProfile;

        public override void Run()
        {
            postProcessProfile = chaos.postProcessVolume.profile;
            Vignette vignette = postProcessProfile.AddSettings<Vignette>();
            vignette.intensity.Override(0.6f);
            vignette.smoothness.Override(0.75f);
            base.Run();
        }

        public override void End()
        {
            postProcessProfile.RemoveSettings<Vignette>();
            base.End();
        }
    }
}
