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

        Light[] lights;

        public override void Run()
        {
            lights = Resources.FindObjectsOfTypeAll<Light>();

            foreach (Light light in lights)
            {
                light.gameObject.SetActive(false);
            }

            base.Run();
        }

        public override void End()
        {
            foreach (Light light in lights)
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
