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
        public override string Name
        {
            get { return "Jump Boost"; }
        }

        public override int Duration
        {
            get { return 15; }
        }

        public override string ID
        {
            get { return "JumpBoost"; }
        }

        public override void Run()
        {
            chaos.fallGuy._inheritedJumpVelocity = new Vector3(0, 25, 0);
            base.Run();
        }

        public override void End()
        {
            chaos.fallGuy._inheritedJumpVelocity = new Vector3(0, 0, 0);
            base.End();
        }
    }
}
