using FGClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections;

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
                End();
            }
        }
    }
}
