using FG.Common;
using FGClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using FG.Common.Character.MotorSystem;
using FG.Common.Character;
using SRF;
using FG.Common.LODs;
using FGClient.UI;
using Rewired;
using FGChaos.Effects;
using HarmonyLib;

namespace FGChaos
{
    public class Chaos : MonoBehaviour
    {
        List<Type> effects = new List<Type>();
        List<Action> actionList = new List<Action>();
        List<Action> FPBlockedActions = new List<Action>();
        public FallGuysCharacterController fallGuy;
        public Rigidbody fgrb;
        public MultiplayerStartingPosition startingPosition;
        public CameraDirector cameraDirector;
        public MotorAgent motorAgent;
        public static float delay;
        float roundedDelay;
        public string effect;
        bool isInFirstPerson = false;
        bool isInvisible = false;
        public static bool jumpingEnabled = true;
        public static bool rocketShip;

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
            {"Speed Arch", "11374594082ca994d8f12cfff47429da" }
        };
        public string[] addressableAssetsNames;



        void OnGUI()
        {
            GUI.Label(new Rect(Screen.width / 2, 5, 100, 300), $"<size=50>{roundedDelay}</size>");
            GUI.Label(new Rect(Screen.width - 150, 5, 145, 200), $"<size=25>{effect}</size>");
        }

        void Awake()
        {
            fallGuy = FindObjectOfType<FallGuysCharacterController>();
            fgrb = fallGuy.GetComponent<Rigidbody>();
            startingPosition = FindObjectOfType<MultiplayerStartingPosition>();
            cameraDirector = FindObjectOfType<CameraDirector>();
            motorAgent = fallGuy.GetComponent<MotorAgent>();
            delay = 5;
            addressableAssetsNames = addressableAssetsKeyNamePairs.Keys.ToArray();
            rocketShip = false;
            ChaosPluginBehaviour.LoadBank("BNK_Music_GP");
            ChaosPluginBehaviour.LoadBank("BNK_PlayGo");
        }

        void OldStart()
        {
            actionList.Add(FlingPlayer);
            actionList.Add(TeleportToStartingPosition);
            actionList.Add(Eliminate);
            actionList.Add(WhoIsWaving);
            actionList.Add(Spawn);
            actionList.Add(WhereIsMyFallGuy);            
            actionList.Add(Surrender);
            actionList.Add(RagdollPlayer);
            actionList.Add(KidnapPlayer);
            actionList.Add(JumpBoost);
            actionList.Add(BoulderRain);
            actionList.Add(PlanetAssault);
            actionList.Add(WitnessProtection);
            actionList.Add(ClonePlayer);
            actionList.Add(FirstPersonMode);
            actionList.Add(PiracyIsNoFalling);
            actionList.Add(RocketShip);
            actionList.Add(Jetpack);
            actionList.Add(LowGravity);
            //actionList.Add(UTurn);

            FPBlockedActions.Add(FirstPersonMode);
            FPBlockedActions.Add(WitnessProtection);
            FPBlockedActions.Add(ClonePlayer);
            FPBlockedActions.Add(WhereIsMyFallGuy);

            InvokeRepeating("RandomEffect", delay, delay);
        }

        void OldRandomEffect()
        {
            delay = 5;
            int getRandomEffect = UnityEngine.Random.Range(0, actionList.Count);
            Action action = actionList[getRandomEffect];
            if (action == Eliminate)
            {
                int rng = UnityEngine.Random.RandomRange(0, 11);
                //Debug.Log($"RNG {rng}");
                if (rng == 5)
                {
                    Debug.Log("Blocked Elimination");
                    actionList[getRandomEffect]();
                }
                else
                {
                    RandomEffect();
                }
            }
            else if (FPBlockedActions.Contains(action) && isInFirstPerson)
            {
                Debug.Log("BlockedFP " + action.Method.Name);
                RandomEffect();
            }
            else if(action == FirstPersonMode && isInvisible)
            {
                RandomEffect();
            }
            else if ((action == RocketShip && !jumpingEnabled) || (action == PiracyIsNoFalling && rocketShip))
            {
                RandomEffect();
            }
            else
            {
                action();
            }
            Debug.Log(getRandomEffect);
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

            InvokeRepeating("RandomEffect", delay, delay);
        }

        void RandomEffect()
        {
            delay = 5;
            int getRandomEffect = UnityEngine.Random.Range(0, effects.Count);
            Effect effectInstance = (Effect)Activator.CreateInstance(effects[getRandomEffect]);
            foreach (Type effectType in effectInstance.BlockedEffects)
            {
                Effect effect = (Effect)Activator.CreateInstance(effectType);
                if (effect.isActive)
                {
                    RandomEffect();
                }
            }
            effect = effectInstance.Name;
            effectInstance.Run();
        }

        IEnumerator InstantiateAddressableObject(string key)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                if (fallGuy != null)
                {
                    obj.transform.position = fallGuy.transform.position;
                }
            }
            else
            {
                Debug.Log($"object '{key}' not found");
            }
        }

        IEnumerator KidnapPlayer(string key)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                
                int random_x = UnityEngine.Random.Range(25, 40);
                int random_z = UnityEngine.Random.Range(25, 41);
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                obj.transform.rotation = fallGuy.transform.rotation;
                yield return new WaitForSeconds(0.1f);
                obj.transform.parent = fallGuy.transform;
                obj.transform.localPosition = new Vector3(0, 1, 0);
                Vector3 velocity = new Vector3(random_x, Math.Max(Math.Abs(random_x), Math.Abs(random_z)), random_z);
                Vector3 velocity2 = velocity.magnitude * fallGuy.transform.forward.normalized;
                rigidbody.velocity = new Vector3(velocity2.x, Math.Max(Math.Abs(random_x), Math.Abs(random_z)), velocity2.z);
                rigidbody.angularVelocity = rigidbody.velocity;
                
            }
            else
            {
                Debug.Log($"object '{key}' not found");
            }
        }

        void Update()
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }

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

        void FlingPlayer()
        {
            effect = "Fling Player";
            int random_x = UnityEngine.Random.Range(-200, 200);
            int random_z = UnityEngine.Random.Range(-200, 201);
            fgrb.velocity = new Vector3(random_x,Math.Max(Math.Abs(random_x), Math.Abs(random_z)), random_z) * 10;
        }

        void TeleportToStartingPosition()
        {
            effect = "Teleport to Start";
            fallGuy.transform.position = startingPosition.transform.position;
            fallGuy.transform.rotation = startingPosition.transform.rotation;
        }

        void Eliminate()
        {
            effect = "Eliminate Player";
            StartCoroutine(EliminateCoroutine().WrapToIl2Cpp());
        }

        IEnumerator EliminateCoroutine()
        {
            EliminatedScreenViewModel.Show("eliminated", null, null);
            AudioManager.PlayGameplayEndAudio(false);
            yield return new WaitForSeconds(5);
            int randomnumber = UnityEngine.Random.Range(0, 3);
            if (randomnumber == 0)
            {
                yield return new WaitForSeconds(1);
                effect = "Fake Eliminate Player"; // improve later
            }
            else
            {
                Addressables.LoadSceneAsync("MainMenu");
            }
        }

        void WhoIsWaving()
        {
            effect = "Who is waving?";
            ChaosPluginBehaviour.LoadBank("BNK_Emote_Wave_A");
            StartCoroutine(WhoIsWavingCoroutine().WrapToIl2Cpp());
        }

        IEnumerator WhoIsWavingCoroutine()
        {
            int woos = UnityEngine.Random.RandomRange(5, 15);
            for (int i = 0; i < woos; i++)
            {
                yield return new WaitForSeconds(2);
                AudioManager.PlayOneShotAttached("SFX_Emote_Wave_A", fallGuy.gameObject);
            }
        }

        void ClonePlayer()
        {
            effect = "Clone Player";
            GameObject clonedfg = Instantiate(fallGuy.gameObject);
            CameraDirector cameraDirectorChild = clonedfg.GetComponentInChildren<CameraDirector>();
            if (cameraDirectorChild != null)
            {
                Destroy(cameraDirectorChild.gameObject);
            }
            FallGuysCharacterController fallGuysCharacter = clonedfg.GetComponent<FallGuysCharacterController>();
            fallGuysCharacter.IsLocalPlayer = true;
            fallGuysCharacter.IsControlledLocally = true;
            GameObject clientPlayerUpdateManagerObject = new GameObject("ClonePlayer");
            ClientPlayerUpdateManager clientPlayerUpdateManager = clientPlayerUpdateManagerObject.AddComponent<ClientPlayerUpdateManager>();
            clientPlayerUpdateManager.RegisterPlayer(clonedfg.GetComponent<FallGuysCharacterController>(), true);
            clientPlayerUpdateManager.GameIsStarting();
            CustomisationManager.Instance.ApplyCustomisationsToFallGuy(clonedfg, GlobalGameStateClient.Instance.PlayerProfile.CustomisationSelections, -1);
        }

        void Spawn()
        {
            int randomnumber = UnityEngine.Random.Range(0, addressableAssetsNames.Length);
            effect = $"Spawn {addressableAssetsNames[randomnumber]}";
            StartCoroutine(InstantiateAddressableObject(addressableAssetsKeyNamePairs[addressableAssetsNames[randomnumber]]).WrapToIl2Cpp());            
        }

        void WhereIsMyFallGuy()
        {
            effect = "Where is my Fall Guy?";
            StartCoroutine(WhereIsMyFallGuyCoroutine().WrapToIl2Cpp());
        }

        IEnumerator WhereIsMyFallGuyCoroutine()
        {
            GameObject model = fallGuy.gameObject.transform.FindChild("Character").gameObject;
            model.SetActive(false);
            isInvisible = true;
            yield return new WaitForSeconds(15);
            isInvisible = false;
            model.SetActive(true);
        }

        public void UTurn()
        {            
            fallGuy.DesiredRotation = new Quaternion(fallGuy.DesiredRotation.x, -fallGuy.DesiredRotation.y, fallGuy.DesiredRotation.z, fallGuy.DesiredRotation.w);
            cameraDirector.StartRecenterToHeading();
        }

        IEnumerator ISurrenderCoroutine()
        {
            MotorFunctionRagdollStateRollOver stateRollOver = motorAgent.GetMotorFunction<MotorFunctionRagdoll>().GetState<MotorFunctionRagdollStateRollOver>();
            stateRollOver.Begin(0);
            yield return new WaitForSeconds(5);
            stateRollOver.End(0);
        }

        void Surrender()
        {
            effect = "I Surrender";
            StartCoroutine(ISurrenderCoroutine().WrapToIl2Cpp());
        }

        IEnumerator RagdollPlayerCoroutine()
        {
            MotorFunctionRagdollStateStunned stateStunned = motorAgent.GetMotorFunction<MotorFunctionRagdoll>().GetState<MotorFunctionRagdollStateStunned>();
            stateStunned.Begin(0);
            yield return new WaitForSeconds(5);
            stateStunned.End(0);
        }

        void RagdollPlayer()
        {
            effect = "Ragdoll Player";
            StartCoroutine(RagdollPlayerCoroutine().WrapToIl2Cpp());
        }

        void KidnapPlayer()
        {
            effect = "Kidnap Player";
            StartCoroutine(KidnapPlayer("PB_Projectile_Futuristic_Hexnut_BigShots").WrapToIl2Cpp());
        }

        IEnumerator JumpBoostCoroutine()
        {
            fallGuy._inheritedJumpVelocity = new Vector3(0,25,0);
            yield return new WaitForSeconds(10);
            fallGuy._inheritedJumpVelocity = new Vector3(0,0,0);
        }

        void JumpBoost()
        {
            effect = "Jump Boost";
            StartCoroutine(JumpBoostCoroutine().WrapToIl2Cpp());
        }

        IEnumerator BoulderRainSpawn()
        {
            Vector3 randompoint = fallGuy.transform.position + (Vector3)(20 * UnityEngine.Random.insideUnitCircle);
            int randomy = UnityEngine.Random.Range(50, 100);
            Vector3 randomPosition = new Vector3(randompoint.x, fallGuy.transform.position.y + randomy, randompoint.z);
            string[] boulderKeys = { "PB_Boulder_Large", "PB_Boulder_Large_01", "PB_Boulder_Large_02", "PB_Boulder_Large_03", "PB_Boulder_Large_04" };
            int randomboulder = UnityEngine.Random.Range(0, boulderKeys.Length);
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(boulderKeys[randomboulder]);
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                obj.transform.position = randomPosition;
                yield return new WaitForSeconds(5);
            }
        }

        IEnumerator BoulderRainCoroutine()
        {
            int randomSpawnAmount = UnityEngine.Random.Range(5, 20);
            for (int i = 0; i < randomSpawnAmount; i++)
            {
                yield return BoulderRainSpawn();
            }
        }

        void BoulderRain()
        {
            effect = "Boulder Rain";
            StartCoroutine(BoulderRainCoroutine().WrapToIl2Cpp());
        }

        IEnumerator PlanetAssaultCoroutine()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("PB_Projectile_Futuristic_Planet");
            yield return handle;
            if (handle.Result != null)
            {
                GameObject obj = Instantiate(handle.Result);
                obj.RemoveComponentIfExists<LodController>();
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                obj.transform.parent = fallGuy.transform;
                int random_x = UnityEngine.Random.Range(50, 100);
                int random_z = UnityEngine.Random.Range(50, 101);
                int front = UnityEngine.Random.Range(0, 2);
                int multiplier = -1;
                multiplier = front == 1 ? -1 : 1;
                obj.transform.localPosition = new Vector3(0, 1, -5 * multiplier);
                Vector3 velocity = new Vector3(random_x, Math.Max(Math.Abs(random_x), Math.Abs(random_z)), random_z);
                Vector3 velocity2 = velocity.magnitude * fallGuy.transform.forward * multiplier;
                rigidbody.velocity = new Vector3(velocity2.x, 5, velocity2.z);
                rigidbody.angularVelocity = rigidbody.velocity;
            }
        }

        void PlanetAssault()
        {
            effect = "Planet Assault";
            StartCoroutine(PlanetAssaultCoroutine().WrapToIl2Cpp());
        }

        IEnumerator WitnessProtectionCoroutine()
        {
            Transform witnessProtection = fallGuy.transform.CreateChild("WitnessProtection").transform;

            float rotation = 0;
            witnessProtection.transform.localPosition = Vector3.zero;
            witnessProtection.transform.eulerAngles = new Vector3(0, rotation, 0);
            Transform characterTransform = fallGuy.transform.GetChild(0);

            for (int i = 0; i < 18; i++)
            {
                Transform witness = witnessProtection.CreateChild("Witness").transform;
                witness.transform.localPosition = Vector3.zero;
                witness.transform.eulerAngles = new Vector3(0, i * 20, 0);
                GameObject witnessGameObject = Instantiate(characterTransform.gameObject, witness);
                witnessGameObject.SetActive(true);
                witnessGameObject.transform.localPosition = new Vector3(0, 0, 5);
                witnessGameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            float wait = 0;
            while (wait < 30)
            {
                rotation += 200 * Time.deltaTime;
                witnessProtection.transform.eulerAngles = new Vector3(0, rotation, 0);
                wait += Time.deltaTime;
                yield return null;
            }

            Destroy(witnessProtection.gameObject);
        }

        void WitnessProtection()
        {
            effect = "Witness Protection";
            StartCoroutine(WitnessProtectionCoroutine().WrapToIl2Cpp());
        }

        IEnumerator FirstPersonModeCoroutine()
        {
            Transform cameraDirectorTransform = cameraDirector.transform;
            cameraDirectorTransform.GetChild(1).gameObject.SetActive(false);
            cameraDirectorTransform.GetChild(0).position = Vector3.zero;
            cameraDirectorTransform.GetChild(0).localPosition = Vector3.zero;
            cameraDirectorTransform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
            yield return null;
            cameraDirectorTransform.SetParent(fallGuy.transform.FindChild("Character/SKELETON/Root/Torso_C_jnt_NoStrechSquash/Chest_C_jnt/Head_C_jnt01/"));
            cameraDirectorTransform.localPosition = new Vector3(0, 2, 0);
            cameraDirectorTransform.rotation = new Quaternion(0, 0, 0, 0);
            yield return new WaitForSeconds(30);
            cameraDirectorTransform.SetParent(null);
            cameraDirectorTransform.position = Vector3.zero;
            cameraDirectorTransform.rotation = new Quaternion(0, 0, 0, 0);
            cameraDirectorTransform.GetChild(1).gameObject.SetActive(true);
            isInFirstPerson = false;
        }

        void FirstPersonMode()
        {
            effect = "First Person Mode";
            isInFirstPerson = true;
            StartCoroutine(FirstPersonModeCoroutine().WrapToIl2Cpp());
        }

        void PiracyIsNoFalling()
        {
            effect = "Pirated Game";
            StartCoroutine(PiracyIsNoFallingCoroutine().WrapToIl2Cpp());
        }

        IEnumerator PiracyIsNoFallingCoroutine()
        {
            ModalMessageData modalMessageData = new ModalMessageData
            {
                Title = "PIRACY IS NO FALLING",
                Message = "It is a serious crime under copyright law to pirate Fall Guys: Ultimate Knockout.\nJumping has been disabled. Exit the game now and delete the software.",
                LocaliseTitle = UIModalMessage.LocaliseOption.NotLocalised,
                LocaliseMessage = UIModalMessage.LocaliseOption.NotLocalised,
                ModalType = UIModalMessage.ModalType.MT_OK
            };

            PopupManager.Instance.Show(PopupInteractionType.Warning, modalMessageData);
            jumpingEnabled = false;
            yield return new WaitForSeconds(15);
            jumpingEnabled = true;
        }

        void RocketShip()
        {
            effect = "Rocket Ship";
            StartCoroutine(RocketShipCoroutine().WrapToIl2Cpp());
        }

        IEnumerator RocketShipCoroutine()
        {
            rocketShip = true;
            yield return new WaitForSeconds(15);
            rocketShip = false;
        }

        IEnumerator JetpackCoroutine()
        {
            Player rewiredplayer = fallGuy.GetComponent<FallGuysCharacterControllerInput>()._rewiredPlayer;
            float wait = 0;
            while (wait < 20) { rocketShip = rewiredplayer.GetButton(2); wait += Time.deltaTime; yield return null; }
            rocketShip = false;
        }

        void Jetpack()
        {
            effect = "Jetpack";
            StartCoroutine(JetpackCoroutine().WrapToIl2Cpp());
        }

        IEnumerator LowGravityCoroutine()
        {
            Physics.gravity = new Vector3(0, -5, 0);
            yield return new WaitForSeconds(30);
            Physics.gravity = new Vector3(0, -30, 0);
        }

        void LowGravity()
        {
            effect = "Low Gravity";
            StartCoroutine(LowGravityCoroutine().WrapToIl2Cpp());
        }
    }
}
