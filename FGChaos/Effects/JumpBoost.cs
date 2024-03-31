using UnityEngine;

namespace FGChaos.Effects
{
    public class JumpBoost : Effect
    {
        public JumpBoost()
        {
            Name = "Jump Boost";
            ID = "JumpBoost";
            Duration = 15;
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
