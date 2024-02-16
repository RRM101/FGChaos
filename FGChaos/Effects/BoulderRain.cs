using FG.Common.LODs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Collections;
using SRF;

namespace FGChaos.Effects
{
    public class BoulderRain : Effect
    {
        new public string Name = "Boulder Rain";

        int randomSpawnAmount;

        public override void Run()
        {
            randomSpawnAmount = UnityEngine.Random.Range(5, 20);

            base.Run();
        }

        public override void Update()
        {
            randomSpawnAmount = UnityEngine.Random.Range(5, 20);
            randomSpawnAmount--;
            if (randomSpawnAmount > 0)
            {
                BoulderRainSpawn();
            }
            else
            {
                End();
            }
        }

        void BoulderRainSpawn()
        {
            StartCorutine(BoulderRainSpawnCorutine());
        }

        IEnumerator BoulderRainSpawnCorutine()
        {
            Vector3 randompoint = chaos.fallGuy.transform.position + (Vector3)(20 * UnityEngine.Random.insideUnitCircle);
            int randomy = UnityEngine.Random.Range(50, 100);
            Vector3 randomPosition = new Vector3(randompoint.x, chaos.fallGuy.transform.position.y + randomy, randompoint.z);
            string[] boulderKeys = { "PB_Boulder_Large", "PB_Boulder_Large_01", "PB_Boulder_Large_02", "PB_Boulder_Large_03", "PB_Boulder_Large_04" };
            int randomboulder = UnityEngine.Random.Range(0, boulderKeys.Length);
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(boulderKeys[randomboulder]);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                obj.transform.position = randomPosition;
                yield return new WaitForSeconds(5);
            }
        }
    }
}
