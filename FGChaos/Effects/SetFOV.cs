using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class SetFOV : Effect
    {
        public SetFOV()
        {
            FOV = UnityEngine.Random.RandomRange(30, 121);
            Name = $"Set FOV to {FOV}";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(SetFOV) };
        }

        int FOV;
        LensSettings lensSettings;

        public override void Run()
        {
            lensSettings = chaos.cameraDirector._closeCamera.m_Lens;

            base.Run();
        }

        public override void Update()
        {
            lensSettings.FieldOfView = FOV;
            chaos.cameraDirector._closeCamera.m_Lens = lensSettings;
        }

        public override void End()
        {
            lensSettings.FieldOfView = 60;
            if (chaos != null)
            {
                chaos.cameraDirector._closeCamera.m_Lens = lensSettings;
            }
            base.End();
        }
    }
}
