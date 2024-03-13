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
    public class PlanetAssault : Effect
    {
        public PlanetAssault()
        {
            Name = "Planet Assault";
            ID = "PlanetAssault";
        }

        public override void Run()
        {
            StartCoroutine(PlanetAssaultCoroutine());
            base.Run();
        }

        IEnumerator PlanetAssaultCoroutine()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("PB_Projectile_Futuristic_Planet");
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                obj.transform.parent = chaos.fallGuy.transform;
                int random_x = UnityEngine.Random.Range(50, 100);
                int random_z = UnityEngine.Random.Range(50, 101);
                int front = UnityEngine.Random.Range(0, 2);
                int multiplier = -1;
                multiplier = front == 1 ? -1 : 1;
                obj.transform.localPosition = new Vector3(0, 1, -5 * multiplier);
                Vector3 velocity = new Vector3(random_x, Math.Max(Math.Abs(random_x), Math.Abs(random_z)), random_z);
                Vector3 velocity2 = velocity.magnitude * chaos.fallGuy.transform.forward * multiplier;
                rigidbody.velocity = new Vector3(velocity2.x, 5, velocity2.z);
                rigidbody.angularVelocity = rigidbody.velocity;
            }
        }
    }
}
