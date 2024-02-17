using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class Speed : Effect
    {
        public string name;

        public override string Name
        {
            get { return name; }
        }

        public override int Duration
        {
            get { return 15; }
        }

        public override Type[] BlockedEffects
        {
            get { return new Type[] { typeof(Speed) }; }
        }

        float[] speeds = new float[] { 0.2f, 0.5f };
        float speed;

        public Speed()
        {
            speed = speeds[UnityEngine.Random.Range(0, speeds.Length)];
            name = $"{speed}x Speed";
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
