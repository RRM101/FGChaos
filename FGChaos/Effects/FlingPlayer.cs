using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class FlingPlayer : Effect
    {
        public override string Name
        {
            get { return "Fling Player"; }
        }

        public override void Run()
        {
            int random_x = UnityEngine.Random.Range(-200, 200);
            int random_z = UnityEngine.Random.Range(-200, 201);
            chaos.fgrb.velocity = new Vector3(random_x, Math.Max(Math.Abs(random_x), Math.Abs(random_z)), random_z) * 10;
            base.Run();
        }
    }
}
