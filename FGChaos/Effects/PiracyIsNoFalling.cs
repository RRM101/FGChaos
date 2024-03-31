using FGClient.UI;

namespace FGChaos.Effects
{
    public class PiracyIsNoFalling : Effect
    {
        public PiracyIsNoFalling()
        {
            Name = "Pirated Game (Jumping Disabled)";
            ID = "PiracyIsNoFalling";
            Duration = 15;
        }

        public override void Run()
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
            Chaos.jumpingEnabled = false;
            base.Run();
        }

        public override void End()
        {
            Chaos.jumpingEnabled = true;
            base.End();
        }
    }
}
