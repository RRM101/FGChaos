using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FGChaos.Effects
{
    public class Gravity : Effect
    {
        public Gravity()
        {
            string[] gravityArray = gravityDictionary.Keys.ToArray();
            string gravityType = gravityArray[UnityEngine.Random.Range(0, gravityArray.Length)];
            Name = $"{gravityType} Gravity";
            gravity = gravityDictionary[gravityType];
            Duration = 20;
            BlockedEffects = new Type[] { typeof(Gravity) };
        }

        int gravity;
        Dictionary<string, int> gravityDictionary = new Dictionary<string, int>()
        {
            {"Zero", 0},
            {"Low", -5},
            {"High", -50}
        };

        public override void Run()
        {
            Physics.gravity = new Vector3(0, gravity, 0);
            base.Run();
        }

        public override void End()
        {
            Physics.gravity = new Vector3(0, -30, 0);
            base.End();
        }
    }
}
