using Ship;
using Upgrade;
using ActionsList;
using Actions;
using UnityEngine;
using System;
using Tokens;
using System.Collections.Generic;
using BoardTools;

namespace UpgradesList.SecondEdition
{
    public class IG11 : GenericDualUpgrade
    {
        public IG11() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "IG-11",
                UpgradeType.Crew,
                cost: 6,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Scum),
                addAction: new ActionInfo(typeof(CalculateAction)),
                abilityType: typeof(Abilities.SecondEdition.Ig11CrewAbility)
            );
            SelectSideOnSetup = false;
            AnotherSide = typeof(IG11AntiCaptureProtocol);
        }
    }

    public class IG11AntiCaptureProtocol : GenericDualUpgrade
    {
        public IG11AntiCaptureProtocol() : base()
        {
            IsHidden = true; // Hidden in Squad Builder only
            NameCanonical = "ig11-sideb";

            UpgradeInfo = new UpgradeCardInfo(
                "IG-11 (Anti-Capture Protocol)",
                UpgradeType.Crew,
                cost: 6,
                fuses: 2,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Scum),
                abilityType: typeof(Abilities.SecondEdition.Ig11AntiCaptureAbility)
            );
            SelectSideOnSetup = false;
            AnotherSide = typeof(IG11AntiCaptureProtocol);
        }
    }
}


namespace Abilities.SecondEdition
{
    public class Ig11CrewAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnDamageCardIsDealt += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDamageCardIsDealt -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            if (Combat.CurrentCriticalHitCard.IsFaceup)
            {
                Triggers.RegisterTrigger(new Trigger()
                {
                    Name = HostName,
                    TriggerType = TriggerTypes.OnDamageCardIsDealt,
                    TriggerOwner = ship.Owner.PlayerNo,
                    EventHandler = AssignFuse,
                    Sender = ship
                });
            }
        }

        private void AssignFuse(object sender, EventArgs e)
        {
            HostUpgrade.State.AddFuse();
            HostShip.Tokens.AssignToken(typeof(CalculateToken), delegate { });
            Messages.ShowInfo(HostName + " places 1 Fuse Marker on " + HostUpgrade.UpgradeInfo.Name + " and gains 1 Calculate Token instead of suffering critical damage.");
            Combat.CurrentCriticalHitCard = null;
            if (HostUpgrade.State.Fuses >= 2)
            {
                (HostUpgrade as GenericDualUpgrade).Flip(delegate { Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel); });
            }
            Triggers.FinishTrigger();
        }
    }

    public class Ig11AntiCaptureAbility : GenericAbility
    {
        public GenericAbility Ability { get; set; }
        public override void ActivateAbility()
        {
            HostShip.OnGenerateActions += AddAction;
            Phases.Events.OnEndPhaseStart_Triggers += RemoveFuse;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGenerateActions -= AddAction;
            Phases.Events.OnEndPhaseStart_Triggers -= RemoveFuse;
        }

        private void RemoveFuse()
        {
            HostUpgrade.State.RemoveFuse();
            if(HostUpgrade.State.Fuses <= 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnEndPhaseStart, DestroyThisShip);
            }
        }

        private void DestroyThisShip(object sender, System.EventArgs e)
        {
            List<GenericShip> sufferedShips = new List<GenericShip>();

            foreach (var ship in Roster.AllShips.Values)
            {
                if (ship.ShipId == HostShip.ShipId) continue;

                DistanceInfo distInfo = new DistanceInfo(HostShip, ship);
                if (distInfo.Range < 2) sufferedShips.Add(ship);
            }

            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " is destroyed");
            HostShip.DestroyShipForced(delegate { DealDamageToShips(sufferedShips, 1, true, Triggers.FinishTrigger); });
        }

        private void AddAction(GenericShip ship)
        {
            ship.AddAvailableAction(new Ig11AntiCaptureAction()
            {
                ImageUrl = HostShip.ImageUrl,
                HostShip = HostShip,
                HostUpgrade = HostUpgrade,
                Ability = this,
                Name = "Add Fuse"
            }) ;
        }

        private class Ig11AntiCaptureAction : GenericAction
        {
            public GenericAbility Ability { get; set; }
            public GenericUpgrade HostUpgrade { get; set; }
            public override void ActionTake()
            {
                HostUpgrade.State.AddFuse();
                Phases.CurrentSubPhase.CallBack();
            }
        }
    }
}