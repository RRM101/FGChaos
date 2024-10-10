using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace FGChaos.Effects
{
    public class Camera2D : Effect
    {
        public Camera2D()
        {
            Name = "2D Camera";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(Camera2D), typeof(FirstPersonMode) };
        }

        bool usingDepthOfField;
        DepthOfField depthOfField;

        public override void Run()
        {
            depthOfField = chaos.cameraDirector.MainNativeCam.GetComponent<PostProcessSettingsApplier>()._PCProfile.GetSetting<DepthOfField>();
            usingDepthOfField = depthOfField.active;

            if (usingDepthOfField)
            {
                StartCoroutine(DisableDOF());
            }

            chaos.cameraDirector.MainNativeCam.orthographic = true;
            base.Run();
        }

        public override void End()
        {
            if (chaos != null)
            {
                chaos.cameraDirector.MainNativeCam.orthographic = false;

                if (usingDepthOfField)
                {
                    StartCoroutine(EnableDOF());
                }
            }
            base.End();
        }

        IEnumerator DisableDOF()
        {
            depthOfField.active = false;
            chaos.cameraDirector.MainNativeCam.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
            chaos.cameraDirector.MainNativeCam.gameObject.SetActive(true);
        }

        IEnumerator EnableDOF()
        {
            depthOfField.active = true;
            if (chaos != null)
                chaos.cameraDirector.MainNativeCam.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
            if (chaos != null)
                chaos.cameraDirector.MainNativeCam.gameObject.SetActive(true);
        }
    }
}
