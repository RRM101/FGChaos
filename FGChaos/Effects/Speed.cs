using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FGChaos.Effects
{
    public class Speed : Effect
    {
        public Speed()
        {
            speed = speeds[UnityEngine.Random.Range(0, speeds.Length)];
            Name = $"{speed}x Speed";
            ID = "Speed";
            Duration = 15;
            BlockedEffects = new Type[] { typeof(Speed) };
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
