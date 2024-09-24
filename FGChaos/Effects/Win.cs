using Events;
using FG.Common;
using FG.Common.CMS;
using FGClient;
using FGClient.UI;
using System;

namespace FGChaos.Effects
{
    public class Win : Effect
    {
        public Win()
        {
            Name = "Win";
        }

        public override void Run()
        {
            ClientPlayerManager clientPlayerManager = GlobalGameStateClient.Instance._clientPlayerManager;
            FGChaosUtils.UnloadBank(CMSLoader.Instance.CMSData.Rounds[NetworkGameData.currentGameOptions_._roundID].IngameMusicSoundBank);
            

            if (clientPlayerManager._players.Count == 0)
            {
                PlayerMetadata playerMetadata = new PlayerMetadata(GlobalGameStateClient.Instance.PlayerProfile.CustomisationSelections, -1, "", "", true);

                clientPlayerManager.RegisterLocalPlayer(0, null, GlobalGameStateClient.Instance.PlayerProfile.PlatformAccountName, true);
                clientPlayerManager._playerMetadata.Add(0, playerMetadata);
            }

            FGChaosUtils.LoadBank("BNK_SFX_WinnerScreen");
            FGChaosUtils.LoadBank("BNK_Music_GP");

            Action action = SwitchToVictoryScreen;

            AudioManager.PlayGameplayEndAudio(true);
            WinnerScreenViewModel.Show("winner", true, action);

            base.Run();
        }

        void SwitchToVictoryScreen()
        {
            GlobalGameStateClient.Instance.SwitchToVictoryScreen(0);
            Broadcaster.Instance.Broadcast(new OnTransitionToVictoryScreen());
        }
    }
}
