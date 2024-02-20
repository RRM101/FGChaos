using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class LockCamera : Effect
    {
        public override string Name
        {
            get { return "Lock Camera"; }
        }

        public override string ID
        {
            get { return "LockCamera"; }
        }

        public override int Duration
        {
            get { return 20; }
        }

        public override Type[] BlockedEffects
        {
            get { return new Type[] { typeof(FirstPersonMode) }; }
        }

        public override void Update()
        {
            chaos.cameraDirector.StartRecenterToHeading();
        }
    }
}
