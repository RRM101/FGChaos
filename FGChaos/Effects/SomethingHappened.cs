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
    public class SomethingHappened : Effect
    {
        public static bool isEvil = false;

        public SomethingHappened()
        {
            Name = "Something Happened";
        }

        public override void Run()
        {
            isEvil = true;
            base.Run();
        }
    }
}
