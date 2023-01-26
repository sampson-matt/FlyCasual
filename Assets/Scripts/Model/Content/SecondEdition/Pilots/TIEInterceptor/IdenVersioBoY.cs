using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class IdenVersioBoY : TIEInterceptor
        {
            public IdenVersioBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Iden Versio",
                    4,
                    64,
                    isLimited: true,
                    charges: 2,
                    regensCharges: 1,
                    abilityType: typeof(IdenVersioBoYAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());
                PilotNameCanonical = "idenversio-boy";
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/idenversio-boy.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class IdenVersioBoYAbility : GenericAbility
    {
        private GenericShip curToDamage;

        public override void ActivateAbility()
        {
            GenericShip.OnTryDamagePreventionGlobal += CheckIdenVersioAbilitySE;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnTryDamagePreventionGlobal -= CheckIdenVersioAbilitySE;
        }

        private void CheckIdenVersioAbilitySE(GenericShip toDamage, DamageSourceEventArgs e)
        {
            curToDamage = toDamage;

            // Is the defender on our team? If not return.
            if (!Tools.IsFriendly(curToDamage, HostShip))
                return;

            if (!(curToDamage.ShipInfo as ShipCardInfo).Tags.Contains(Tags.Tie))
                return;

            // If the defender is at range one of us we register our trigger to prevent damage.
            BoardTools.DistanceInfo distanceInfo = new BoardTools.DistanceInfo(curToDamage, HostShip);
            if (distanceInfo.Range <= 1)
            {
                RegisterAbilityTrigger(TriggerTypes.OnTryDamagePrevention, UseIdenVersioAbilitySE);
            }
        }

        private void UseIdenVersioAbilitySE(object sender, System.EventArgs e)
        {
            // Are there any non-crit damage results in the damage queue?
            if (HostShip.State.Charges > 1)
            {
                // If there are we prompt to see if they want to use the ability.
                AskToUseAbility(
                    HostShip.PilotInfo.PilotName,
                    AlwaysUseByDefault,
                    delegate { BlankDamage(); },
                    descriptionLong: "Do you want to spend 2 Charges to prevent 1 damage?",
                    imageHolder: HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void BlankDamage()
        {
            HostShip.SpendCharges(2);
            if (curToDamage.AssignedDamageDiceroll.CriticalSuccesses > 0)
            {
                curToDamage.AssignedDamageDiceroll.RemoveType(DieSide.Crit);
            }
            else
            {
                curToDamage.AssignedDamageDiceroll.RemoveType(DieSide.Success);
            }
            DecisionSubPhase.ConfirmDecision();
        }


    }
}