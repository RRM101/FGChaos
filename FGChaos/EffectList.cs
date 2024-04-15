﻿using FGChaos.Effects;
using System.Collections.Generic;

namespace FGChaos
{
    public static class EffectList
    {
        public static List<Effect> effects = new List<Effect>()
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
            new ClonePlayer(),
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
            new Creepypasta()
        };
    }
}
