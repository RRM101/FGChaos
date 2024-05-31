using FGClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class RageQuit : Effect
    {
        public RageQuit()
        {
            Name = "Rage Quit";
        }

        public override void Run()
        {
            StartCoroutine(Quit());
            base.Run();
        }

        IEnumerator Quit()
        {
            InGameOptionsMenuManager.Instance.ShowScreen();
            yield return WaitForSeconds(0.5f);
            AudioManager.PlayOneShot(AudioManager.EventMasterData.GenericPopUpAppears);
            LeaveMatchPopupManager.Instance.ShowScreen();
            yield return WaitForSeconds(0.5f);
            LeaveMatchPopupManager.Instance.OnClose(true);
            AudioManager.Instance.PlayOneShot(AudioManager.EventMasterData.GenericAccept, null);
        }
    }
}
