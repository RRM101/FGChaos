using FGClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class Lag : Effect
    {
        public Lag()
        {
            Name = "Lag";
            Duration = 20;
        }

        ConnectionQualityViewModel connectionQualityViewModel;

        public override void Run()
        {
            connectionQualityViewModel = GameObject.FindObjectOfType<ConnectionQualityViewModel>();
            connectionQualityViewModel._trackState = true;
            connectionQualityViewModel._currentNetworkState = FG.Common.FG_NetworkManager.NetConnQuality.Extreme;
            base.Run();
            StartCoroutine(ILag());
        }

        IEnumerator ILag()
        {
            Vector3 previousPosition;
            while (isActive)
            {
                yield return new WaitForSeconds(0.5f);
                previousPosition = chaos.fallGuy.transform.position;
                yield return new WaitForSeconds(0.5f);
                chaos.fallGuy.transform.position = previousPosition;
            }
        }

        public override void End()
        {
            connectionQualityViewModel._trackState = false;
            connectionQualityViewModel._currentNetworkState = FG.Common.FG_NetworkManager.NetConnQuality.Disconnected;
            base.End();
        }
    }
}
