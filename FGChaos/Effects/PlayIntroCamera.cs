using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class PlayIntroCamera : Effect
    {
        public PlayIntroCamera() // wont be in 1.1
        {
            Name = "Play Intro Camera";
        }

        public override void Run()
        {
            StartCoroutine(PlayIntroCam());
            base.Run();
        }

        IEnumerator PlayIntroCam()
        {
            chaos.cameraDirector._cachedCurrentCamera.gameObject.SetActive(false);
            chaos.cameraDirector.IntroCams.GetComponent<Animator>().enabled = true;
            chaos.cameraDirector.IntroCams.Play(1);
            yield return new WaitForSeconds(chaos.cameraDirector.IntroCamerasDuration);
            if (chaos != null)
            {
                chaos.cameraDirector._cachedCurrentCamera.gameObject.SetActive(true);
                chaos.cameraDirector.IntroCams.gameObject.SetActive(false);
            }
        }
    }
}
