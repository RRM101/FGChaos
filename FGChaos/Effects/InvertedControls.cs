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

        public override void Run()
        {
            Chaos.invertedControls = true;
            base.Run();
        }

        public override void End()
        {
            Chaos.invertedControls = false;
            base.End();
        }
    }
}
