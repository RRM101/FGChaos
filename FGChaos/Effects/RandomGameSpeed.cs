using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class RandomGameSpeed : Effect
    {
        public RandomGameSpeed()
        {
            Name = "Random Game Speed";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(Speed), typeof(RandomGameSpeed) };
        }

        float delay = 0;
        float[] speeds = new float[] { 0.2f, 0.5f, 2f, 5f, 10f };

        public override void Update()
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime / Time.timeScale;
            }
            else
            {
                delay = 2;
                SetGameSpeed();
            }
        }

        void SetGameSpeed()
        {
            Time.timeScale = speeds[UnityEngine.Random.Range(0, speeds.Length)];
        }

        public override void End()
        {
            Time.timeScale = 1;
            base.End();
        }
    }
}
