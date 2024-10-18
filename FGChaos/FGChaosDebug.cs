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
                if (ChaosManager.chaosInstance != null)
                {
                    if (ChaosManager.chaosInstance.nextEffect != null)
                    {
                        return ChaosManager.chaosInstance.nextEffect.ID;
                    }
                }
                return "null";
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

        static string HelpText => "\nFGChaos Debug Commands\neffect <Effect ID> - Sets the next effect to the specified ID\nruneffect <Effect ID> <delay> - Runs the effect with a delay seperate from the progress bar\nhelp - Shows this";

        void OnGUI()
        {
            if (showDebugMenu)
            {
                GUI.Label(new Rect(5, 5, Screen.width, Screen.height), $"<size=25>Debug FGChaos v{Plugin.version}\nFPS: {(int)(1/Time.deltaTime)}\nRoundID: {NetworkGameData.currentGameOptions_._roundID}\nSeed: {GlobalGameStateClient.Instance.GameStateView.RoundRandomSeed}\nNextEffect: {nextEffectID}\n\nActiveEffects:\n{activeEffects}</size>");

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
            string command = strings[0].ToLower();

            switch (command)
            {
                case "effect":
                    SetNextEffect(strings[1]);
                    break;
                case "help":
                    LogHelpText();
                    break;
                case "runeffect":
                    RunEffectWithDelay(strings);
                    break;
                default:
                    break;

            }
        }

        void RunEffectWithDelay(string[] args)
        {
            if (args.Length >= 3 && ChaosManager.chaosInstance != null)
            {
                bool parseSuccess = float.TryParse(args[2], out float delay);
                if (parseSuccess)
                    ChaosManager.chaosInstance.RunEffectWithDelay(FGChaosUtils.GetEffectForID(args[1]), delay);
            }
        }

        void SetNextEffect(string effectID)
        {
            Effect selectedEffect = FGChaosUtils.GetEffectForID(effectID);

            if (selectedEffect != null && ChaosManager.chaosInstance != null)
            {
                ChaosManager.chaosInstance.nextEffect = selectedEffect;
            }
        }

        void LogHelpText()
        {
            Plugin.Logs.LogMessage(HelpText);
        }
    }
}
