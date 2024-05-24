using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class SuperJump : Effect
    {
        public SuperJump()
        {
            Name = "Super Jump";
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

            chaos.fgrb.velocity = new Vector3(chaos.fgrb.velocity.x + boost.x, chaos.fgrb.velocity.y + boost.y, chaos.fgrb.velocity.z + boost.z);
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
