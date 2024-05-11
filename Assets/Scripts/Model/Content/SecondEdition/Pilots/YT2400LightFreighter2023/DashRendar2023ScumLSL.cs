using Ship;
using BoardTools;
using SubPhases;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.YT2400LightFreighter2023
    {
        public class DashRendar2023ScumLSL : YT2400LightFreighter2023
        {
            public DashRendar2023ScumLSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Dash Rendar",
                    5,
                    74,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.DashRendar2023Ability),
                    extraUpgradeIcon: UpgradeType.Talent,
                    factionOverride: Faction.Scum
                );

                PilotNameCanonical = "dashrendar-swz103-lsl-scumandvillainy";
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/YT2400/dashRendar-scum.png";
            }
        }
    }
}

