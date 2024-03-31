using System.Collections;
using UnityEngine;

namespace FGChaos.Effects
{
    public class WhoIsWaving : Effect
    {
        public WhoIsWaving()
        {
            Name = "Who is waving?";
            ID = "WhoIsWaving";
        }

        public override void Run()
        {
            ChaosPluginBehaviour.LoadBank("BNK_Emote_Wave_A");
            StartCoroutine(WhoIsWavingCoroutine());
            isActive = true;
            RunWithoutWait();
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
