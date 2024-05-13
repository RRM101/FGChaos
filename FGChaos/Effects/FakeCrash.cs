using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class FakeCrash : Effect
    {
        public FakeCrash()
        {
            Name = "Fake Crash";
        }

        public override void Run()
        {
            DateTime time = DateTime.Now;
            DateTime endTime = DateTime.Now.AddSeconds(10);

            while (time.Second != endTime.Second)
            {
                time = DateTime.Now;
            }

            base.Run();
        }
    }
}
