using FG.Common.LODs;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using SRF;

namespace FGChaos.Effects
{
    public class Spawn : Effect
    {
        public Spawn()
        {
            int randomnumber = UnityEngine.Random.Range(0, chaos.addressableAssetsNames.Length);

            SpawnName = chaos.addressableAssetsNames[randomnumber];
            SpawnGUID = chaos.addressableAssetsKeyNamePairs[SpawnName];
            Name = $"Spawn {SpawnName}";
            ID = "Spawn";
        }

        public string SpawnName;
        public string SpawnGUID;

        public override void Run()
        {
            StartCoroutine(InstantiateAddressableObject(SpawnGUID));
            base.Run();
        }

        IEnumerator InstantiateAddressableObject(string key)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                if (chaos.fallGuy != null)
                {
                    obj.transform.position = chaos.fallGuy.transform.position;
                }
            }
            else
            {
                Debug.Log($"object '{key}' not found");
            }
        }
    }
}
