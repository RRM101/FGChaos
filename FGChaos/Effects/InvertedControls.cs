using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class InvertedControls : Effect
    {
        public InvertedControls()
        {
            Name = "Inverted Controls";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(InvertedControls) };
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
