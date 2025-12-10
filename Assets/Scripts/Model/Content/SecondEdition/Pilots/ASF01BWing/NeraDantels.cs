using Abilities.SecondEdition;
using ActionsList;
using Arcs;
using Ship;
using System;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class NeraDantels : ASF01BWing
        {
            public NeraDantels() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Nera Dantels",
                    3,
                    44,
                    isLimited: true,
                    abilityType: typeof(NeraDantelsAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ShipInfo.ArcInfo.Arcs.Add(new ShipArcInfo(ArcType.SingleTurret));

                PilotNameCanonical = "neradantels-wat1";

                ModelInfo.SkinName = "Red";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class NeraDantelsAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnGameStart += RestrictTorpedoArcRequirements;
            HostShip.OnTokenIsAssigned += TryRegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGameStart -= RestrictTorpedoArcRequirements;
            HostShip.OnTokenIsAssigned -= TryRegisterAbility;
        }

        private void TryRegisterAbility(GenericShip ship, GenericToken token)
        {
            if (token is StressToken)
            {
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsAssigned, PerformAction);
            }
        }

        private void PerformAction(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                UseRotateAbility,
                descriptionLong: "Do you want to rotate your turret arc indicator?",
                imageHolder: HostShip
            );
        }

        private void UseRotateAbility(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            new RotateArcAction().DoOnlyEffect(Triggers.FinishTrigger);
        }

        private void RestrictTorpedoArcRequirements()
        {
            foreach (GenericUpgrade weaponUpgrade in HostShip.UpgradeBar.GetSpecialWeaponsAll())
            {
                IShipWeapon specialWeapon = weaponUpgrade as IShipWeapon;
                if (specialWeapon.WeaponType == WeaponTypes.Torpedo)
                {
                    if (specialWeapon.WeaponInfo.ArcRestrictions.Contains(ArcType.Front))
                    {
                        specialWeapon.WeaponInfo.ArcRestrictions.Remove(ArcType.Front);
                        specialWeapon.WeaponInfo.ArcRestrictions.Add(ArcType.SingleTurret);
                    }
                }
            }
        }
    }
}