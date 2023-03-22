using Abilities.SecondEdition;
using Arcs;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BoardTools;
using Content;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class HolOkandBoY : BTLA4YWing
        {
            public HolOkandBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hol Okand",
                    4,
                    34,
                    pilotTitle: "Battle of Yavin",
                    isLimited: true,
                    abilityType: typeof(HolOkandBoYAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Modification }
                );
                ShipAbilities.Add(new HopeAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/holokand-boy.png";
                PilotNameCanonical = "holokand-battleofyavin";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class HolOkandBoYAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation += CheckForAbility;
            HostShip.OnSystemsAbilityActivation += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation -= CheckForAbility;
            HostShip.OnSystemsAbilityActivation -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (HasUpgradeToRecharge() && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(1, 2), Team.Type.Enemy).Count == 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToUseHolOkandBoYAbility);
            }
        }

        private void AskToUseHolOkandBoYAbility(object sender, EventArgs e)
        {
            HolOkandDecisionSubphase subphase = Phases.StartTemporarySubPhaseNew<HolOkandDecisionSubphase>("Hol Okand Subphase Decision", Triggers.FinishTrigger);

            subphase.DescriptionShort = HostShip.PilotInfo.PilotName;
            subphase.DescriptionLong = "You may recover one Charge on any upgrade";
            subphase.ImageSource = HostShip;

            foreach (GenericUpgrade upgrade in HostShip.UpgradeBar.GetUpgradesAll())
            {
                if (upgrade.State.MaxCharges > 0
                    && upgrade.State.Charges < upgrade.State.MaxCharges
                    && !upgrade.UpgradeInfo.CannotBeRecharged
                )
                {
                    subphase.AddDecision(
                        upgrade.UpgradeInfo.Name,
                        delegate { RecoverUpgradeCharge(upgrade); },
                        upgrade.ImageUrl
                    );
                }
            }

            subphase.ShowSkipButton = true;
            subphase.DecisionOwner = HostShip.Owner;
            subphase.DefaultDecisionName = subphase.GetDecisions().First()?.Name;

            subphase.Start();
        }

        private void RecoverUpgradeCharge(GenericUpgrade upgrade)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            upgrade.State.RestoreCharges(1);
            Triggers.FinishTrigger();
        }

        private void CheckForAbility(GenericShip ship, ref bool flag)
        {
            if (HasUpgradeToRecharge() && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(1, 2), Team.Type.Enemy).Count == 0) flag = true;
        }

        private bool HasUpgradeToRecharge()
        {
            foreach (GenericUpgrade upgrade in HostShip.UpgradeBar.GetUpgradesAll())
            {
                if (upgrade.State.MaxCharges > 0
                    && upgrade.State.Charges < upgrade.State.MaxCharges
                    && !upgrade.UpgradeInfo.CannotBeRecharged
                )
                {
                    return true;
                }
            }

            return false;
        }

        private class HolOkandDecisionSubphase : DecisionSubPhase { }
    }
}