using FG.Common;
using Il2CppSystem.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class FirstPersonMode : Effect
    {
        public override string Name
        {
            get { return "First Person Mode"; }
        }

        public override int Duration
        {
            get { return 30; }
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
            cameraDirectorTransform.SetParent(null);
            cameraDirectorTransform.position = Vector3.zero;
            cameraDirectorTransform.rotation = new Quaternion(0, 0, 0, 0);
            cameraDirectorTransform.GetChild(1).gameObject.SetActive(true);
            base.End();
        }
    }
}
