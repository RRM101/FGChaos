using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class HeavenTeleport : Effect
    {
        public HeavenTeleport()
        {
            Name = "Teleport to Heaven";
        }

        public override void Run()
        {
            int random = UnityEngine.Random.Range(0, 2);

            if (random == 0)
            {
                StartCoroutine(Fake(chaos.fallGuy.transform.position));
            }

            chaos.fallGuy.transform.position = new Vector3(chaos.fallGuy.transform.position.x, chaos.fallGuy.transform.position.y + 1000, chaos.fallGuy.transform.position.z);
            base.Run();
        }


        IEnumerator Fake(Vector3 previousPosition)
        {
            yield return WaitForSeconds(5);
            chaos.fallGuy.transform.position = previousPosition;
            textMeshPro.text = "Fake Teleport to Heaven";
        }
    }
}
