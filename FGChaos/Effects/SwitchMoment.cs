using FGClient;
using UnityEngine;

namespace FGChaos.Effects
{
    public class SwitchMoment : Effect
    {
        public SwitchMoment()
        {
            Name = "Switch Moment";
            Duration = 30;
            BlockedEffects = new System.Type[] { typeof(SwitchMoment) };
        }

        public override void Run()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 15;
            Chaos.switchMode = true;
            base.Run();
        }

        public override void End()
        {
            QualitySettings.vSyncCount = GlobalGameStateClient.Instance.PlayerProfile.GraphicsSettings.VSync ? 1 : 0;
            Application.targetFrameRate = GlobalGameStateClient.Instance.PlayerProfile.GraphicsSettings.TargetFrameRate;
            Chaos.switchMode = false;
            base.End();
        }
    }
}
