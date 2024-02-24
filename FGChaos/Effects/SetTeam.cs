using FG.Common;
using FGClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos.Effects
{
    public class SetTeam : Effect
    {
        string name;

        public override string Name
        {
            get { return name; }
        }

        public override string ID
        {
            get { return "SetTeam"; }
        }

        TeamColourOption teamColour;

        public SetTeam()
        {
            TeamColourOption[] teamColourOptions = Resources.FindObjectsOfTypeAll<TeamColourOption>();
            int randomnumber = UnityEngine.Random.Range(0, 4);
            teamColour = teamColourOptions[randomnumber];
            name = $"You are now in {teamColour.name}"; // Improve this later
        }

        public override void Run()
        {
            CustomisationSelections customisationSelections = GlobalGameStateClient.Instance.PlayerProfile.CustomisationSelections;
            customisationSelections.ColourOption = teamColour;
            CustomisationManager.Instance.ApplyCustomisationsToFallGuy(chaos.fallGuy.gameObject, customisationSelections, 0);
            base.Run();
        }
    }
}
