using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace FGChaos.Effects
{
    public abstract class Effect
    {
        public string Name;

        public int Duration = 0;

        public string ID => GetType().Name;

        public Type[] BlockedEffects = new Type[] {}; // procrastinating making this an Effect array

        public bool destroyEffectName = true;

        public Chaos chaos = ChaosPluginBehaviour.chaosInstance;

        public float actualDuration { get { return (int)(Duration * Time.timeScale); } }

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

            if (Duration == 0 && destroyEffectName)
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

        IEnumerator WaitCoroutine(float seconds)
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

        /// <summary>
        /// Use this instead of base.Run() if you want to call End() manually.
        /// </summary>
        public void RunWithoutEnd()
        {
            AddEffectName();
            isActive = true;
            Chaos.activeEffects.Add(this);
            StartCoroutine(RunUpdate());
            if (Duration == 0 && destroyEffectName)
            {
                StartCoroutine(DestroyEffectName());
            }
        }
    }
}
