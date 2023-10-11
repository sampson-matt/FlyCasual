using Ship;
using BoardTools;
using SubPhases;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.YT2400LightFreighter2023
    {
        public class Leebo2023ScumLSL : YT2400LightFreighter2023
        {
            public Leebo2023ScumLSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Leebo",
                    3,
                    69,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.Leebo2023Ability),
                    extraUpgradeIcon: UpgradeType.Talent,
                    factionOverride: Faction.Scum
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Crew);

                PilotNameCanonical = "leebo-swz103-sl-scumandvillainy";
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/YT2400/leebo-scum.png";
            }
        }
    }
}

