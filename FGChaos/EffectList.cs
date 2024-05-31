using FGChaos.Effects;
using System.Collections.Generic;
using System.Linq;

namespace FGChaos
{
    public static class EffectList
    {
        public static Effect[] effects = new Effect[]
        {
            new FlingPlayer(),
            new TeleportToStartingPosition(),
            new Eliminate(),
            new WhoIsWaving(),
            new Spawn(),
            new Spawn(),
            new Spawn(),
            new WhereIsMyFallGuy(),
            new HandsInTheAir(),
            new RagdollPlayer(),
            new KidnapPlayer(),
            new JumpBoost(),
            new BoulderRain(),
            new PlanetAssault(),
            new WitnessProtection(),
            //new ClonePlayer(),  // Temporarily disabled because its bugged
            new FirstPersonMode(),
            new PiracyIsNoFalling(),
            new RocketShip(),
            new Jetpack(),
            new Gravity(),
            new Speed(),
            new BlueberryBombardment(),
            new SetTeam(),
            new LockCamera(),
            new TopDownView(),
            new SpeedBoost(),
            new BallBoost(),
            new Win(),
            new VignetteEffect(),
            new Creepypasta(),
            new RespawnAtLastCheckpoint(),
            new Gun(),
            new SwitchMoment(),
            new RemoveCoyoteTime(),
            new InvertedControls(),
            new SlideEverywhere(),
            new PaperGuys(),
            new RespawnAtRandomCheckpoint(),
            new NoLights(),
            new InfiniteJumps(),
            new SlipperyFloor(),
            new Lag(),
            new Camera2D(),
            new ReverseGun(),
            new SetFOV(),
            new RussianRoulette(),
            new WKeyStuck(),
            new ChangeMusic(),
            new WideGuys(),
            new HeavenTeleport(),
            new FakeCrash(),
            new RandomGameSpeed(),
            new PlayAsBert(),
            new RandomFPS(),
            new SuperJump(),
            new BouncyPlayer(),
            new SuperHot(),
            new RageQuit(),
            new InvertedCameraControls()
        };

        public static List<Effect> enabledEffects = effects.ToList();
    }
}
