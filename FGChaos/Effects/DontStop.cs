using FG.Common;
using FG.Common.Character;
using FG.Common.Character.MotorSystem;
using FGClient;
using Rewired;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class DontStop : Effect
    {
        public DontStop()
        {
            eliminate = UnityEngine.Random.Range(0, 3) == 1;

            Name = eliminate ? "Don't stop for more than 1 second <color=red>OR ELSE</color>" : "Don't stop for more than 1 second";
            Duration = 20;
            BlockedEffects = new Type[] { typeof(DontStop), typeof(Win), typeof(Eliminate) };
        }

        bool eliminate;
        Player rewiredPlayer;
        Action onElimBannerClosed;

        float timer;
        bool punish = true;

        public override void Run()
        {
            rewiredPlayer = chaos.fallGuy.GetComponent<FallGuysCharacterControllerInput>()._rewiredPlayer;
            onElimBannerClosed = ReplaceStateMainMenu;
            base.Run();
        }

        public override void Update()
        {
            bool isMoving = new Vector2(rewiredPlayer.GetAxis("Move Horizontal"), rewiredPlayer.GetAxis("Move Vertical")).magnitude > 0;

            if (!isMoving && punish)
            {
                timer += Time.unscaledDeltaTime;
            }
            else
            {
                timer = 0;
            }

            if (timer >= 1 && punish)
            {
                Punish(eliminate);
                timer = 0;
            }
        }

        void Punish(bool eliminate)
        {
            if (eliminate)
            {
                EliminatedScreenViewModel.Show("eliminated", null, onElimBannerClosed);
                AudioManager.PlayGameplayEndAudio(false);
                punish = false;
            }
            else
            {
                chaos.fallGuy.MotorAgent.GetMotorFunction<MotorFunctionTeleport>().RequestTeleport(chaos.startingPosition.transform.position, chaos.startingPosition.transform.rotation);

            }
        }

        void ReplaceStateMainMenu()
        {
            GlobalGameStateClient.Instance._gameStateMachine.ReplaceCurrentState(new StateMainMenu(GlobalGameStateClient.Instance._gameStateMachine, GlobalGameStateClient.Instance.CreateClientGameStateData(), false).Cast<GameStateMachine.IGameState>());
        }
    }
}
