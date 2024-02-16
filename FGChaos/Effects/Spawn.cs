using FG.Common.LODs;
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

namespace FGChaos.Effects
{
    public class Spawn : Effect
    {
        public string SpawnName;
        public string SpawnGUID;
        public string name;

        public override string Name
        {
            get { return name; }
            set { }
        }

        public Spawn()
        {
            int randomnumber = UnityEngine.Random.Range(0, chaos.addressableAssetsNames.Length);

            SpawnName = chaos.addressableAssetsNames[randomnumber];
            SpawnGUID = chaos.addressableAssetsKeyNamePairs[chaos.addressableAssetsNames[randomnumber]];
            name = $"Spawn {chaos.addressableAssetsNames[randomnumber]}";
        }

        public override void Run()
        {
            StartCorutine(InstantiateAddressableObject(SpawnGUID));
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
