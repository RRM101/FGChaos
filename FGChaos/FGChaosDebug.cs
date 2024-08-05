using FG.Common;
using FGChaos.Effects;
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
        bool showDebugMenu = false;

        string nextEffectID 
        { 
            get 
            {
                return ChaosPluginBehaviour.chaosInstance != null ? ChaosPluginBehaviour.chaosInstance.nextEffect.ID : "null";
            } 
        }

        string activeEffects
        {
            get
            {
                List<string> activeEffectIDs = new();
                
                foreach (Effect effect in Chaos.activeEffects)
                {
                    activeEffectIDs.Add(effect.ID);
                }

                return Chaos.activeEffects.Count > 0 ? string.Join("\n", activeEffectIDs.ToArray()) : "null";
            }
        }

        string command;
        bool runCommand;

        void OnGUI()
        {
            if (showDebugMenu)
            {
                GUI.Label(new Rect(5, 5, Screen.width, Screen.height), $"<size=25>Debug FGChaos v{Plugin.version}\nRoundID: {NetworkGameData.currentGameOptions_._roundID}\nSeed: {GlobalGameStateClient.Instance.GameStateView.RoundRandomSeed}\nNextEffect: {nextEffectID}\n\nActiveEffects:\n{activeEffects}</size>");

                GUI.Box(new Rect(Screen.width - 265, 10f, 260f, 75f), "");
                command = GUI.TextField(new Rect(Screen.width - 255, 20f, 240f, 25f), command);
                runCommand = GUI.Button(new Rect(Screen.width - 255, 50f, 240f, 25f), "Run");
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                showDebugMenu = !showDebugMenu;
            }

            if (runCommand)
            {
                Run(command);
            }
        }

        void Run(string s)
        {
            string[] strings = s.Split(' ');
            string command = strings[0];
            string arg = strings[1];

            switch (command)
            {
                case "effect":
                    SetNextEffect(arg);
                    break;
                default:
                    break;

            }
        }

        void SetNextEffect(string effectID)
        {
            Effect selectedEffect = null;
            foreach (Effect effect in EffectList.effects)
            {
                if (effect.ID == effectID)
                {
                    selectedEffect = effect;
                    break;
                }
            }

            if (selectedEffect != null && ChaosPluginBehaviour.chaosInstance != null)
            {
                ChaosPluginBehaviour.chaosInstance.nextEffect = selectedEffect;
            }
        }
    }
}
