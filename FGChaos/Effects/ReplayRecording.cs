using FGChaos.MonoBehaviours;
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

        public override void Run()
        {
            recorder = chaos.fallGuy.gameObject.AddComponent<ReplayRecorder>();
            StartCoroutine(PlayRecordingAfter10s());

            base.Run();
        }

        IEnumerator PlayRecordingAfter10s()
        {
            yield return new WaitForSecondsRealtime(10);

            if (isActive)
            {
                recorder.recording = false;
                textMeshPro.text = $"Playing Replay ({Duration})";
            }
        }

        public override void End()
        {
            if (recorder != null)
            {
                recorder.StopRecording();
                UnityEngine.Object.Destroy(recorder);
            }
            base.End();
        }
    }
}
