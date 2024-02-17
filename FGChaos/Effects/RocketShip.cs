using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class RocketShip : Effect
    {
        public override string Name
        {
            get { return "Rocket Ship"; }
        }

        public override int Duration
        {
            get { return 15; }
        }

        public override Type[] BlockedEffects
        {
            get { return new Type[] { typeof(Jetpack) }; }
        }

        public override void Run()
        {
            Chaos.rocketShip = true;
            base.Run();
        }

        public override void End()
        {
            Chaos.rocketShip = false;
            base.End();
        }
    }
}
