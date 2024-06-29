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
        public PlayIntroCamera()
        {
            Name = "Play Intro Camera";
            Duration = chaos == null ? 0 : (int)Math.Round(chaos.cameraDirector.IntroCamerasDuration);
            BlockedEffects = new Type[] { typeof(PlayIntroCamera), typeof(FirstPersonMode) };
        }

        public override void Run()
        {
            chaos.cameraDirector._cachedCurrentCamera.gameObject.SetActive(false);
            chaos.cameraDirector.IntroCams.GetComponent<Animator>().enabled = true;
            chaos.cameraDirector.IntroCams.Play(1);
            base.Run();
        }

        public override void End()
        {
            if (chaos != null)
            {
                chaos.cameraDirector._cachedCurrentCamera.gameObject.SetActive(true);
                chaos.cameraDirector.IntroCams.gameObject.SetActive(false);
            }
            base.End();
        }
    }
}
