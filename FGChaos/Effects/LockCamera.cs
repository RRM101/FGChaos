using System;

namespace FGChaos.Effects
{
    public class LockCamera : Effect
    {
        public LockCamera()
        {
            Name = "Lock Camera";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(FirstPersonMode) };
        }

        public override void Update()
        {
            chaos.cameraDirector.StartRecenterToHeading(); // Improve later
        }
    }
}
