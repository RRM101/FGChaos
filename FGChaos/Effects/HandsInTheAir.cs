using FG.Common.Character;

namespace FGChaos.Effects
{
    public class HandsInTheAir : Effect
    {
        public HandsInTheAir()
        {
            Name = "Hands In The Air";
            ID = "HandsInTheAir";
            Duration = 10;
        }

        MotorFunctionRagdollStateRollOver stateRollOver;

        public override void Run()
        {
            stateRollOver = chaos.motorAgent.GetMotorFunction<MotorFunctionRagdoll>().GetState<MotorFunctionRagdollStateRollOver>();
            stateRollOver.Begin(0);
            base.Run();
        }

        public override void End()
        {
            stateRollOver.End(0);
            base.End();
        }
    }
}
