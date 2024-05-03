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
        }

        public override void Run()
        {
            StartCoroutine(EliminateCoroutine());
            RunWithoutEnd();
        }

        IEnumerator EliminateCoroutine()
        {
            EliminatedScreenViewModel.Show("eliminated", null, null);
            AudioManager.PlayGameplayEndAudio(false);
            yield return WaitForSeconds(5);
            int randomnumber = UnityEngine.Random.Range(0, 3);
            if (randomnumber == 0)
            {
                yield return WaitForSeconds(1);
                textMeshPro.text = "Fake Eliminate Player"; // improve later
                yield return WaitForSeconds(4);
                End();
            }
            else
            {
                GlobalGameStateClient.Instance._gameStateMachine.ReplaceCurrentState(new StateMainMenu(GlobalGameStateClient.Instance._gameStateMachine, GlobalGameStateClient.Instance.CreateClientGameStateData(), false).Cast<GameStateMachine.IGameState>());
                End();
            }
        }
    }
}
