using BepInEx;
using BepInEx.Configuration;
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

        ClientGameManager cgm;
        CustomVolumeWaveProvider volumeWaveProvider;
        float fgMusicVolume;

        public override void Run()
        {
            chaos.fallGuy.transform.GetChild(0).transform.GetChild(0).DOScaleX(5, 1);
            if (Plugin.CustomAudio.Value)
            {
                PlayAudio();
            }
            base.Run();
        }

        void PlayAudio()
        {
            Mp3FileReader mp3File = new Mp3FileReader($"{Plugin.GetModFolder()}/Assets/Audio/wideputin.mp3");
            volumeWaveProvider = new(mp3File);
            volumeWaveProvider.InitAndPlay();
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out cgm);
            cgm._musicInstance.getVolume(out fgMusicVolume);
            cgm._musicInstance.setVolume(0);
        }

        public override void End()
        {
            if (volumeWaveProvider != null)
            {
                volumeWaveProvider.StopAudio();
                if (!cgm.IsShutdown)
                {
                    cgm._musicInstance.setVolume(1);
                }
            }

            if (chaos != null)
            {
                chaos.fallGuy.transform.GetChild(0).transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            }
            base.End();
        }
    }
}
