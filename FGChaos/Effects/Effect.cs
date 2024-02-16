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

        public Chaos chaos = ChaosPluginBehaviour.chaosInstance;

        public abstract void Run();

        public virtual void End()
        {
            throw new NotImplementedException();
        }
    }
}
