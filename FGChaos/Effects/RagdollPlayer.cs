using FG.Common.Character;

namespace FGChaos.Effects
{
    public class RagdollPlayer : Effect
    {
        public RagdollPlayer()
        {
            Name = "Ragdoll Player";
            Duration = 10;
        }

        MotorFunctionRagdollStateStunned stateStunned;

        public override void Run()
        {
            stateStunned = chaos.motorAgent.GetMotorFunction<MotorFunctionRagdoll>().GetState<MotorFunctionRagdollStateStunned>();
            stateStunned.Begin(0);
            base.Run();
        }

        public override void End()
        {
            if (chaos != null)
            {
                if (chaos.fallGuy != null)
                {
                    stateStunned.End(0);
                }
            }
            base.End();
        }
    }
}
