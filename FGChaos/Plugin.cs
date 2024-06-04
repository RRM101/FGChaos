using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using Il2CppInterop.Runtime.Injection;
using System.Collections;
using UnityEngine;
using FMODUnity;
using HarmonyLib;
using BepInEx.Configuration;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using FGClient.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UniverseLib.UI;
using FGChaos.Effects;
using System.Linq;

namespace FGChaos
{
    [BepInPlugin("org.rrm1.fgchaos", "FGChaos", version)]
    public class Plugin : BasePlugin
    {
        public const string version = "1.0.1";

        public static ConfigEntry<bool> Disable { get; set; }
        public static ConfigEntry<int> EffectTimer { get; set; }
        public static ConfigEntry<bool> PlayEffectRunSFX { get; set; }
        public static ConfigEntry<bool> DisableGameSpeedEffects { get; set; }
        public static ConfigEntry<bool> ShowWatermark { get; set; } 
        public static ConfigEntry<bool> EasyFirstPersonMode { get; set; }

        public override void Load()
        {
            Disable = Config.Bind("Config", "Disabled", false, "Disables the mod. (Requires Restart)");
            EffectTimer = Config.Bind("Config", "Effect Timer", 10, "The amount of time in seconds for the next effect to run.");
            PlayEffectRunSFX = Config.Bind("Config", "Play Effect Run Sound Effect", false, "Plays a sound effect when an Effect is ran.");
            DisableGameSpeedEffects = Config.Bind("Config", "Disable Game Speed Effects", false, "Disables the Game Speed Effects. (Deprecated)");
            ShowWatermark = Config.Bind("Config", "Show Watermark", true, "Shows a Watermark at the Bottom-Left side of the Screen.");
            EasyFirstPersonMode = Config.Bind("Config", "Easy First Person Mode", false, "Makes First Person Mode easier.");

            if (!Disable.Value)
            {
                Harmony.CreateAndPatchAll(typeof(Patches), "FGChaosPatches");

                ClassInjector.RegisterTypeInIl2Cpp<MonoBehaviours.BouncyPlayer>();

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
        public static TextMeshProUGUI effectName;
        string[] modFiles = new string[]
        {
            "/FGChaos/FGChaos.dll",
            "/FGChaos/Assets/fgchaosbundle",
            "/FGChaos/Assets/Images/blueberrybombardment.png"
        };
        bool hasMissingFiles;
        List<string> missingFilePaths = new List<string>();
        UIBase UI;
        
        public void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            instance = this;

            UniverseLib.Universe.Init(1, null, null, new()
            {
                Disable_EventSystem_Override = false,
                Force_Unlock_Mouse = true,
                Unhollowed_Modules_Folder = Paths.BepInExRootPath + "/interop"
            });

            DisableEffects();
            CheckForMissingFiles();
        }

        void OnEnable()
        {
            SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>)OnSceneLoaded);
        }

        void OnGUI()
        {
            if (Plugin.ShowWatermark.Value)
            {
                GUI.Label(new Rect(5, Screen.height - 25, 300, 20), $"FGChaos {Plugin.version}");
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Chaos.StopAllEffects();

            if (scene.name == "MainMenu")
            {
                if (UI == null)
                {
                    UI = UniversalUI.RegisterUI("org.rrm1.fgchaos", null);
                    new UI.EffectOptionsUI(UI);
                    UI.Enabled = false;
                }

                if (effectName == null)
                {
                    GameObject effectNameGameObject = new GameObject("Effect Name");
                    GameObject.DontDestroyOnLoad(effectNameGameObject);
                    effectNameGameObject.hideFlags = HideFlags.HideAndDontSave;
                    effectNameGameObject.AddComponent<LayoutElement>();
                    effectName = effectNameGameObject.AddComponent<TextMeshProUGUI>();
                    foreach(TMP_FontAsset fontAsset in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
                    {
                        if (fontAsset.name.Contains("Titan"))
                        {
                            effectName.font = fontAsset;
                            break;
                        }
                    }
                    effectName.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    effectName.rectTransform.sizeDelta = new Vector2(Screen.width, 50);
                }

                if (Application.version != "10.8.1")
                {
                    ShowWrongGameVersionPopup();
                }

                if (Directory.Exists($"{Paths.PluginPath}/FGTools/"))
                {
                    ShowFGToolsPopup();
                }

                if (hasMissingFiles)
                {
                    ShowMissingFilesPopup();
                }
            }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.L) && chaosInstance != null)
            {
                Destroy(chaosInstance.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                UI.Enabled = !UI.Enabled;
                CursorManager.Instance.OnApplicationFocus(true);
            }
        }

        void CheckForMissingFiles()
        {
            foreach (string path in modFiles)
            {
                if (!File.Exists(Paths.PluginPath + path))
                {
                    hasMissingFiles = true;
                    missingFilePaths.Add(Paths.PluginPath + path);
                }
            }
        }

        void ShowMissingFilesPopup()
        {
            string missingFiles = string.Join("\n", missingFilePaths);
            ModalMessageData modalMessageData = new ModalMessageData
            {
                Title = "FGChaos - Missing Files!",
                Message = $"The Missing Files are:\n{missingFiles}\nMake you sure you placed the mod in the correct folder.",
                LocaliseTitle = UIModalMessage.LocaliseOption.NotLocalised,
                LocaliseMessage = UIModalMessage.LocaliseOption.NotLocalised,
                ModalType = UIModalMessage.ModalType.MT_OK
            };

            PopupManager.Instance.Show(PopupInteractionType.Error, modalMessageData);
            Harmony.UnpatchID("FGChaosPatches");
        }

        void ShowWrongGameVersionPopup()
        {
            Action<bool> stupid = StupidMTBoolQuitGame;

            ModalMessageData modalMessageData = new ModalMessageData
            {
                Title = "FGChaos - ERROR!",
                Message = $"You are on Fall Guys {Application.version}, the mod is only for Fall Guys 10.8.1",
                LocaliseTitle = UIModalMessage.LocaliseOption.NotLocalised,
                LocaliseMessage = UIModalMessage.LocaliseOption.NotLocalised,
                ModalType = UIModalMessage.ModalType.MT_BLOCKING,
                OnCloseButtonPressed = stupid
            };

            PopupManager.Instance.Show(PopupInteractionType.Error, modalMessageData);
        }

        void StupidMTBoolQuitGame(bool stupid)
        {
            Application.Quit();
        }

        void ShowFGToolsPopup()
        {
            ModalMessageData modalMessageData = new ModalMessageData
            {
                Title = "FGChaos - WARNING!",
                Message = "You are using FGTools which is not currently supported by FGChaos, the mod will still work, but there might be unexpected bugs.",
                LocaliseTitle = UIModalMessage.LocaliseOption.NotLocalised,
                LocaliseMessage = UIModalMessage.LocaliseOption.NotLocalised,
                ModalType = UIModalMessage.ModalType.MT_OK
            };

            PopupManager.Instance.Show(PopupInteractionType.Warning, modalMessageData);
        }

        public static void DisableEffects()
        {
            if (File.Exists($"{Paths.PluginPath}/FGChaos/disabledeffects.txt"))
            {
                string[] disabledEffects = File.ReadAllLines($"{Paths.PluginPath}/FGChaos/disabledeffects.txt");

                EffectList.enabledEffects = EffectList.effects.ToList();

                foreach (Effect effect in EffectList.effects)
                {
                    foreach (string disabledEffect in disabledEffects)
                    {
                        if (effect.ID == disabledEffect)
                        {
                            EffectList.enabledEffects.Remove(effect);
                        }
                    }
                }
            }
        }

        void SpawnAddressableAsset(string key)
        {
            StartCoroutine(ISpawnAddressableAsset(key).WrapToIl2Cpp());
        }

        IEnumerator ISpawnAddressableAsset(string key)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = GameObject.Instantiate(handle.Result);
            }
            else
            {
                Debug.Log($"object '{key}' not found");
            }
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

        public static Sprite PNGtoSprite(string path)
        {
            byte[] imagedata = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            ImageConversion.LoadImage(texture, imagedata);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            return sprite;
        }

        public static void ChaosStartError(Exception e)
        {
            ModalMessageData modalMessageData = new ModalMessageData()
            {
                Title = "FGChaos - ERROR!",
                Message = $"An error occured while starting chaos.\nThe Error:\n{e.Message}",
                LocaliseTitle = UIModalMessage.LocaliseOption.NotLocalised,
                LocaliseMessage = UIModalMessage.LocaliseOption.NotLocalised,
                ModalType = UIModalMessage.ModalType.MT_OK
            };
            PopupManager.Instance.Show(PopupInteractionType.Error, modalMessageData);
        }

        public void EnableChaos()
        {
            try
            {
                if (chaosInstance == null)
                {
                    GameObject obj = new GameObject("FGChaos");
                    chaosInstance = obj.AddComponent<Chaos>();
                }
            }
            catch (Exception e)
            {
                ChaosStartError(e);
                throw;
            }
        }

        public void RunCoroutine(IEnumerator enumerator)
        {
            StartCoroutine(enumerator.WrapToIl2Cpp());
        }
    }    
}