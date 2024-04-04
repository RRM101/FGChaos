using FGClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace FGChaos.Effects
{
    public class Creepypasta : Effect
    {
        public Creepypasta()
        {
            Name = "Creepypasta";
            ID = "Creepypasta";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(VignetteEffect), typeof(Creepypasta) };
        }

        PostProcessProfile postProcessProfile;
        Light[] lights;
        Dictionary<Light, Color> lightValues = new Dictionary<Light, Color>();

        public override void Run()
        {
            postProcessProfile = chaos.postProcessVolume.profile;
            Vignette vignette = postProcessProfile.AddSettings<Vignette>();
            /*ColorGrading colorGrading = postProcessProfile.AddSettings<ColorGrading>(); // this doesn't work so i have to change the colour of the lights
            colorGrading.colorFilter.Override(Color.red);*/
            vignette.intensity.Override(0.6f);
            vignette.smoothness.Override(0.75f);
            vignette.color.Override(new Color(0.3f, 0, 0, 0));
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out ClientGameManager cgm);
            cgm._musicInstance.setPitch(0.3f);
            lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                lightValues.Add(light, light.color);
                light.color = Color.red;
            }

            base.Run();
        }

        public override void End()
        {
            postProcessProfile.RemoveSettings<Vignette>();
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out ClientGameManager cgm);
            cgm._musicInstance.setPitch(1f);
            foreach (Light light in lights)
            {
                light.color = lightValues[light];
            }

            base.End();
        }
    }
}
