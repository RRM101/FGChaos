using FG.Common.Character;
using FG.Common.Character.MotorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class RagdollPlayer : Effect
    {
        public RagdollPlayer()
        {
            Name = "Ragdoll Player";
            ID = "RagdollPlayer";
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
            stateStunned.End(0);
            base.End();
        }
    }
}
