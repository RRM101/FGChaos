using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using Il2CppInterop.Runtime.Injection;
using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using HarmonyLib;
using BepInEx.Configuration;
using Il2CppSystem;

namespace FGChaos
{
    [BepInPlugin("org.rrm1.fgchaos", "FGChaos", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public static ConfigEntry<bool> Disable { get; set; }

        public override void Load()
        {
            Disable = Config.Bind("Config", "Disable", false, "Disables the mod when enabled.");

            if (!Disable.Value)
            {
                Harmony.CreateAndPatchAll(typeof(Patches));

                ClassInjector.RegisterTypeInIl2Cpp<ChaosPluginBehaviour>();
                ClassInjector.RegisterTypeInIl2Cpp<Chaos>();
                GameObject obj = new GameObject("FGChaos Behaviour");
                GameObject.DontDestroyOnLoad(obj);
                obj.hideFlags = HideFlags.HideAndDontSave;
                obj.AddComponent<ChaosPluginBehaviour>();
                Log.LogInfo($"Plugin FGChaos has been loaded!");
            }
        }
    }
    public class ChaosPluginBehaviour : MonoBehaviour
    {
        public static ChaosPluginBehaviour instance;
        public static Chaos chaosInstance;
        bool devMode = false;
        
        public void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            instance = this;
        }

        public static void LoadBank(string bank)
        {
            if (!RuntimeManager.HasBankLoaded(bank))
            {
                RuntimeManager.LoadBank(bank);
                RuntimeManager.LoadBank($"{bank}.assets");
            }
        }

        public static void UnloadBank(string bank)
        {
            if (RuntimeManager.HasBankLoaded(bank))
            {
                RuntimeManager.UnloadBank(bank);
                RuntimeManager.UnloadBank($"{bank}.assets");
            }
        }

        public void EnableChaos()
        {
            GameObject obj = new GameObject("FGChaos");
            chaosInstance = obj.AddComponent<Chaos>();
        }

        public void RunCoroutine(IEnumerator enumerator)
        {
            StartCoroutine(enumerator.WrapToIl2Cpp());
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && devMode)
            {
                EnableChaos();
            }
        }
    }
    
}