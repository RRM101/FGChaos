﻿using FG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FG.Common.Character.MotorSystem;
using FG.Common.Character;
using FGChaos.Effects;
using UnityEngine.UI;
using BepInEx;
using UnityEngine.Rendering.PostProcessing;
using Levels.Progression;
using System.Collections;
using BepInEx.Unity.IL2CPP.Utils.Collections;

namespace FGChaos
{
    public class Chaos : MonoBehaviour
    {
        List<Effect> effects = EffectList.enabledEffects;
        public static List<Effect> activeEffects = new List<Effect>();
        public Effect nextEffect;

        public FallGuysCharacterController fallGuy;
        public Rigidbody fgrb;
        public MultiplayerStartingPosition startingPosition;
        public CameraDirector cameraDirector;
        public MotorAgent motorAgent;
        public PostProcessVolume postProcessVolume;
        //public Sprite blueberrySprite;
        public float delay;
        public static bool jumpingEnabled = true;
        public static bool rocketShip;
        public static List<Action> OnJumpActions = new List<Action>();
        public static Dictionary<string, object> LoadedObjects => ChaosManager.chaosInstance.loadedObjects;
        public GameObject chaosCanvas;
        public Dictionary<string, object> loadedObjects = new();
        Slider chaosSlider;

        public static Dictionary<string, string> addressableAssetsKeyNamePairs = new Dictionary<string, string>()
        {
            {"Planet", "PB_Projectile_Futuristic_Planet"},
            {"Banana", "PB_Banana_FallBall" },
            {"Bert", "PB_Penguin_NoScore" },
            {"Sherbert", "PB_S05_Penguin" },
            {"Snowball", "PB_SnowBoulder" },
            {"Hoop", "PB_Hoop_Master" },
            {"Ball", "PB_FallBall_Ball" },
            {"Rhino", "PB_Bull" },
            {"Wifi", "167d0008aef582c4eb63bb6c88bbc610" },
            {"SS2 Turntable", "51b68558b403c074d8b6eb09e3cf1651" },
            {"Speed Arch", "11374594082ca994d8f12cfff47429da" },
            {"Blueberry", "PB_DodgeFall_Fruit_Berry_01"},
            {"Battery", "PB_Carry_TerritoryControlBattery"},
            {"Egg", "PB_CarrySmall_DY_EggGrab"},
            {"Golden Egg", "PB_CarrySmall_DY_EggGrab_Special"},
            {"Basketball", "PB_BasketFall"},
            {"Golden Basketball", "PB_BasketFall_Gold"},
            {"Broken Turntable", "6cdb55482c74e5847abe5610d6e3028f"}
        };
        public static string[] addressableAssetsNames;

        void Awake()
        {
            try
            {
                RectTransform chaosSliderRectTransform;
                AssetBundle chaosBundle = AssetBundle.LoadFromFile(Plugin.GetModFolder() + "/Assets/fgchaosbundle");

                fallGuy = FindObjectOfType<FallGuysCharacterController>();
                fgrb = fallGuy.GetComponent<Rigidbody>();
                startingPosition = FindObjectOfType<MultiplayerStartingPosition>();
                cameraDirector = FindObjectOfType<CameraDirector>();
                motorAgent = fallGuy.GetComponent<MotorAgent>();
                postProcessVolume = cameraDirector.MainNativeCam.gameObject.AddComponent<PostProcessVolume>();
                postProcessVolume.isGlobal = true;
                delay = Plugin.EffectTimer.Value;
                addressableAssetsNames = addressableAssetsKeyNamePairs.Keys.ToArray();
                rocketShip = false;
                chaosCanvas = Instantiate(chaosBundle.LoadAsset("ChaosCanvas").Cast<GameObject>());
                chaosSlider = chaosCanvas.transform.GetChild(0).GetComponent<Slider>();
                chaosSliderRectTransform = chaosSlider.GetComponent<RectTransform>();
                chaosSliderRectTransform.sizeDelta = new Vector2(Screen.width + 5, chaosSliderRectTransform.sizeDelta.y);
                chaosCanvas.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
                chaosBundle.Unload(false);
                FGChaosUtils.LoadBank("BNK_Music_GP");
                FGChaosUtils.LoadBank("BNK_PlayGo");
                ChooseRandomEffect();
                LoadAssets();

                if (Plugin.PlayEffectRunSFX.Value)
                {
                    FGChaosUtils.LoadBank("BNK_UI_MainMenu");
                }
            }
            catch (Exception e)
            {
                ChaosManager.ChaosStartError(e);
                throw;
            }
        }

        void LoadAssets()
        {
            if (loadedObjects.Count == 0)
            {
                loadedObjects.Add("Blueberry Sprite", FGChaosUtils.PNGtoSprite(Plugin.GetModFolder() + "/Assets/Images/blueberrybombardment.png"));
            }
        }

        public static object GetLoadedAsset(string assetName)
        {
            return LoadedObjects[assetName];
        }

        void ChooseRandomEffect()
        {
            if (effects.Count == 0)
            {
                Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                Application.Quit();
            }

            if (!CanContinue())
            {
                nextEffect = null;
                Plugin.Logs.LogInfo("No available effects");
                return;
            }

            int getRandomEffect = UnityEngine.Random.Range(0, effects.Count);
            Effect effectInstance = effects[getRandomEffect].Create();

            foreach (Effect activeEffect in activeEffects)
            {
                foreach (Type effectType in activeEffect.BlockedEffects)
                {
                    Effect effect = (Effect)Activator.CreateInstance(effectType);
                    if (effect.ID == effectInstance.ID)
                    {
                        Plugin.Logs.LogInfo($"Blocked {effectInstance.ID} because {activeEffect.ID} is active");
                        ChooseRandomEffect();
                        return;
                    }
                }
            }

            if (effectInstance.ID == "Eliminate" || effectInstance.ID == "Win")
            {
                int rng = UnityEngine.Random.RandomRange(0, 8);
                if (rng != 5)
                {
                    Plugin.Logs.LogInfo($"Blocked {effectInstance.ID} because you got lucky/unlucky");
                    ChooseRandomEffect();
                    return;
                }
            }

            if (effectInstance.ID == "RageQuit")
            {
                if (UnityEngine.Random.Range(0, 3) != 2)
                {
                    Plugin.Logs.LogInfo($"Blocked {effectInstance.ID} because you got weren't angry enough");
                    ChooseRandomEffect();
                    return;
                }
            }

            if (effectInstance.ID == "RespawnAtLastCheckpoint" || effectInstance.ID == "RespawnAtRandomCheckpoint")
            {
                CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();
                if (checkpointManager == null)
                {
                    Plugin.Logs.LogInfo($"Blocked {effectInstance.ID} because there isn't a Checkpoint Manager");
                    ChooseRandomEffect();
                    return;
                }

                if (!checkpointManager.NetIDToCheckpointMap.ContainsKey(fallGuy._pNetObject.NetID))
                {
                    Plugin.Logs.LogInfo($"Blocked {effectInstance.ID} because the Checkpoint Manager doesn't contain the player FallGuy");
                    ChooseRandomEffect();
                    return;
                }
            }

            if (effectInstance.ID == "Speed" && Plugin.DisableGameSpeedEffects.Value)
            {
                ChooseRandomEffect();
                return;
            }

            nextEffect = effectInstance;
            Plugin.Logs.LogInfo("Effect Chosen: " + effectInstance.ID);
        }

        bool CanContinue()
        {
            foreach (Effect effect in effects)
            {
                if (!isEffectBlocked(effect))
                    return true;
            }
            return false;
        }

        bool isEffectBlocked(Effect effect)
        {
            foreach (Effect activeEffect in activeEffects)
            {
                foreach (Type effectType in activeEffect.BlockedEffects)
                {
                    Effect effectInstance = (Effect)Activator.CreateInstance(effectType);
                    if (effectInstance.ID == effect.ID)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        void RunEffect()
        {
            if (nextEffect != null)
            {
                nextEffect.Create().Run();

                if (Plugin.PlayEffectRunSFX.Value)
                {
                    AudioManager.PlayOneShot("UI_MainMenu_Settings_Accept");
                }

                Plugin.Logs.LogInfo("Effect Ran: " + nextEffect.ID);
                return;
            }
            Plugin.Logs.LogInfo("No effect ran because nextEffect is null");
        }

        void Update()
        {
            if (delay > 0)
            {
                delay -= SuperHot.active ? Time.deltaTime : Time.unscaledDeltaTime; 
            }
            else
            {
                delay = Plugin.EffectTimer.Value;

                try
                {
                    if (nextEffect == null && CanContinue())
                    {
                        ChooseRandomEffect();
                    }
                    RunEffect();
                }
                catch (Exception e)
                {
                    Plugin.Logs.LogError($"An error occured: {e.GetType().Name}: {e.Message}\n\nStack Trace:\n{e.StackTrace}");
                }
                ChooseRandomEffect();
            }

            delay = Math.Min(delay, Plugin.EffectTimer.Value);
            chaosSlider.value = delay / Plugin.EffectTimer.Value;

            if (fallGuy == null)
            {
                Destroy(this);
            }
        }

        public void RunEffectWithDelay(Effect effect, float delay)
        {
            StartCoroutine(IRunEffectWithDelay(effect, delay).WrapToIl2Cpp());
        }

        IEnumerator IRunEffectWithDelay(Effect effect, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            effect.Run();
        }


        public static bool CanJump(MotorFunctionJump motorFunctionJump)
        {
            if (ChaosManager.chaosInstance != null)
            {
                if (InfiniteJumps.active)
                {
                    return true;
                }
                else
                {
                    return jumpingEnabled ? motorFunctionJump.CanJump() : false;
                }
            }
            else
            {
                return motorFunctionJump.CanJump();
            }
        }

        public static void StopAllEffects()
        {
            foreach (Effect effect in activeEffects.ToList())
            {
                try
                {
                    effect.End();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

        void OnDestroy()
        {
            Destroy(chaosCanvas);
            StopAllEffects();
        }
    }
}
