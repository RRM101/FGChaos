using FGClient;
using UnityEngine;

namespace FGChaos.Effects
{
    /*
        Effect Idea by dubtoshi
    */

    public class SwitchMoment : Effect
    {
        public SwitchMoment()
        {
            Name = "Switch Moment";
            Duration = 30;
        }

        public override void Run()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 15;
            base.Run();
        }

        public override void End()
        {
            QualitySettings.vSyncCount = GlobalGameStateClient.Instance.PlayerProfile.GraphicsSettings.VSync ? 1 : 0;
            Application.targetFrameRate = GlobalGameStateClient.Instance.PlayerProfile.GraphicsSettings.TargetFrameRate;
            base.End();
        }
    }
}
