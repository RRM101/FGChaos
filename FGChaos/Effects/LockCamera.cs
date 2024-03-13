using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class LockCamera : Effect
    {
        public LockCamera()
        {
            Name = "Lock Camera";
            ID = "LockCamera";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(FirstPersonMode) };
        }

        public override void Update()
        {
            chaos.cameraDirector.StartRecenterToHeading(); // Improve later
        }
    }
}
