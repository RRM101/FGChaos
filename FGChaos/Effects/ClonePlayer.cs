﻿using FG.Common;
using FGClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class ClonePlayer : Effect
    {
        new public string Name = "Clone Player";

        public override void Run()
        {
            GameObject clonedfg = GameObject.Instantiate(chaos.fallGuy.gameObject);
            CameraDirector cameraDirectorChild = clonedfg.GetComponentInChildren<CameraDirector>();
            if (cameraDirectorChild != null)
            {
                GameObject.Destroy(cameraDirectorChild.gameObject);
            }
            FallGuysCharacterController fallGuysCharacter = clonedfg.GetComponent<FallGuysCharacterController>();
            fallGuysCharacter.IsLocalPlayer = true;
            fallGuysCharacter.IsControlledLocally = true;
            GameObject clientPlayerUpdateManagerObject = new GameObject("ClonePlayer");
            ClientPlayerUpdateManager clientPlayerUpdateManager = clientPlayerUpdateManagerObject.AddComponent<ClientPlayerUpdateManager>();
            clientPlayerUpdateManager.RegisterPlayer(clonedfg.GetComponent<FallGuysCharacterController>(), true);
            clientPlayerUpdateManager.GameIsStarting();
            CustomisationManager.Instance.ApplyCustomisationsToFallGuy(clonedfg, GlobalGameStateClient.Instance.PlayerProfile.CustomisationSelections, -1);
        }
    }
}