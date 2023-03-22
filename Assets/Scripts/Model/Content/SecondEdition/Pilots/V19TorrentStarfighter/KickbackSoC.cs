using Actions;
using ActionsList;
using Ship;
using Content;
using System.Collections.Generic;
using Upgrade;
using System;

namespace Ship.SecondEdition.V19TorrentStarfighter
{
    public class KickbackSoC : V19TorrentStarfighter
    {
        public KickbackSoC()
        {
            PilotInfo = new PilotCardInfo(
                "\"Kickback\"",
                4,
                33,
                true,
                abilityType: typeof(Abilities.SecondEdition.KickbackSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC
                },
                extraUpgradeIcon: UpgradeType.Talent
            );
            ShipInfo.Hull++;
            ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
            ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

            PilotNameCanonical = "kickback-siegeofcoruscant";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/kickback-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class KickbackSoCAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckConditions;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckConditions;
        }

        private void CheckConditions(GenericAction action)
        {
            if (action is BarrelRollAction)
            {
                HostShip.OnActionDecisionSubphaseEnd += PerformLockAction;
            }
        }

        private void PerformLockAction(GenericShip ship)
        {
            HostShip.OnActionDecisionSubphaseEnd -= PerformLockAction;

            RegisterAbilityTrigger(TriggerTypes.OnFreeAction, AskPerformBoostAction);
        }

        private void AskPerformBoostAction(object sender, System.EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += CheckAbility;
            HostShip.AskPerformFreeAction(
                new TargetLockAction() { Color = ActionColor.Red },
                Triggers.FinishTrigger,
                HostShip.PilotInfo.PilotName,
                "After you perform a Barrel Roll action, you may perform a red Lock action",
                HostShip
            );
        }

        private void CheckAbility(GenericAction action, ref bool isFree)
        {
            HostShip.BeforeActionIsPerformed -= CheckAbility;
            if (action is TargetLockAction)
            {
                RegisterAbilityTrigger(TriggerTypes.BeforeActionIsPerformed, AskToTreatAsWhite);
            }
        }

        private void AskToTreatAsWhite(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotName,
                NeverUseByDefault,
                TreatActionAsWhite,
                descriptionLong: "Do you want to gain 1 strain and treat action as white?",
                imageHolder: HostShip
            );
        }

        private void TreatActionAsWhite(object sender, EventArgs e)
        {
            HostShip.OnCheckActionColor += TreatThisTargetLockActionAsWhite;
            HostShip.Tokens.AssignToken(typeof(Tokens.StrainToken), SubPhases.DecisionSubPhase.ConfirmDecision);
        }

        private void TreatThisTargetLockActionAsWhite(GenericAction action, ref ActionColor color)
        {
            if (action is TargetLockAction && color == ActionColor.Red)
            {
                color = ActionColor.White;
                HostShip.OnCheckActionColor -= TreatThisTargetLockActionAsWhite;
            }
        }
    }
}
