using FG.Common;
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

namespace FGChaos
{
    public class Chaos : MonoBehaviour
    {
        List<Effect> effects = EffectList.effects;
        public static List<Effect> activeEffects = new List<Effect>();
        public FallGuysCharacterController fallGuy;
        public Rigidbody fgrb;
        public MultiplayerStartingPosition startingPosition;
        public CameraDirector cameraDirector;
        public MotorAgent motorAgent;
        public PostProcessVolume postProcessVolume;
        public Sprite blueberrySprite;
        public static float delay;
        float roundedDelay;
        public string effect;
        public static bool jumpingEnabled = true;
        public static bool rocketShip;
        public GameObject chaosCanvas;
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
            {"Blueberry", "PB_DodgeFall_Fruit_Berry_01"}
        };
        public static string[] addressableAssetsNames;

        void Awake()
        {
            try
            {
                RectTransform chaosSliderRectTransform;
                AssetBundle chaosBundle = AssetBundle.LoadFromFile(Paths.PluginPath + "/FGChaos/Assets/fgchaosbundle");

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
                blueberrySprite = ChaosPluginBehaviour.PNGtoSprite(Paths.PluginPath + "/FGChaos/Assets/Images/blueberrybombardment.png");
                ChaosPluginBehaviour.LoadBank("BNK_Music_GP");
                ChaosPluginBehaviour.LoadBank("BNK_PlayGo");
                ChaosPluginBehaviour.LoadBank("BNK_UI_MainMenu");
            }
            catch (Exception e)
            {
                ChaosPluginBehaviour.ChaosStartError(e);
                throw;
            }
        }

        void RandomEffect()
        {
            delay = Plugin.EffectTimer.Value;
            int getRandomEffect = UnityEngine.Random.Range(0, effects.Count);
            Effect effectInstance = effects[getRandomEffect].Create();

            foreach (Effect activeEffect in activeEffects)
            {
                foreach (Type effectType in activeEffect.BlockedEffects)
                {
                    Effect effect = (Effect)Activator.CreateInstance(effectType);
                    if (effect.ID == effectInstance.ID)
                    {
                        Debug.Log($"Blocked {effectInstance.ID} because {activeEffect.ID} is active");
                        RandomEffect();
                        return;
                    }
                }
            }

            if (effectInstance.ID == "Eliminate" || effectInstance.ID == "Win")
            {
                int rng = UnityEngine.Random.RandomRange(0, 11);
                if (rng != 5)
                {
                    RandomEffect();
                    return;
                }
            }

            if (effectInstance.ID == "RespawnAtLastCheckpoint")
            {
                CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();
                if (checkpointManager == null)
                {
                    RandomEffect();
                    return;
                }
            }

            effectInstance.Run();

            if (effectInstance.Duration > 0)
            {
                effect = $"{effectInstance.Name} ({effectInstance.Duration}s)";
            }
            else
            {
                effect = effectInstance.Name;
            }

            if (Plugin.PlayEffectRunSFX.Value)
            {
                AudioManager.PlayOneShot("UI_MainMenu_Settings_Accept");
            }

            Debug.Log("Effect Ran: " + effectInstance.Name);
        }

        void Update()
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime / Time.timeScale;
            }
            else
            {
                RandomEffect();
            }

            chaosSlider.value = delay / Plugin.EffectTimer.Value;

            roundedDelay = (float)Math.Round(delay, 0);
        }

        public static bool CanJump(MotorFunctionJump motorFunctionJump)
        {
            if (ChaosPluginBehaviour.chaosInstance != null)
            {
                return jumpingEnabled ? motorFunctionJump.CanJump() : false;
            }
            else
            {
                return motorFunctionJump.CanJump();
            }
        }

        public static void StopAllEffects()
        {
            foreach (Effect effect in activeEffects)
            {
                try
                {
                    effect.End();
                }
                catch { }
            }
        }

        void OnDestroy()
        {
            Destroy(chaosCanvas);
            StopAllEffects();
        }
    }
}
