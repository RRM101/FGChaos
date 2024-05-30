using System;
using UnityEngine;

namespace FGChaos.Effects
{
    public class Speed : Effect
    {
        public Speed()
        {
            speed = speeds[UnityEngine.Random.Range(0, speeds.Length)];
            Name = $"{speed}x Speed";
            Duration = 15;
            BlockedEffects = new Type[] { typeof(Speed), typeof(RandomGameSpeed), typeof(SuperHot) };
        }

        float[] speeds = new float[] { 0.2f, 0.5f, 2f, 5f, 10f };
        float speed;

        public override void Run()
        {
            Time.timeScale = speed;
            base.Run();
        }

        public override void End()
        {
            Time.timeScale = 1;
            base.End();
        }
    }
}
