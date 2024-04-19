using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace FGChaos.Effects
{
    public class TopDownView : Effect
    {
        public TopDownView()
        {
            Name = "Top Down View";
            Duration = 30;
            BlockedEffects = new Type[]
            {
                typeof(FirstPersonMode),
                typeof(LockCamera),
                typeof(TopDownView)
            };
        }

        Transform cameraDirectorTransform;
        GameObject parentGameObject;
        FallGuysCharacterControllerInput input;

        public override void Run()
        {
            parentGameObject = new GameObject("TopDownView Camera Parent");
            cameraDirectorTransform = chaos.cameraDirector.transform.GetChild(0);
            cameraDirectorTransform.GetParent().GetChild(1).gameObject.SetActive(false);
            cameraDirectorTransform.GetParent().SetParent(parentGameObject.transform);
            cameraDirectorTransform.localPosition = Vector3.zero;
            cameraDirectorTransform.GetParent().localPosition = Vector3.zero;
            cameraDirectorTransform.DOLocalMove(new Vector3(0, 20, 0), 1).SetEase(Ease.InOutSine);
            cameraDirectorTransform.DORotate(new Vector3(90, 0, 0), 1).SetEase(Ease.InOutSine);
            input = chaos.fallGuy.GetComponent<FallGuysCharacterControllerInput>();
            input._camera = parentGameObject.transform;

            base.Run();
        }

        public override void Update()
        {
            parentGameObject.transform.position = chaos.fallGuy.transform.position;
        }

        IEnumerator EndCoroutine()
        {
            cameraDirectorTransform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.InOutSine);
            Tween tween = cameraDirectorTransform.GetChild(0).DORotate(chaos.fallGuy.transform.eulerAngles, 1).SetEase(Ease.InOutSine);
            yield return tween.WaitForCompletion();
            input._camera = cameraDirectorTransform;
            cameraDirectorTransform.GetParent().SetParent(null);
            cameraDirectorTransform.position = Vector3.zero;
            cameraDirectorTransform.rotation = new Quaternion(0, 0, 0, 0);
            cameraDirectorTransform.GetParent().GetChild(1).gameObject.SetActive(true);
            GameObject.Destroy(parentGameObject);
        }

        public override void End()
        {
            if (cameraDirectorTransform != null)
            {
                StartCoroutine(EndCoroutine());
            }
            base.End();
        }
    }
}
