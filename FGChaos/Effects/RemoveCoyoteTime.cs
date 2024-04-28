using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGChaos.Effects
{
    /*
        Effect Idea by dubtoshi
    */

    public class RemoveCoyoteTime : Effect
    {
        public RemoveCoyoteTime()
        {
            Name = "Remove Coyote Time";
            Duration = 15;
        }

        public override void Run()
        {
            CMSGlobalSettings.CharacterJumpCoyoteTime = 0;
            base.Run();
        }

        public override void End()
        {
            CMSGlobalSettings.CharacterJumpCoyoteTime = 0.15f;
            base.End();
        }
    }
}
