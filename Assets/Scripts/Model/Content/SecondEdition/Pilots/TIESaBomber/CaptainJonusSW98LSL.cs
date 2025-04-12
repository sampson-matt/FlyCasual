using ActionsList;
using Bombs;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class CaptainJonusSW98LSL : TIESaBomber
        {
            public CaptainJonusSW98LSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Captain Jonus",
                    4,
                    36,
                     tags: new List<Tags>
                    {
                        Tags.LsL
                    },
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CaptainJonusLSLAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                PilotNameCanonical = "captainjonus-swz98-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you drop or launch a device, gain an evade token.
    public class CaptainJonusLSLAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnBombWasDropped += CheckAbilityOnDrop;
            HostShip.OnBombWasLaunched += CheckAbilityOnLaunch;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnBombWasDropped += CheckAbilityOnDrop;
            HostShip.OnBombWasLaunched += CheckAbilityOnLaunch;
        }

        private void CheckAbilityOnDrop()
        {
            RegisterAbilityTrigger(TriggerTypes.OnBombWasDropped, GetEvadeToken);
        }

        private void CheckAbilityOnLaunch()
        {
            RegisterAbilityTrigger(TriggerTypes.OnBombWasLaunched, GetEvadeToken);
        }

        private void GetEvadeToken(object sender, System.EventArgs e)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " gains Evade token");
            HostShip.Tokens.AssignToken(typeof(Tokens.EvadeToken), Triggers.FinishTrigger);
        }
    }
}