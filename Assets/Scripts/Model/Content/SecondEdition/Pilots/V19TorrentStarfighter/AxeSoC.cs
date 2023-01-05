using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;
using Content;

namespace Ship.SecondEdition.V19TorrentStarfighter
{
    public class AxeSoC : V19TorrentStarfighter
    {
        public AxeSoC()
        {
            PilotInfo = new PilotCardInfo(
                "\"Axe\"",
                3,
                33,
                true,
                tags: new List<Tags>
                {
                    Tags.SoC
                },
                abilityType: typeof(Abilities.SecondEdition.AxeSoCAbility),
                extraUpgradeIcon: UpgradeType.Talent
            );
            ShipInfo.Hull++;
            ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
            ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

            PilotNameCanonical = "axe-soc";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/axe-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you defend or perform an attack, you may choose a friendly ship at range 0-2 in your left or right arc. 
    //the chosen ship gains a lock on the defender.
    public class AxeSoCAbility : GenericAbility
    {
        private ITargetLockable LockedShip;

        public override void ActivateAbility()
        {
            HostShip.OnAttackFinish += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackFinish -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            LockedShip = Combat.Defender;
            if (LockedShip != null && Roster.AllShips.Values.Any(s => FilterAbilityTargets(s)))
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskAbility);
            }
            
        }

        private void AskAbility(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                GetTargetLockOnDefender,
                FilterAbilityTargets,
                AiPriority,
                HostShip.Owner.PlayerNo,
                HostName,
                "You may choose a ship in your left or right arc to acquire a lock on the defender",
                HostShip
            );
        }

        private int AiPriority(GenericShip ship)
        {
            int priority = 0;

            if (!ship.Tokens.HasToken(typeof(BlueTargetLockToken))) priority += 50;

            if (LockedShip is GenericShip)
            {
                BoardTools.ShotInfo shotInfo = new BoardTools.ShotInfo(ship, LockedShip as GenericShip, ship.PrimaryWeapons);
                if (shotInfo.IsShotAvailable) priority += 40;
            }

            priority += ship.State.Firepower * 5;

            return priority;
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            return FilterByTargetType(ship, new List<TargetTypes>() { TargetTypes.OtherFriendly })
                && ship.ShipAbilities.Any(n => n.GetType() == typeof(Abilities.SecondEdition.BornForThisAbility))
                && FilterTargetsByRange(ship, 0, 2)
                && (HostShip.SectorsInfo.IsShipInSector(ship, Arcs.ArcType.Left) || HostShip.SectorsInfo.IsShipInSector(ship, Arcs.ArcType.Right));
        }

        private void GetTargetLockOnDefender()
        {
            if (LockedShip is GenericShip)
            {
                Messages.ShowInfo(TargetShip.PilotInfo.PilotName + " acquired a Target Lock on " + (LockedShip as GenericShip).PilotInfo.PilotName);
            }
            else
            {
                Messages.ShowInfo(TargetShip.PilotInfo.PilotName + " acquired a Target Lock on obstacle");
            }

            ActionsHolder.AcquireTargetLock(TargetShip, LockedShip, SelectShipSubPhase.FinishSelection, SelectShipSubPhase.FinishSelection, ignoreRange: true);
        }
    }
}
