using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class JumpBoost : Effect
    {
        new public string Name = "Jump Boost";

        new public int Duration = 10;

        public override void Run()
        {
            chaos.fallGuy._inheritedJumpVelocity = new Vector3(0, 25, 0);
            WaitTillEnd();
        }

        public override void End()
        {
            chaos.fallGuy._inheritedJumpVelocity = new Vector3(0, 0, 0);
            base.End();
        }
    }
}
