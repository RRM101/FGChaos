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

        WaveOutEvent waveOut;
        ClientGameManager cgm;
        VolumeWaveProvider16 volumeWaveProvider;
        float fgMusicVolume;

        public override void Run()
        {
            Physics.gravity = new Vector3(0, 1, 0);
            if (Plugin.CustomAudio.Value)
            {
                PlayAudio();
                Plugin.CustomAudioVolume.SettingChanged += CustomAudioVolumeSettingChanged;
            }
            base.Run();
        }

        void PlayAudio()
        {
            waveOut = new WaveOutEvent();
            Mp3FileReader mp3File = new Mp3FileReader($"{Plugin.GetModFolder()}/Assets/Audio/tothemoon.mp3");
            volumeWaveProvider = new VolumeWaveProvider16(mp3File);
            volumeWaveProvider.Volume = Math.Min((float)Plugin.CustomAudioVolume.Value / 100, 100);
            waveOut.Init(volumeWaveProvider);
            waveOut.Play();
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out cgm);
            cgm._musicInstance.getVolume(out fgMusicVolume);
            cgm._musicInstance.setVolume(0);
        }

        void CustomAudioVolumeSettingChanged(object sender, EventArgs eventArgs)
        {
            SettingChangedEventArgs settingChangedEventArgs = (SettingChangedEventArgs)eventArgs;
            float volume = Math.Min((float)Convert.ToInt32(settingChangedEventArgs.ChangedSetting.BoxedValue) / 100, 100);
            volumeWaveProvider.Volume = volume;
        }

        void StopAudio()
        {
            waveOut.Stop();
            waveOut.Dispose();
            Plugin.CustomAudioVolume.SettingChanged -= CustomAudioVolumeSettingChanged;
            if (!cgm.IsShutdown)
            {
                cgm._musicInstance.setVolume(fgMusicVolume);
            }
        }

        public override void End()
        {
            if (waveOut != null)
            {
                StopAudio();
            }

            Physics.gravity = new Vector3(0, -30, 0);
            base.End();
        }
    }
}
