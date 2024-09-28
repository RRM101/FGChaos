using DG.Tweening;
using FGClient;
using NAudio.Wave;
using SRF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace FGChaos.Effects
{
    public class Flashbang : Effect // unused
    {
        public Flashbang()
        {
            Name = "Flashbang";
            Duration = 10;
        }

        Image screenshot;
        Image whiteimage;
        ClientGameManager cgm;
        FGChaosVolumeWaveProvider volumeWaveProvider;

        public override void Run()
        {
            Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            screenshot = chaos.chaosCanvas.transform.CreateChild("Flashbang Screenshot").AddComponent<Image>();
            screenshot.rectTransform.anchoredPosition = Vector2.zero;
            screenshot.sprite = sprite;

            whiteimage = chaos.chaosCanvas.transform.CreateChild("Flashbang White").AddComponent<Image>();
            whiteimage.rectTransform.anchoredPosition = Vector2.zero;
            whiteimage.sprite = new Sprite();
            whiteimage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

            screenshot.SetNativeSize();
            StartCoroutine(ImageFade(screenshot, 9f));
            StartCoroutine(ImageFade(whiteimage, 3f));
            if (Plugin.CustomAudio.Value)
            {
                PlayAudio();
            }
            base.Run();
        }

        void PlayAudio()
        {
            Mp3FileReader mp3File = new Mp3FileReader($"{Plugin.GetModFolder()}/Assets/Audio/flashbang.mp3");
            volumeWaveProvider = new(mp3File);
            volumeWaveProvider.InitAndPlay();
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out cgm);
            cgm._musicInstance.setVolume(0);
        }

        IEnumerator ImageFade(Image image, float seconds)
        {
            Tween tween = image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0), seconds);
            yield return tween.WaitForCompletion();
            GameObject.Destroy(image.gameObject);
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
            base.End();
        }
    }
}