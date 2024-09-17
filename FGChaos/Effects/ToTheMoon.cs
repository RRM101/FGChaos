using BepInEx.Configuration;
using FGClient;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class ToTheMoon : Effect
    {
        public ToTheMoon()
        {
            Name = "To The Moon";
            Duration = 20;
            BlockedEffects = new Type[]
            {
                typeof(ToTheMoon),
                typeof(Gravity)
            };
        }

        //WaveOutEvent waveOut;
        ClientGameManager cgm;
        CustomVolumeWaveProvider volumeWaveProvider;
        float fgMusicVolume;

        public override void Run()
        {
            Physics.gravity = new Vector3(0, 1, 0);
            if (Plugin.CustomAudio.Value)
            {
                PlayAudio();
            }
            base.Run();
        }

        void PlayAudio()
        {
            Mp3FileReader mp3File = new Mp3FileReader($"{Plugin.GetModFolder()}/Assets/Audio/tothemoon.mp3");
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

            Physics.gravity = new Vector3(0, -30, 0);
            base.End();
        }
    }
}
