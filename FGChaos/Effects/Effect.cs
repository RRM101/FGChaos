using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public abstract class Effect
    {
        public string Name;

        public int Duration;

        public List<Effect> BlockedEffects;

        public abstract void Run();

        public abstract void End();
    }
}
