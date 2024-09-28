using BepInEx.Configuration;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos
{
    public class FGChaosVolumeWaveProvider
    {
        public FGChaosVolumeWaveProvider(IWaveProvider waveProvider)
        {
            WaveProvider = waveProvider;
        }

        public IWaveProvider WaveProvider;
        public WaveOutEvent waveOut;
        public VolumeWaveProvider16 volumeWaveProvider;

        public void InitAndPlay()
        {
            waveOut = new WaveOutEvent();
            volumeWaveProvider = new VolumeWaveProvider16(WaveProvider);
            volumeWaveProvider.Volume = Math.Min((float)Plugin.CustomAudioVolume.Value / 100, 100);
            waveOut.Init(volumeWaveProvider);
            waveOut.Play();
            Plugin.CustomAudioVolume.SettingChanged += CustomAudioVolumeSettingChanged;
        }

        void CustomAudioVolumeSettingChanged(object sender, EventArgs eventArgs)
        {
            SettingChangedEventArgs settingChangedEventArgs = (SettingChangedEventArgs)eventArgs;
            float volume = Math.Min((float)Convert.ToInt32(settingChangedEventArgs.ChangedSetting.BoxedValue) / 100, 100);
            volumeWaveProvider.Volume = volume;
        }

        public void StopAudio()
        {
            waveOut.Stop();
            waveOut.Dispose();
            Plugin.CustomAudioVolume.SettingChanged -= CustomAudioVolumeSettingChanged;
        }
    }
}
