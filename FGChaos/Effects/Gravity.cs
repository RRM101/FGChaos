using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FGChaos.Effects
{
    public class Gravity : Effect
    {
        string name;
        int gravity;

        public override string Name
        {
            get { return name; }
        }

        public override int Duration
        {
            get { return 20; }
        }

        public override string ID
        {
            get { return "Gravity"; }
        }

        public override Type[] BlockedEffects
        {
            get { return new Type[] { typeof(Gravity) }; }
        }

        Dictionary<string, int> gravityDictionary = new Dictionary<string, int>()
        {
            {"Zero", 0},
            {"Low", -5},
            {"High", -50}
        };

        public Gravity()
        {
            string[] gravityArray = gravityDictionary.Keys.ToArray();
            string gravityType = gravityArray[UnityEngine.Random.Range(0, gravityArray.Length)];
            name = $"{gravityType} Gravity";
            gravity = gravityDictionary[gravityType];
        }

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
