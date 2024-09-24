using FG.Common.LODs;
using Rewired;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Collections;
using SRF;

namespace FGChaos.Effects
{
    public class ReverseGun : Effect
    {
        public ReverseGun()
        {
            Name = "Reverse Gun";
            Duration = 20;
            BlockedEffects = new System.Type[] { typeof(ReverseGun) };
        }

        public override void Run()
        {
            FGChaosUtils.LoadBank("BNK_SFX_OBJ_Cannon");
            Chaos.OnJumpActions.Add(DoShoot);
            base.Run();
        }

        void DoShoot()
        {
            AudioManager.PlayOneShot("SFX_OBJ_Cannon_Shoot_Close");
            StartCoroutine(Shoot());
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
                obj.transform.position = new Vector3(chaos.fallGuy.transform.position.x, chaos.fallGuy.transform.position.y + 2, chaos.fallGuy.transform.position.z);
                obj.transform.Translate(chaos.fallGuy.transform.forward * 5, Space.World);
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                rigidbody.velocity = chaos.fallGuy.transform.rotation * new Vector3(0, 0, 100);
                rigidbody.velocity = new Vector3(-rigidbody.velocity.x, rigidbody.velocity.y, -rigidbody.velocity.z);

                rigidbody.angularVelocity = chaos.fallGuy.transform.rotation * new Vector3(UnityEngine.Random.Range(-100, 200), UnityEngine.Random.Range(-100, 199), UnityEngine.Random.Range(-100, 201));
            }
        }

        public override void End()
        {
            if (Chaos.OnJumpActions.Contains(DoShoot))
            {
                Chaos.OnJumpActions.Remove(DoShoot);
            }
            base.End();
        }
    }
}
