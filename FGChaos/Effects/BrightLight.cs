using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class BrightLight : Effect
    {
        public BrightLight()
        {
            Name = "Bright Light";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(BrightLight), typeof(Creepypasta), typeof(NoLights) };
        }

        Light[] lights;
        Dictionary<Light, float> lightValues = new Dictionary<Light, float>();

        public override void Run()
        {
            lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                lightValues.Add(light, light.intensity);
                light.intensity = 5;
            }
            base.Run();
        }

        public override void End()
        {
            foreach (Light light in lights)
            {
                if (light != null)
                {
                    light.intensity = lightValues[light];
                }
            }
            base.End();
        }
    }
}
