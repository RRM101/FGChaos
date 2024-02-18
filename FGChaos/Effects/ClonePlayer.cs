using FG.Common;
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
        public override string Name
        {
            get { return "Clone Player"; }
        }

        public override string ID
        {
            get { return "ClonePlayer"; }
        }

        public override void Run()
        {
            GameObject clonedfg = GameObject.Instantiate(chaos.fallGuy.gameObject);
            CameraDirector cameraDirectorChild = clonedfg.GetComponentInChildren<CameraDirector>();
            if (cameraDirectorChild != null)
            {
                GameObject.Destroy(cameraDirectorChild.gameObject);
            }
            clonedfg.transform.GetChild(0).gameObject.SetActive(true);
            FallGuysCharacterController fallGuysCharacter = clonedfg.GetComponent<FallGuysCharacterController>();
            fallGuysCharacter.IsLocalPlayer = true;
            fallGuysCharacter.IsControlledLocally = true;
            GameObject clientPlayerUpdateManagerObject = new GameObject("ClonePlayer");
            ClientPlayerUpdateManager clientPlayerUpdateManager = clientPlayerUpdateManagerObject.AddComponent<ClientPlayerUpdateManager>();
            clientPlayerUpdateManager.RegisterPlayer(clonedfg.GetComponent<FallGuysCharacterController>(), true);
            clientPlayerUpdateManager.GameIsStarting();
            CustomisationManager.Instance.ApplyCustomisationsToFallGuy(clonedfg, GlobalGameStateClient.Instance.PlayerProfile.CustomisationSelections, -1);
            base.Run();
        }
    }
}
