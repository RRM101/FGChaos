using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    public class TouchTrigger : Effect
    {
        public TouchTrigger()
        {
            Name = "Touch Trigger";
            Duration = 30;
        }

        MonoBehaviours.TouchTrigger touchTriggerScript;

        public override void Run()
        {
            touchTriggerScript = chaos.fallGuy.gameObject.AddComponent<MonoBehaviours.TouchTrigger>();
            base.Run();
        }

        public override void End()
        {
            if (touchTriggerScript != null)
            {
                UnityEngine.Object.Destroy(touchTriggerScript);
            }
            base.End();
        }
    }
}
