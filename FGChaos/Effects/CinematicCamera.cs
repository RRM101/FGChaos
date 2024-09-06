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
        Vector3 originalCameraPositon;
        bool isAtOriginalCameraPositon = false;
        bool isSettingCameraPosition = false;

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

            bool isMoving = new Vector2(input._rewiredPlayer.GetAxis("Move Horizontal"), input._rewiredPlayer.GetAxis("Move Vertical")).magnitude > 0;

            if (!isMoving)
            {
                cameraDirectorTransform.position += chaos.fallGuy.transform.rotation * new Vector3(0, 0, 0.05f);
                isAtOriginalCameraPositon = false;
                isSettingCameraPosition = false;
            }
            else if (!isAtOriginalCameraPositon && !isSettingCameraPosition)
            {
                cameraDirectorTransform.DOMove(originalCameraPositon, 0.1f).SetEase(Ease.InOutSine);
            }
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
            isSettingCameraPosition = true;
            Vector3 randompoint = chaos.fallGuy.transform.position + (50 * UnityEngine.Random.insideUnitSphere);
            randompoint.y = chaos.fallGuy.transform.position.y + 20;

            cameraDirectorTransform.position = randompoint;
            originalCameraPositon = randompoint;

            if ((cameraDirectorTransform.position - chaos.fallGuy.transform.position).magnitude > 35)
            {
                camera.fieldOfView = 10;
            }
            else
            {
                camera.fieldOfView = 30;
            }
        }
    }
}
