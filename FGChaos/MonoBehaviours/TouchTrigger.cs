using BepInEx.Unity.IL2CPP.Utils.Collections;
using FG.Common.LODs;
using SRF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FGChaos.MonoBehaviours
{
    public class TouchTrigger : MonoBehaviour
    {
        float timer = 1;

        void Update()
        {
            timer -= Time.deltaTime;
            timer = Math.Clamp(timer, 0, 1);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.GetComponent<Rigidbody>() == null && timer == 0)
            {
                timer = 1;
                if (collision.contactCount > 0)
                {
                    StartCoroutine(Shoot(collision).WrapToIl2Cpp());
                }
            }
        }

        IEnumerator Shoot(Collision collision)
        {
            string[] boulderKeys = { "PB_Boulder_Large", "PB_Boulder_Large_01", "PB_Boulder_Large_02", "PB_Boulder_Large_03", "PB_Boulder_Large_04" };
            int randomboulder = UnityEngine.Random.Range(0, boulderKeys.Length);
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(boulderKeys[randomboulder]);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                obj.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                obj.transform.Translate(collision.GetContact(0).normal * 3, Space.World);
                obj.transform.LookAt(transform.position);
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                rigidbody.velocity = obj.transform.rotation * new Vector3(0, 0, 100);

                rigidbody.angularVelocity = transform.rotation * new Vector3(UnityEngine.Random.Range(-100, 200), UnityEngine.Random.Range(-100, 199), UnityEngine.Random.Range(-100, 201));
            }
        }
    }
}
