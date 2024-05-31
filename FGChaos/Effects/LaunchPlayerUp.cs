using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class LaunchPlayerUp : Effect
    {
        public LaunchPlayerUp()
        {
            Name = "Launch Player Up";
        }

        public override void Run()
        {
            chaos.fgrb.AddForce(new UnityEngine.Vector3(0, 120, 0), UnityEngine.ForceMode.VelocityChange);
            base.Run();
        }
    }
}
