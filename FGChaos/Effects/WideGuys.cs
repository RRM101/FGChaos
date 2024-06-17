using BepInEx;
using DG.Tweening;
using FGClient;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class WideGuys : Effect
    {
        public WideGuys()
        {
            Name = "Wide Guys";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(FirstPersonMode), typeof(PaperGuys), typeof(WideGuys) };
        }

        WaveOutEvent waveOut;
        ClientGameManager cgm;
        float fgMusicVolume;

        public override void Run()
        {
            chaos.fallGuy.transform.GetChild(0).transform.GetChild(0).DOScaleX(5, 1);
            PlayAudio();
            base.Run();
        }

        void PlayAudio()
        {
            waveOut = new WaveOutEvent();
            Mp3FileReader mp3File = new Mp3FileReader($"{Paths.PluginPath}/FGChaos/Assets/Audio/wideputin.mp3");
            waveOut.Init(mp3File);
            waveOut.Play();
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out cgm);
            cgm._musicInstance.getVolume(out fgMusicVolume);
            cgm._musicInstance.setVolume(0.3f);
        }

        void StopAudio()
        {
            waveOut.Stop();
            waveOut.Dispose();
            if (!cgm.IsShutdown)
            {
                cgm._musicInstance.setVolume(fgMusicVolume);
            }
        }

        public override void End()
        {
            StopAudio();
            if (chaos != null)
            {
                chaos.fallGuy.transform.GetChild(0).transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            }
            base.End();
        }
    }
}
