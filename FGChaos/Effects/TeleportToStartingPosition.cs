﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class TeleportToStartingPosition : Effect
    {
        new public string Name = "Teleport to Start";

        public override void Run()
        {
            chaos.fallGuy.transform.position = chaos.startingPosition.transform.position;
            chaos.fallGuy.transform.rotation = chaos.startingPosition.transform.rotation;
        }
    }
}