using Ship;
using Upgrade;
using UnityEngine;
using Tokens;
using System.Linq;
using Conditions;
using System;

namespace UpgradesList.SecondEdition
{
    public class FlightInstructor : GenericUpgrade
    {
        public FlightInstructor() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Flight Instructor",
                UpgradeType.Crew,
                cost: 8,
                abilityType: typeof(Abilities.FirstEdition.FlightInstructorAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/crew/flightinstructor.png";
        }        
    }
}