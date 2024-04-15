using FG.Common;
using FGClient;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FGChaos.Effects
{
    public class SetTeam : Effect
    {
        public SetTeam()
        {
            TeamColourOption[] teamColourOptions = Resources.FindObjectsOfTypeAll<TeamColourOption>();
            if (teamColourOptions.Length == 4)
            {
                int randomnumber = UnityEngine.Random.Range(0, 4);
                teamColour = teamColourOptions[randomnumber];
                string teamName = Regex.Replace(teamColour.name, @"[\d-]", string.Empty);
                Name = $"You are now in {teamName}";
            }
            else
            {
                Name = "SetTeam";
            }
            ID = "SetTeam";
        }

        TeamColourOption teamColour;

        public override void Run()
        {
            CustomisationSelections customisationSelections = GlobalGameStateClient.Instance.PlayerProfile.CustomisationSelections;
            customisationSelections.ColourOption = teamColour;
            CustomisationManager.Instance.ApplyCustomisationsToFallGuy(chaos.fallGuy.gameObject, customisationSelections, 0);
            base.Run();
        }
    }
}
