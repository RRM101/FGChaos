using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class SuperHot : Effect
    {
        public SuperHot()
        {
            Name = "Super Hot";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(Speed), typeof(RandomGameSpeed), typeof(SuperHot) };
        }

        public static bool active;

        public override void Run()
        {
            active = true;
            base.Run();
        }

        public override void Update()
        {
            float gameSpeed = chaos.fgrb.velocity.magnitude / 8.5f;
            gameSpeed = Math.Max(gameSpeed, 0.1f);
            Time.timeScale = gameSpeed;
        }

        public override void End()
        {
            active = false;
            Time.timeScale = 1;
            base.End();
        }
    }
}
