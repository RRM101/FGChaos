using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class BouncyPlayer : Effect
    {
        public BouncyPlayer()
        {
            Name = "Bouncy Fall Guy";
            Duration = 20;
        }

        PhysicMaterial physicMaterial;

        public override void Run()
        {
            physicMaterial = chaos.fallGuy.GetComponent<CapsuleCollider>().material;
            physicMaterial.bounciness = 1;
            physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;
            base.Run();
        }

        public override void End()
        {
            physicMaterial.bounciness = 0;
            physicMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
            base.End();
        }
    }
}
