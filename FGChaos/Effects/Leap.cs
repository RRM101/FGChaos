using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class Leap : Effect
    {
        public Leap()
        {
            Name = "Leap";
            Duration = 30;
        }

        public override void Run()
        {
            Chaos.OnJumpActions.Add(Boost);
            base.Run();
        }

        void Boost()
        {
            Vector3 boost = new Vector3(0, 5, 30);
            boost = chaos.fallGuy.transform.rotation * boost;

            chaos.fgrb.AddForce(boost, ForceMode.VelocityChange);
        }

        public override void End()
        {
            if (Chaos.OnJumpActions.Contains(Boost))
            {
                Chaos.OnJumpActions.Remove(Boost);
            }

            base.End();
        }
    }
}
