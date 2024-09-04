using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class WKeyStuck : Effect
    {
        public WKeyStuck()
        {
            Name = "Your W Key is stuck";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(WKeyStuck) };
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
