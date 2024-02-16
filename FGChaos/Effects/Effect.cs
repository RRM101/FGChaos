using System;
using System.Collections;
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

        public Effect[] BlockedEffects;

        public Chaos chaos = ChaosPluginBehaviour.chaosInstance;

        bool canRunUpdateMethod = true;

        public virtual void Run()
        {
            StartCorutine(RunUpdate());
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public virtual void End()
        {
            throw new NotImplementedException();
        }

        IEnumerator RunUpdate()
        {
            while (canRunUpdateMethod)
            {
                try
                {
                    Update();
                }
                catch
                {
                    canRunUpdateMethod = false;
                }
                yield return null;
            }
        }

        public void StartCorutine(IEnumerator enumerator)
        {
            ChaosPluginBehaviour.instance.RunCoroutine(enumerator);
        }
    }
}
