using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using SRF;
using FG.Common.LODs;

namespace FGChaos.Effects
{
    public class Boulders : Effect
    {
        public Boulders()
        {
            Name = "Boulders.";
        }

        public override void Run()
        {
            StartCoroutine(SpawnBoulders(chaos.fallGuy.transform.position));
            base.Run();
        }

        IEnumerator SpawnBoulders(Vector3 fgpos)
        {
            Quaternion rotation = Quaternion.Euler(0, chaos.fallGuy.transform.eulerAngles.y, chaos.fallGuy.transform.eulerAngles.z);
            GameObject gameObject = new GameObject("Boulders.");
            gameObject.transform.position = fgpos + rotation * new Vector3(-30, 0, -5);
            gameObject.transform.rotation = rotation;
            int spacing = 6;
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("PB_Boulder_Large");
            yield return handle;
            for (int i = 0; i < 10*spacing; i++)
            {
                i += spacing;
                GameObject boulder = GameObject.Instantiate(handle.Result);
                boulder.RemoveComponentIfExists<LodController>();
                boulder.transform.parent = gameObject.transform;
                boulder.transform.localPosition = new Vector3(i+0.5f, 50, 0);

                for (int j = 0; j < 10*spacing; j++)
                {
                    j += spacing;
                    GameObject boulder2 = GameObject.Instantiate(handle.Result);
                    boulder2.RemoveComponentIfExists<LodController>();
                    boulder2.transform.parent = gameObject.transform;
                    boulder2.transform.localPosition = new Vector3(i+0.5f, 50, j);
                }
            }
        }
    }
}
