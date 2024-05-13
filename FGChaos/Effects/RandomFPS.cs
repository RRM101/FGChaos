using FGClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class RandomFPS : Effect
    {
        public RandomFPS()
        {
            Name = "Random FPS";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(SwitchMoment), typeof(RandomFPS) };
        }

        float delay = 0;
        int[] fps = new int[] { 5, 10, 15, 30, 45, 60 };

        public override void Run()
        {
            QualitySettings.vSyncCount = 0;
            base.Run();
        }

        public override void Update()
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime / Time.timeScale;
            }
            else
            {
                delay = 2;
                SetFPS();
            }
        }

        void SetFPS()
        {
            Application.targetFrameRate = fps[UnityEngine.Random.Range(0, fps.Length)];
        }

        public override void End()
        {
            QualitySettings.vSyncCount = GlobalGameStateClient.Instance.PlayerProfile.GraphicsSettings.VSync ? 1 : 0;
            Application.targetFrameRate = GlobalGameStateClient.Instance.PlayerProfile.GraphicsSettings.TargetFrameRate;
            base.End();
        }
    }
}
