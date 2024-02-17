using Rewired;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class Jetpack : Effect
    {
        public override string Name
        {
            get { return "Jetpack"; }
        }

        public override int Duration
        {
            get { return 20; }
        }

        public override Type[] BlockedEffects
        {
            get { return new Type[] { typeof(RocketShip) }; }
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
            Chaos.rocketShip = false;
            base.End();
        }
    }
}
