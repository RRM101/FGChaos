using FGChaos.MonoBehaviours;
using FGClient;
using FGClient.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class ReplayRecording : Effect
    {
        public ReplayRecording()
        {
            Name = "Recording Replay";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(ReplayRecording) };
        }

        ReplayRecorder recorder;
        ClientGameManager cgm;

        public override void Run()
        {
            recorder = chaos.fallGuy.gameObject.AddComponent<ReplayRecorder>();
            StartCoroutine(PlayRecordingAfter10s());
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out cgm);


            base.Run();
        }

        IEnumerator PlayRecordingAfter10s()
        {
            yield return new WaitForSecondsRealtime(Duration/2);

            if (isActive)
            {
                recorder.recording = false;
                textMeshPro.text = $"Playing Replay ({Duration}s)";
                SetUIState(true);
            }
        }

        void SetUIState(bool spectator)
        {
            cgm._inGameUiManager._switchableView._views[4].GetComponentInChildren<GameplayQualificationStatusPromptViewModel>().UpdateDisplay(true, false);
            cgm._inGameUiManager._switchableView._views[4].GetComponentInChildren<NameTagViewModel>().UpdateDisplay(GlobalGameStateClient.Instance.GetLocalPlayerKey(), "", GlobalGameStateClient.Instance._playerProfile.CustomisationSelections);

            if (spectator)
            {
                cgm._inGameUiManager._switchableView.SetViewVisibility(2, false);
                cgm._inGameUiManager._switchableView.SetViewVisibility(4, true);
            }
            else
            {
                cgm._inGameUiManager._switchableView.SetViewVisibility(2, true);
                cgm._inGameUiManager._switchableView.SetViewVisibility(4, false);
            }
        }

        public override void End()
        {
            if (recorder != null)
            {
                recorder.StopRecording();
                UnityEngine.Object.Destroy(recorder);
            }

            if (!cgm.IsShutdown)
            {
                SetUIState(false);
            }
            base.End();
        }
    }
}
