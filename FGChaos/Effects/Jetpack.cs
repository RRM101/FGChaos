using Rewired;
using System;

namespace FGChaos.Effects
{
    public class Jetpack : Effect
    {
        public Jetpack()
        {
            Name = "Jetpack (Hold Jump)";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(RocketShip) };
        }

        Player rewiredplayer;

        public override void Run()
        {
            rewiredplayer = chaos.fallGuy.GetComponent<FallGuysCharacterControllerInput>()._rewiredPlayer;
            base.Run();
        }

        public override void Update()
        {
            Chaos.rocketShip = rewiredplayer.GetButton(2);
        }

        public override void End()
        {
            base.End();
            Chaos.rocketShip = false;
        }
    }
}
