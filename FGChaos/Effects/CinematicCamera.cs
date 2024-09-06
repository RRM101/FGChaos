using Cinemachine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class CinematicCamera : Effect
    {
        public CinematicCamera()
        {
            Name = "Cinematic Camera";
            Duration = 30;
            BlockedEffects = new Type[]
            {
                typeof(FirstPersonMode),
                typeof(LockCamera),
                typeof(TopDownView),
                typeof(BertCamera),
                typeof(CinematicCamera)
            };
        }

        Transform cameraDirectorTransform;
        FallGuysCharacterControllerInput input;
        Camera camera;

        float delay = 5;
        bool zoomed = false;

        public override void Run()
        {
            cameraDirectorTransform = chaos.cameraDirector.transform.GetChild(0);
            cameraDirectorTransform.GetParent().GetChild(1).gameObject.SetActive(false);
            input = chaos.fallGuy.GetComponent<FallGuysCharacterControllerInput>();
            input._camera = input.transform;
            camera = cameraDirectorTransform.GetComponent<Camera>();
            camera.fieldOfView = 30;

            SetCameraPositon();

            base.Run();
        }

        public override void Update()
        {
            cameraDirectorTransform.LookAt(chaos.fallGuy.transform);

            if (delay > 0)
            {
                delay -= Time.deltaTime / Time.timeScale;
            }
            else
            {
                delay = 5;
                SetCameraPositon();
            }

            float cameraDistance = (cameraDirectorTransform.position - chaos.fallGuy.transform.position).magnitude;

            if (cameraDistance > 35 && !zoomed)
            {
                camera.DOFieldOfView(10, 0.2f).SetEase(Ease.InOutCubic);
                zoomed = true;
            }
            else if (cameraDistance < 35 && zoomed)
            {
                camera.DOFieldOfView(30, 0.2f).SetEase(Ease.InOutCubic);
                zoomed = false;
            }

            //camera.fieldOfView = FOV;

            Debug.Log($"Distance: {cameraDistance}");
        }

        public override void End()
        {
            if (chaos != null)
            {
                input._camera = cameraDirectorTransform;
                cameraDirectorTransform.GetParent().GetChild(1).gameObject.SetActive(true);
            }

            base.End();
        }

        void SetCameraPositon()
        {
            Vector3 randompoint = chaos.fallGuy.transform.position + (50 * UnityEngine.Random.insideUnitSphere);
            randompoint.y = chaos.fallGuy.transform.position.y + 20;

            cameraDirectorTransform.position = randompoint;
        }
    }
}
