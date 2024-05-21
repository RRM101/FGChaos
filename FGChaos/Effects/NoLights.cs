using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class NoLights : Effect
    {
        public NoLights()
        {
            Name = "No Lights";
            Duration = 20;
        }

        List<Light> Lights = new();

        public override void Run()
        {
            Light[] allLights = Resources.FindObjectsOfTypeAll<Light>();

            foreach (Light light_ in allLights)
            {
                if (light_.gameObject.active)
                {
                    Lights.Add(light_);
                }
            }

            foreach (Light light in Lights)
            {
                light.gameObject.SetActive(false);
            }

            base.Run();
        }

        public override void End()
        {
            foreach (Light light in Lights)
            {
                if (light != null)
                {
                    light.gameObject.SetActive(true);
                }
            }

            base.End();
        }
    }
}
