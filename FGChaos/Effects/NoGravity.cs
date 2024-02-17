using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class NoGravity : Effect
    {
        public override string Name
        {
            get { return "No Gravity"; }
        }

        public override int Duration
        {
            get { return 20; }
        }

        public override void Run()
        {
            Physics.gravity = new Vector3(0, 0, 0);
            base.Run();
        }

        public override void End()
        {
            Physics.gravity = new Vector3(0, -30, 0);
            base.End();
        }
    }
}
