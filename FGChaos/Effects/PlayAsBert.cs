using FG.Common;
using FMODUnity;
using Levels.ChickenChase;
using SRF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FGChaos.Effects
{
    public class PlayAsBert : Effect
    {
        public PlayAsBert()
        {
            Name = "You are now Bert";
            Duration = 30;
            BlockedEffects = new Type[] { typeof(PlayAsBert), typeof(WhereIsMyFallGuy), typeof(FirstPersonMode) };
        }

        GameObject model;
        GameObject bert;

        public override void Run()
        {
            StartCoroutine(SpawnBert());
            base.Run();
        }

        IEnumerator SpawnBert()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("PB_Penguin_NoScore");
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
                GameObject.Destroy(obj.transform.GetChild(1).gameObject);
                obj.RemoveComponentIfExists<Rigidbody>();
                obj.RemoveComponentIfExists<ChickenController>();
                obj.RemoveComponentIfExists<ChickenAI>();
                obj.RemoveComponentIfExists<CarryObject>();
                obj.RemoveComponentIfExists<NetworkAwareGeneric>();
                obj.RemoveComponentIfExists<SoundBankLoader>();
                obj.RemoveComponentIfExists<CarryObjectAudio>();
                obj.RemoveComponentIfExists<ChickenVisualsController>();
                obj.transform.parent = chaos.fallGuy.transform;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.rotation = new Quaternion(0, 0, 0, 0);
                model = chaos.fallGuy.gameObject.transform.FindChild("Character").gameObject;
                model.SetActive(false);
                bert = obj;
            }
        }

        public override void End()
        {
            if (model != null)
            {
                model.SetActive(true);
            }

            if (bert != null)
            {
                GameObject.Destroy(bert);
            }

            base.End();
        }
    }
}
