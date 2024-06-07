using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace FGChaos.Effects
{
    public class BertTeleport : Effect
    {
        public BertTeleport()
        {
            Name = "Teleport to Bert in 15";
            destroyEffectName = false;
        }

        GameObject bert;
        float teleportTime = 16f;

        public override void Run()
        {
            StartCoroutine(SpawnBert());
            RunWithoutEnd();
        }

        IEnumerator SpawnBert()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("PB_Penguin_NoScore");
            yield return handle;
            bert = GameObject.Instantiate(handle.Result, chaos.fallGuy.transform.position, new Quaternion(0, 0, 0, 0));
        }

        public override void Update()
        {
            teleportTime -= Time.unscaledDeltaTime;
            if (teleportTime > 6)
            {
                textMeshPro.text = $"Teleport to Bert in {(int)teleportTime}";
            }
            else
            {
                textMeshPro.text = $"Teleport to Bert in <color=red>{(int)teleportTime}</color>";
            }

            if (teleportTime < 0)
            {
                End();
            }
        }

        public override void End()
        {
            if (chaos != null)
            {
                chaos.fallGuy.transform.position = bert.transform.position;
            }

            if (bert != null)
            {
                GameObject.Destroy(bert);
            }

            if (textMeshPro != null)
            {
                GameObject.Destroy(textMeshPro.gameObject);
            }

            base.End();
        }
    }
}
