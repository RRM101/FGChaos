using FGClient;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class ChangeMusic : Effect
    {
        public ChangeMusic()
        {
            string[] musicNames = music.Keys.ToArray();
            chosenMusic = musicNames[UnityEngine.Random.Range(0, musicNames.Length)];
            Name = $"Change Music to {chosenMusic}";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(ChangeMusic) };
        }

        string chosenMusic;

        Dictionary<string, string[]> music = new Dictionary<string, string[]>()
        {
            {"Orbital Tumbling", new string[]{"BNK_Music_Cheap_Ethics", "MUS_InGame_Cheap_Ethics"} },
            {"Satellite Scramble", new string[]{ "BNK_Music_Rung_Equip", "MUS_InGame_Rung_Equip" } },
            {"Temple Tumble", new string[]{ "BNK_Music_Temple_Tumble", "MUS_InGame_Temple_Tumble" } },
            {"Crown Stack Overflow", new string[]{ "BNK_Music_Swallow_Bottom", "MUS_InGame_Swallow_Bottom" } }
        };

        EventInstance eventInstance;

        public override void Run()
        {
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out ClientGameManager cgm);
            cgm._musicInstance.setPaused(true);

            ChaosPluginBehaviour.LoadBank(music[chosenMusic][0]);
            eventInstance = RuntimeManager.CreateInstance(AudioManager.GetGuidForKey(music[chosenMusic][1]));
            eventInstance.start();

            base.Run();
        }

        public override void End()
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out ClientGameManager cgm);
            if (cgm != null)
            {
                cgm._musicInstance.setPaused(false);
            }

            base.End();
        }
    }
}
