using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public abstract class Effect
    {
        public string Name;

        public int Duration;

        public static Type[] BlockedEffects;

        public Chaos chaos = ChaosPluginBehaviour.chaosInstance;

        bool canRunUpdateMethod = true;

        public static bool isActive;

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
            isActive = false;
            canRunUpdateMethod = false;
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

        IEnumerator WaitCoroutine(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            End();
        }

        public void WaitTillEnd()
        {
            StartCorutine(WaitCoroutine(Duration));
        }

        public void StartCorutine(IEnumerator enumerator)
        {
            ChaosPluginBehaviour.instance.RunCoroutine(enumerator);
        }
    }
}
