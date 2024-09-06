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
    public class BertCamera : Effect
    {
        public BertCamera()
        {
            Name = "Camera On Bert";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(BertCamera), typeof(FirstPersonMode), typeof(TopDownView), typeof(CinematicCamera) };
        }

        GameObject bert;
        Rigidbody bertrb;
        Vector3 previousPosition;

        public override void Run()
        {
            StartCoroutine(Bert());
            base.Run();
        }

        IEnumerator Bert()
        {
            previousPosition = chaos.fallGuy.transform.position;
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("PB_Penguin_NoScore");
            yield return handle;
            bert = GameObject.Instantiate(handle.Result, previousPosition, new Quaternion(0,0,0,0));
            bertrb = bert.GetComponent<Rigidbody>();
            chaos.cameraDirector.AddCloseCameraTarget(bert, true);
        }

        public override void Update()
        {
            if (bert != null)
            {
                if (bert.transform.position.y < -40)
                {
                    bert.transform.position = previousPosition;
                    bertrb.velocity = Vector3.zero;
                }
            }
        }

        public override void End()
        {
            if (chaos != null)
            {
                chaos.cameraDirector._selectedCloseCameraTarget = chaos.fallGuy.transform;
                chaos.cameraDirector.UseCloseShot();
                chaos.cameraDirector._closeCameraTargets.Remove(bert.transform);
            }

            base.End();
        }
    }
}
