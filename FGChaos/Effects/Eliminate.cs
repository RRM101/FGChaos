using FGClient;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections;
using FG.Common.CMS;
using FG.Common;
using Events;

namespace FGChaos.Effects
{
    public class Eliminate : Effect
    {
        public Eliminate()
        {
            Name = "Eliminate Player";
            ID = "Eliminate";
        }

        public override void Run()
        {
            StartCoroutine(EliminateCoroutine());
            RunWithoutWait();
        }

        IEnumerator EliminateCoroutine()
        {
            ChaosPluginBehaviour.UnloadBank(CMSLoader.Instance.CMSData.Rounds[NetworkGameData.currentGameOptions_._roundID].IngameMusicSoundBank);
            EliminatedScreenViewModel.Show("eliminated", null, null);
            AudioManager.PlayGameplayEndAudio(false);
            yield return new WaitForSeconds(5);
            int randomnumber = UnityEngine.Random.Range(0, 3);
            if (randomnumber == 0)
            {
                yield return new WaitForSeconds(1);
                textMeshPro.text = "Fake Eliminate Player"; // improve later
                yield return new WaitForSeconds(4);
                End();
            }
            else
            {
                Addressables.LoadSceneAsync("MainMenu");
                Broadcaster.Instance.Broadcast(new OnTransitionToVictoryScreen());
                End();
            }
        }
    }
}
