﻿using System;
using UnityEngine;

namespace FGChaos.Effects
{
    public class WhereIsMyFallGuy : Effect
    {
        public WhereIsMyFallGuy()
        {
            Name = "Where is my Fall Guy?";
            Duration = 15;
            BlockedEffects = new Type[] { typeof(FirstPersonMode), typeof(PlayAsBert) };
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
            if (model != null)
            {
                model.SetActive(true);
            }
            base.End();
        }
    }
}
