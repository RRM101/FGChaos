using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos
{
    public class FGChaosDebug : MonoBehaviour
    {
        void OnGUI()
        {
            GUI.Label(new Rect(5, 5, Screen.width, Screen.height), $"<size=10>FGChaos v{Plugin.version}</size>");
        }
    }
}
