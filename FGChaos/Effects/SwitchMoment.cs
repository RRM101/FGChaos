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
            BlockedEffects = new System.Type[] { typeof(SwitchMoment), typeof(RandomFPS) };
        }

        public static bool active;

        public override void Run()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 15;
            active = true;
            base.Run();
        }

        public override void End()
        {
            QualitySettings.vSyncCount = GlobalGameStateClient.Instance.PlayerProfile.GraphicsSettings.VSync ? 1 : 0;
            Application.targetFrameRate = GlobalGameStateClient.Instance.PlayerProfile.GraphicsSettings.TargetFrameRate;
            active = false;
            base.End();
        }
    }
}
