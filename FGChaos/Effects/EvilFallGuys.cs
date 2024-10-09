using FG.Common.CMS;
using FGClient;
using FGClient.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class EvilFallGuys : Effect
    {
        public static bool isEvil = false;

        public EvilFallGuys()
        {
            Name = "Evil Fall Guys";
        }

        public override void Run()
        {
            if (!isEvil)
            {
                PlayerInfoDisplayCanvas playerInfoDisplay = GameObject.FindObjectOfType<PlayerInfoDisplayCanvas>();
                playerInfoDisplay.SetText("EVIL " + playerInfoDisplay._text.text);

                GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out ClientGameManager cgm);
                try
                {
                    GameplayInstructionsViewModel gameplayInstructionsViewModel = cgm._inGameUiManager._inGameUiStates[2]._gameplayViewModels[0].Cast<GameplayInstructionsViewModel>();
                    gameplayInstructionsViewModel.ObjectiveText = "EVIL " + gameplayInstructionsViewModel._propertyListeners["ObjectiveText"]._previousValue.ToString();
                }
                catch (Exception e)
                {
                    Plugin.Logs.LogError($"An error occured: {e.GetType().Name}: {e.Message}\n\nStack Trace:\n{e.StackTrace}");
                }
            }

            isEvil = true;
            base.Run();
        }
    }
}
