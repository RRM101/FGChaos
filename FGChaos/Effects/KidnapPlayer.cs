using FG.Common.LODs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using SRF;
using System.Collections;

namespace FGChaos.Effects
{
    public class KidnapPlayer : Effect
    {
        public override string Name
        {
            get { return "Kidnap Player"; }
        }

        public override void Run()
        {
            StartCoroutine(KidnapPlayerCoroutine("PB_Projectile_Futuristic_Hexnut_BigShots"));
            base.Run();
        }

        IEnumerator KidnapPlayerCoroutine(string key)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();

                int random_x = UnityEngine.Random.Range(25, 40);
                int random_z = UnityEngine.Random.Range(25, 41);
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                obj.transform.rotation = chaos.fallGuy.transform.rotation;
                yield return new WaitForSeconds(0.1f);
                obj.transform.parent = chaos.fallGuy.transform;
                obj.transform.localPosition = new Vector3(0, 1, 0);
                Vector3 velocity = new Vector3(random_x, Math.Max(Math.Abs(random_x), Math.Abs(random_z)), random_z);
                Vector3 velocity2 = velocity.magnitude * chaos.fallGuy.transform.forward.normalized;
                rigidbody.velocity = new Vector3(velocity2.x, Math.Max(Math.Abs(random_x), Math.Abs(random_z)), velocity2.z);
                rigidbody.angularVelocity = rigidbody.velocity;

            }
            else
            {
                Debug.Log($"object '{key}' not found");
            }
        }

    }
}
