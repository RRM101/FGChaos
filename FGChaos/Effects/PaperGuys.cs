using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class PaperGuys : Effect
    {
        public PaperGuys()
        {
            Name = "Paper Guys";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(FirstPersonMode), typeof(PaperGuys), typeof(WideGuys) };
        }

        public override void Run()
        {
            chaos.fallGuy.transform.GetChild(0).transform.GetChild(0).localScale = new Vector3(1, 1, 0.1f);
            base.Run();
        }

        public override void End()
        {
            if (chaos != null)
            {
                chaos.fallGuy.transform.GetChild(0).transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            }
            base.End();
        }
    }
}
