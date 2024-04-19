using FG.Common.LODs;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Collections;
using SRF;

namespace FGChaos.Effects
{
    public class BoulderRain : Effect
    {
        public BoulderRain()
        {
            Name = "Boulder Rain";
            destroyEffectName = false;
        }

        public override void Run()
        {
            StartCoroutine(BoulderRainCoroutine());
            RunWithoutEnd();
        }

        IEnumerator BoulderRainCoroutine()
        {
            int randomSpawnAmount = UnityEngine.Random.Range(10, 50);
            for (int i = 0; i < randomSpawnAmount; i++)
            {
                yield return BoulderRainSpawn();
            }
        }

        IEnumerator BoulderRainSpawn()
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
                yield return new WaitForSeconds(1);
            }
            End();
        }
    }
}
