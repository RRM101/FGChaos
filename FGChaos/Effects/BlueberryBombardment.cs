using FG.Common.LODs;
using Mediatonic.Tools.Utils;
using SRF;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace FGChaos.Effects
{
    public class BlueberryBombardment : Effect
    {
        public BlueberryBombardment()
        {
            Name = "Blueberry Bombardment";
            Duration = 15;
        }

        Transform spawnPosition;
        float delay = 0.2f;
        Image blueberryImage;

        public override void Run()
        {
            spawnPosition = chaos.fallGuy.transform.CreateChild("Blueberry Spawn").transform;
            spawnPosition.localPosition = new Vector3(0, 10, 1);
            blueberryImage = chaos.chaosCanvas.transform.CreateChild("Blueberry Image").AddComponent<Image>();
            blueberryImage.rectTransform.anchoredPosition = Vector2.zero;
            blueberryImage.sprite = chaos.blueberrySprite;
            blueberryImage.SetNativeSize();
            StartCoroutine(ImageFade());

            base.Run();
        }

        public override void Update()
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime / Time.timeScale;
            }
            else
            {
                delay = 0.2f;
                StartCoroutine(SpawnBerry());
            }
        }

        IEnumerator ImageFade()
        {   
            while (blueberryImage.color.a > 0)
            {
                blueberryImage.SetAlpha(blueberryImage.color.a - Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            GameObject.Destroy(blueberryImage.gameObject);
        }

        IEnumerator SpawnBerry()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("PB_DodgeFall_Fruit_Berry_01");
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                obj.transform.position = spawnPosition.position;
                obj.RemoveComponentIfExists<LodController>();
                obj.GetComponent<Rigidbody>().velocity = new Vector3(0, -60, 0);
            }
        }

        public override void End()
        {
            if (spawnPosition != null)
            {
                GameObject.Destroy(spawnPosition.gameObject);
            }

            if (blueberryImage != null)
            {
                GameObject.Destroy(blueberryImage.gameObject);
            }
            base.End();
        }
    }
}
