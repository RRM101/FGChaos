using System;
using UnityEngine;

namespace FGChaos.Effects
{
    public class FirstPersonMode : Effect
    {
        public FirstPersonMode()
        {
            Name = "First Person Mode";
            ID = "FirstPersonMode";
            Duration = 30;
            BlockedEffects = new Type[]
            {
                typeof(FirstPersonMode),
                typeof(WitnessProtection),
                typeof(ClonePlayer),
                typeof(WhereIsMyFallGuy),
                typeof(LockCamera),
                typeof(TopDownView)
            };
        }

        Transform cameraDirectorTransform;

        public override void Run()
        {
            cameraDirectorTransform = chaos.cameraDirector.transform;
            cameraDirectorTransform.GetChild(1).gameObject.SetActive(false);
            cameraDirectorTransform.GetChild(0).position = Vector3.zero;
            cameraDirectorTransform.GetChild(0).localPosition = Vector3.zero;
            cameraDirectorTransform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
            cameraDirectorTransform.SetParent(chaos.fallGuy.transform.FindChild("Character/SKELETON/Root/Torso_C_jnt_NoStrechSquash/Chest_C_jnt/Head_C_jnt01/"));
            cameraDirectorTransform.localPosition = new Vector3(0, 2, 0);
            cameraDirectorTransform.rotation = new Quaternion(0, 0, 0, 0);
            base.Run();
        }

        public override void End()
        {
            if (cameraDirectorTransform != null)
            {
                cameraDirectorTransform.SetParent(null);
                cameraDirectorTransform.position = Vector3.zero;
                cameraDirectorTransform.rotation = new Quaternion(0, 0, 0, 0);
                cameraDirectorTransform.GetChild(1).gameObject.SetActive(true);
            }
            base.End();
        }
    }
}
