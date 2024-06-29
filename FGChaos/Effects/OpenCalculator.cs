using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class OpenCalculator : Effect
    {
        public OpenCalculator()
        {
            Name = "Open Calculator";
        }

        public override void Run()
        {
            System.Diagnostics.Process.Start("calc");
            base.Run();
        }
    }
}
