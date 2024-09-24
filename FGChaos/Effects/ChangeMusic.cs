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
            Duration = 30;
            BlockedEffects = new Type[] { typeof(ChangeMusic) };
        }

        string chosenMusic;

        Dictionary<string, string[]> music = new Dictionary<string, string[]>()
        {
            {"Orbital Tumbling", new string[]{"BNK_Music_Cheap_Ethics", "MUS_InGame_Cheap_Ethics"} },
            {"Satellite Scramble", new string[]{ "BNK_Music_Rung_Equip", "MUS_InGame_Rung_Equip" } },
            {"Temple Tumble", new string[]{ "BNK_Music_Temple_Tumble", "MUS_InGame_Temple_Tumble" } },
            {"Crown Stack Overflow", new string[]{ "BNK_Music_Swallow_Bottom", "MUS_InGame_Swallow_Bottom" } },
            {"Tentacle Tantrum", new string[]{ "BNK_Music_Critic_Carrot", "MUS_InGame_Critic_Carrot" } },
            {"Scubabeans", new string[]{ "BNK_Music_Bishop_Steak", "MUS_InGame_Bishop_Steak" } },
            {"Chimney Bean Bebop", new string[]{ "BNK_Music_Chimney_Bean_Bebop", "MUS_InGame_Chimney_Bean_Bebop" } },
            {"Fall for the Queen Bean", new string[]{ "BNK_Music_Fall_Queen_Bean", "MUS_InGame_Fall_Queen_Bean" } },
            {"Beans of the Round Table", new string[]{ "BNK_Music_Beans_Round_Table", "MUS_InGame_Beans_Round_Table" } },
            {"Long Wall", new string[]{ "BNK_Music_Long_Wall", "MUS_InGame_Long_Wall" } },
            {"Chill Your Beans", new string[]{ "BNK_Music_FallinFriends", "MUS_InGame_FallinFriends" } },
            {"Falling Stars", new string[]{ "BNK_Music_Falling_Stars", "MUS_InGame_Falling_Stars" } },
            {"Future Utobeania", new string[]{ "BNK_Music_Future_Utobeania", "MUS_InGame_Future_Utobeania" } }
        };

        EventInstance eventInstance;

        public override void Run()
        {
            GlobalGameStateClient.Instance.GameStateView.GetLiveClientGameManager(out ClientGameManager cgm);
            cgm._musicInstance.setPaused(true);

            FGChaosUtils.LoadBank(music[chosenMusic][0]);
            eventInstance = RuntimeManager.CreateInstance(AudioManager.GetGuidForKey(music[chosenMusic][1]));
            eventInstance.start();

            base.Run();
        }

        // TODO: Unload bank if the current round doesn't use it
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
