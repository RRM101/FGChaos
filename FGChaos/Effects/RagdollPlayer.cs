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
        new public string Name = "Ragdoll Player";

        new public int Duration = 5;

        MotorFunctionRagdollStateStunned stateStunned;

        public override void Run()
        {
            stateStunned = chaos.motorAgent.GetMotorFunction<MotorFunctionRagdoll>().GetState<MotorFunctionRagdollStateStunned>();
            stateStunned.Begin(0);
            WaitTillEnd();
            isActive = true;
        }

        public override void End()
        {
            stateStunned.End(0);
            base.End();
        }
    }
}
