using FG.Common.CMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class SomethingHappened : Effect
    {
        public SomethingHappened()
        {
            Name = "Something Happened";
            destroyEffectName = false;
        }

        public override void Run()
        {
            StartCoroutine(ChangeStrings());
            RunWithoutEnd();
        }

        IEnumerator ChangeStrings()
        {
            LocalisedStrings localisedStrings = CMSLoader.Instance._localisedStrings;

            int count = localisedStrings._localisedStrings.Count;
            int done = 0;

            foreach (LocalisedString localisedString in CMSLoader.Instance.CMSData.LocalisedStrings.Values)
            {
                if (isActive)
                {
                    done++;
                    if (!localisedStrings._localisedStrings[localisedString.Id].StartsWith("EVIL "))
                    {
                        localisedStrings._localisedStrings[localisedString.Id] = "EVIL " + localisedStrings._localisedStrings[localisedString.Id];
                    }
                    textMeshPro.text = $"Something Happened ({done}/{count})";
                }
                yield return null;
            }

            Plugin.Logs.LogInfo("Strings Changed");

            yield return new WaitForSecondsRealtime(5);

            if (isActive)
            {
                End();
                GameObject.Destroy(textMeshPro.gameObject);
            }
        }
    }
}
