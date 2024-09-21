using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class GlitchyPlayer : Effect
    {
        public GlitchyPlayer()
        {
            Name = "Glitchy Fall Guy";
            Duration = 10;
            BlockedEffects = new Type[] { typeof(GlitchyPlayer) };
        }

        public override void Run()
        {
            chaos.fallGuy.transform.GetChild(0).localPosition = new Vector3(3, 3, 3);
            base.Run();
        }

        public override void End()
        {
            if (chaos != null)
            {
                chaos.fallGuy.transform.GetChild(0).localPosition = Vector3.zero;
            }
            base.End();
        }
    }
}
