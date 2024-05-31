using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class InvertedCameraControls : Effect
    {
        public InvertedCameraControls()
        {
            Name = "Inverted Camera Controls";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(InvertedCameraControls) };
        }

        public static bool active;

        public override void Run()
        {
            active = true;
            base.Run();
        }

        public override void End()
        {
            active = false;
            base.End();
        }
    }
}
