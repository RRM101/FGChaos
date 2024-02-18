using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FGChaos.Effects
{
    public class LowGravity : Effect
    {
        public override string Name
        {
            get { return "Low Gravity"; }
        }

        public override int Duration
        {
            get { return 30; }
        }

        public override void Run()
        {
            Physics.gravity = new Vector3(0, -5, 0);
            base.Run();
        }

        public override void End()
        {
            Physics.gravity = new Vector3(0, -30, 0);
            base.End();
        }
    }
}
