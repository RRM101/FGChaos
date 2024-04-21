using FG.Common.LODs;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Collections;
using SRF;
using Rewired;

namespace FGChaos.Effects
{
    public class Gun : Effect
    {
        public Gun()
        {
            Name = "Gun (Grab to shoot)";
            Duration = 30;
        }

        Player rewiredPlayer;

        public override void Run()
        {
            rewiredPlayer = chaos.fallGuy.GetComponent<FallGuysCharacterControllerInput>()._rewiredPlayer;
            ChaosPluginBehaviour.LoadBank("BNK_SFX_OBJ_Cannon");
            base.Run();
        }

        public override void Update()
        {
            if (rewiredPlayer.GetButtonDown(4))
            {
                AudioManager.PlayOneShot("SFX_OBJ_Cannon_Shoot_Close");
                StartCoroutine(Shoot());
            }
        }

        IEnumerator Shoot()
        {
            string[] boulderKeys = { "PB_Boulder_Large", "PB_Boulder_Large_01", "PB_Boulder_Large_02", "PB_Boulder_Large_03", "PB_Boulder_Large_04" };
            int randomboulder = UnityEngine.Random.Range(0, boulderKeys.Length);
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(boulderKeys[randomboulder]);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                obj.transform.position = new Vector3(chaos.fallGuy.transform.position.x, chaos.fallGuy.transform.position.y + 5, chaos.fallGuy.transform.position.z);
                obj.GetComponent<Rigidbody>().velocity = chaos.fallGuy.transform.rotation * new Vector3(0, 0, 100);

                obj.GetComponent<Rigidbody>().angularVelocity = chaos.fallGuy.transform.rotation * new Vector3(UnityEngine.Random.Range(-100, 200), UnityEngine.Random.Range(-100, 199), UnityEngine.Random.Range(-100, 201));
            }
        }
    }
}
