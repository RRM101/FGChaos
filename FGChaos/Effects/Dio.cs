using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class Dio : Effect
    {
        public Dio()
        {
            Name = "DIO";
            Duration = 15;
        }

        Vector3 originalPosition;
        Quaternion originalRotation;

        public override void Run()
        {
            originalPosition = chaos.fallGuy.transform.position;
            originalRotation = chaos.fallGuy.transform.rotation;
            base.Run();
        }

        public override void Update()
        {
            if (chaos != null)
            {
                if (chaos.fallGuy != null)
                {
                    if (Math.Abs(Vector3.Distance(originalPosition, chaos.fallGuy.transform.position)) >= 25)
                    {
                        chaos.fallGuy.transform.position = originalPosition;
                        chaos.fallGuy.transform.rotation = originalRotation;
                    }
                }
            }
        }
    }
}
