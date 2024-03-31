namespace FGChaos.Effects
{
    public class TeleportToStartingPosition : Effect
    {
        public TeleportToStartingPosition()
        {
            Name = "Teleport to Start";
            ID = "TeleportToStartingPosition";
        }

        public override void Run()
        {
            chaos.fallGuy.transform.position = chaos.startingPosition.transform.position;
            chaos.fallGuy.transform.rotation = chaos.startingPosition.transform.rotation;
            base.Run();
        }
    }
}
