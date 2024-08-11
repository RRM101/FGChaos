using FG.Common.CMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class EvilFallGuys : Effect
    {
        public static bool isEvil = false;

        public EvilFallGuys()
        {
            Name = "Evil Fall Guys";
        }

        public override void Run()
        {
            isEvil = true;
            base.Run();
        }
    }
}
