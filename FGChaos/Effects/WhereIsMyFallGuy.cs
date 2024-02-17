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
        public override string Name
        {
            get { return "Where is my Fall Guy?"; }
        }

        public override int Duration
        {
            get { return 15; }
        }

        public override Type[] BlockedEffects
        {
            get { return new Type[] { typeof(FirstPersonMode) }; }
        }

        GameObject model;

        public override void Run()
        {
            model = chaos.fallGuy.gameObject.transform.FindChild("Character").gameObject;
            model.SetActive(false);
            base.Run();
        }

        public override void End()
        {
            model.SetActive(true);
            base.End();
        }
    }
}
