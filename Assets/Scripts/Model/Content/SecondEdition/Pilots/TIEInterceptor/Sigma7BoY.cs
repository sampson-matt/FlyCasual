using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using Tokens;
using ActionsList;
using Actions;
using BoardTools;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sigma7 : TIEInterceptor
        {
            public Sigma7() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sigma7",
                    4,
                    43,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Sigma7Ability),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "sigma7-battleofyavin";

                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(TargetLockAction)));
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/sigma7-boy.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class Sigma7Ability : GenericAbility
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

        private void CheckForAbility(GenericShip ship, ref bool flag)
        {
            if (HostShip.State.Charges >= 1 && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0, 1), Team.Type.Enemy).Count > 0) flag = true;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (HostShip.State.Charges >= 1 && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0, 1), Team.Type.Enemy).Count > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToUseSigma7Ability);
            }
        }

        private void AskToUseSigma7Ability(object sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                    GrantFreeTargetLock,
                    FilterAbilityTargets,
                    GetAiAbilityPriority,
                    HostShip.Owner.PlayerNo,
                    HostName,
                    "You may spend 1 Charge to aquire a lock on an enemy at range 1",
                    HostShip
                );
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            int priority = 0;

            if (!HostShip.Tokens.HasToken(typeof(BlueTargetLockToken))) priority += 50;
            ShotInfo shotInfo = new ShotInfo(HostShip, ship, ship.PrimaryWeapons);
            if (shotInfo.IsShotAvailable) priority += 40;

            priority += ship.PilotInfo.Cost;

            return priority;
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            var range = new DistanceInfo(HostShip, ship).Range;
            return ship.Owner != HostShip.Owner && range <= 1;
        }

        private void GrantFreeTargetLock()
        {
            if (TargetShip != null)
            {
                ActionsHolder.AcquireTargetLock(HostShip, TargetShip, SelectShipSubPhase.FinishSelection, SelectShipSubPhase.FinishSelection);
                HostShip.SpendCharge();
            }
            else
            {
                SelectShipSubPhase.FinishSelection();
            }
        }
    }
}