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
        public string name;
        public int duration = 15;

        public override string Name
        {
            get { return name; }
        }

        public override int Duration
        {
            get { return duration; }
        }

        public override string ID
        {
            get { return "Speed"; }
        }

        public override Type[] BlockedEffects
        {
            get { return new Type[] { typeof(Speed) }; }
        }

        float[] speeds = new float[] { 0.2f, 0.5f, 2f, 5f, 10f };
        float speed;

        public Speed()
        {
            speed = speeds[UnityEngine.Random.Range(0, speeds.Length)];
            name = $"{speed}x Speed";
            duration = (int)(duration * speed);
        }

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
