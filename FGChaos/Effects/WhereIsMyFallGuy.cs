using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class WhereIsMyFallGuy : Effect
    {
        new public string Name = "Where is my Fall Guy?";

        new public int Duration = 15;

        GameObject model;

        public override void Run()
        {
            model = chaos.fallGuy.gameObject.transform.FindChild("Character").gameObject;
            model.SetActive(false);
            WaitTillEnd(Duration);
            isActive = true;
        }

        public override void End()
        {
            model.SetActive(true);
            base.End();
        }
    }
}
