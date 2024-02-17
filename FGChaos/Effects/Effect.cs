using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class Effect
    {
        public virtual string Name { get; }

        public virtual int Duration { get { return 0; } }

        public virtual Type[] BlockedEffects { get { return new Type[] {}; } }

        public Chaos chaos = ChaosPluginBehaviour.chaosInstance;

        bool canRunUpdateMethod = true;

        public bool isActive;

        public virtual void Run()
        {
            isActive = true;
            chaos.activeEffects.Add(this);
            StartCoroutine(RunUpdate());
            WaitTillEnd();
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public virtual void End()
        {
            isActive = false;
            chaos.activeEffects.Remove(this);
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
            StartCoroutine(WaitCoroutine(Duration));
        }

        public void StartCoroutine(IEnumerator enumerator)
        {
            ChaosPluginBehaviour.instance.RunCoroutine(enumerator);
        }

        public void RunWithoutWait()
        {
            isActive = true;
            StartCoroutine(RunUpdate());
        }
    }
}
