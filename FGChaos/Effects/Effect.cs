using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace FGChaos.Effects
{
    public class Effect
    {
        public string Name;

        public int Duration = 0;

        public string ID;

        public Type[] BlockedEffects = new Type[] {};

        public Chaos chaos = ChaosPluginBehaviour.chaosInstance;

        public int actualDuration { get { return (int)(Duration * Time.timeScale); } }

        bool canRunUpdateMethod = true;

        public bool isActive;

        public TextMeshProUGUI textMeshPro;

        public virtual void Run()
        {
            AddEffectName();
            isActive = true;
            Chaos.activeEffects.Add(this);
            StartCoroutine(RunUpdate());
            WaitTillEnd();

            if (Duration == 0)
            {
                StartCoroutine(DestroyEffectName());
            }
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public virtual void End()
        {
            isActive = false;
            Chaos.activeEffects.Remove(this);
            canRunUpdateMethod = false;
            if (Duration > 0 && textMeshPro != null)
            {
                GameObject.Destroy(textMeshPro.gameObject);
            }
        }

        public Effect Create()
        {
            Effect effect = (Effect)Activator.CreateInstance(GetType());
            return effect;
        }

        IEnumerator RunUpdate()
        {
            while (canRunUpdateMethod)
            {
                try
                {
                    Update();
                }
                catch (NotImplementedException e)
                {
                    canRunUpdateMethod = false;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occured while running Update method: {e.Message} in effect {ID}");
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

        IEnumerator DestroyEffectName()
        {
            yield return new WaitForSeconds(10);
            if (textMeshPro != null)
            {
                GameObject.Destroy(textMeshPro.gameObject);
            }
        }

        public void AddEffectName()
        {
            textMeshPro = GameObject.Instantiate(ChaosPluginBehaviour.effectName.gameObject, chaos.chaosCanvas.transform.GetChild(1)).GetComponent<TextMeshProUGUI>();
            if (Duration > 0)
            {
                textMeshPro.text = $"{Name} ({Duration}s)";
            }
            else
            {
                textMeshPro.text = Name;
            }
        }

        public void WaitTillEnd()
        {
            StartCoroutine(WaitCoroutine(actualDuration));
        }

        public void StartCoroutine(IEnumerator enumerator)
        {
            ChaosPluginBehaviour.instance.RunCoroutine(enumerator);
        }

        public void RunWithoutWait()
        {
            AddEffectName();
            isActive = true;
            Chaos.activeEffects.Add(this);
            StartCoroutine(RunUpdate());
            if (Duration == 0)
            {
                StartCoroutine(DestroyEffectName());
            }
        }
    }
}
