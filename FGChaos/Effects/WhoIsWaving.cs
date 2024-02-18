using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class WhoIsWaving : Effect
    {
        public override string Name
        {
            get { return "Who is waving?"; }
        }

        public override string ID
        {
            get { return "WhoIsWaving"; }
        }

        public override void Run()
        {
            ChaosPluginBehaviour.LoadBank("BNK_Emote_Wave_A");
            StartCoroutine(WhoIsWavingCoroutine());
            isActive = true;
        }

        IEnumerator WhoIsWavingCoroutine()
        {
            int woos = UnityEngine.Random.RandomRange(5, 15);
            for (int i = 0; i < woos; i++)
            {
                yield return new WaitForSeconds(2);
                AudioManager.PlayOneShotAttached("SFX_Emote_Wave_A", chaos.fallGuy.gameObject);
            }
            End();
        }
    }
}
