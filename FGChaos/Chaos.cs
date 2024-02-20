using FG.Common;
using FGClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FG.Common.Character.MotorSystem;
using FG.Common.Character;
using FGChaos.Effects;
using UnityEngine.UI;
using BepInEx;

namespace FGChaos
{
    public class Chaos : MonoBehaviour
    {
        List<Type> effects = new List<Type>();
        public static List<Effect> activeEffects = new List<Effect>();
        public FallGuysCharacterController fallGuy;
        public Rigidbody fgrb;
        public MultiplayerStartingPosition startingPosition;
        public CameraDirector cameraDirector;
        public MotorAgent motorAgent;
        public Sprite blueberrySprite;
        public static float delay;
        float roundedDelay;
        public string effect;
        public static bool jumpingEnabled = true;
        public static bool rocketShip;
        public GameObject chaosCanvas;
        Slider chaosSlider;

        public Dictionary<string, string> addressableAssetsKeyNamePairs = new Dictionary<string, string>()
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
        public string[] addressableAssetsNames;



        void OnGUI()
        {
            //GUI.Label(new Rect(Screen.width / 2, 5, 100, 300), $"<size=50>{roundedDelay}</size>");
            //GUI.Label(new Rect(Screen.width - 150, 5, 145, 200), $"<size=25>{effect}</size>");
        }

        void Awake()
        {
            RectTransform chaosSliderRectTransform;
            AssetBundle chaosBundle = AssetBundle.LoadFromFile(Paths.PluginPath + "/FGChaos/Assets/fgchaosbundle");

            fallGuy = FindObjectOfType<FallGuysCharacterController>();
            fgrb = fallGuy.GetComponent<Rigidbody>();
            startingPosition = FindObjectOfType<MultiplayerStartingPosition>();
            cameraDirector = FindObjectOfType<CameraDirector>();
            motorAgent = fallGuy.GetComponent<MotorAgent>();
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
            StopAllEffects();
        }

        void Start()
        {
            effects.Add(typeof(FlingPlayer));
            effects.Add(typeof(TeleportToStartingPosition));
            effects.Add(typeof(Eliminate));
            effects.Add(typeof(WhoIsWaving));
            effects.Add(typeof(Spawn));
            effects.Add(typeof(WhereIsMyFallGuy));
            effects.Add(typeof(HandsInTheAir));
            effects.Add(typeof(RagdollPlayer));
            effects.Add(typeof(KidnapPlayer));
            effects.Add(typeof(JumpBoost));
            effects.Add(typeof(BoulderRain));
            effects.Add(typeof(PlanetAssault));
            effects.Add(typeof(WitnessProtection));
            effects.Add(typeof(ClonePlayer));
            effects.Add(typeof(FirstPersonMode));
            effects.Add(typeof(PiracyIsNoFalling));
            effects.Add(typeof(RocketShip));
            effects.Add(typeof(Jetpack));
            effects.Add(typeof(LowGravity));
            effects.Add(typeof(NoGravity));
            effects.Add(typeof(Speed));
            effects.Add(typeof(BlueberryBombardment));
            effects.Add(typeof(SetTeam));
            effects.Add(typeof(LockCamera));

            //InvokeRepeating("RandomEffect", delay, delay);
        }

        void RandomEffect()
        {
            delay = Plugin.EffectTimer.Value;
            int getRandomEffect = UnityEngine.Random.Range(0, effects.Count);
            Effect effectInstance = (Effect)Activator.CreateInstance(effects[getRandomEffect]);

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

            if (effectInstance.ID == "Eliminate")
            {
                int rng = UnityEngine.Random.RandomRange(0, 11);
                if (rng != 5)
                {
                    Debug.Log("Blocked Elimination");
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
                delay = Plugin.EffectTimer.Value;
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
    }
}
