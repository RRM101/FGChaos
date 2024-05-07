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
    public class RussianRoulette : Effect
    {
        public RussianRoulette()
        {
            Name = "Russian Roulette";
        }

        public override void Run()
        {
            int random = UnityEngine.Random.Range(0, 6);
            ChaosPluginBehaviour.LoadBank("BNK_SFX_OBJ_Cannon");
            StartCoroutine(Fate(random));

            base.Run();
        }

        IEnumerator Fate(int number)
        {
            yield return WaitForSeconds(5);
            if (number == 5)
            {
                textMeshPro.text = "You will die.";
                textMeshPro.color = Color.red;
                yield return WaitForSeconds(2);
                yield return Shoot();
                yield return WaitForSeconds(1);
                Application.Quit();
            }
            else
            {
                textMeshPro.text = "You got lucky.";
            }
        }

        IEnumerator Shoot() // totally didnt copy paste this from reverse gun
        {
            AudioManager.PlayOneShot("SFX_OBJ_Cannon_Shoot_Close");
            string[] boulderKeys = { "PB_Boulder_Large", "PB_Boulder_Large_01", "PB_Boulder_Large_02", "PB_Boulder_Large_03", "PB_Boulder_Large_04" };
            int randomboulder = UnityEngine.Random.Range(0, boulderKeys.Length);
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(boulderKeys[randomboulder]);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                obj.transform.position = new Vector3(chaos.fallGuy.transform.position.x, chaos.fallGuy.transform.position.y + 2, chaos.fallGuy.transform.position.z);
                obj.transform.Translate(chaos.fallGuy.transform.forward * 5, Space.World);
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                rigidbody.velocity = chaos.fallGuy.transform.rotation * new Vector3(0, 0, 100);
                rigidbody.velocity = new Vector3(-rigidbody.velocity.x, rigidbody.velocity.y, -rigidbody.velocity.z);

                rigidbody.angularVelocity = chaos.fallGuy.transform.rotation * new Vector3(UnityEngine.Random.Range(-100, 200), UnityEngine.Random.Range(-100, 199), UnityEngine.Random.Range(-100, 201));
            }
        }
    }
}
