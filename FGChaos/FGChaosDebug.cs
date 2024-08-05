using FG.Common;
using FGClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos
{
    public class FGChaosDebug : MonoBehaviour
    {
        string nextEffectID 
        { 
            get 
            {
                return ChaosPluginBehaviour.chaosInstance != null ? ChaosPluginBehaviour.chaosInstance.nextEffect.ID : "null";
            } 
        }

        void OnGUI()
        {
            GUI.Label(new Rect(5, 5, Screen.width, Screen.height), $"<size=25>FGChaos v{Plugin.version}\nRoundID: {NetworkGameData.currentGameOptions_._roundID}\nSeed: {GlobalGameStateClient.Instance.GameStateView.RoundRandomSeed}\nNextEffect: {nextEffectID}</size>");
        }
    }
}
