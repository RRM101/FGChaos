using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    /*
        Effect Idea by dubtoshi
    */

    public class InvertedControls : Effect
    {
        public InvertedControls()
        {
            Name = "Inverted Controls";
            Duration = 20;
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
