﻿using Abilities.SecondEdition;
using ActionsList;
using Ship;
using Upgrade;
using Content;
using System.Collections.Generic;

namespace Ship.SecondEdition.TIEInterceptor
{
    public class SecondSister : TIEInterceptor
    {
        public SecondSister() : base()
        {
            PilotInfo = new PilotCardInfo(
                "Second Sister",
                4,
                47,
                force: 2,
                pilotTitle: "Manipulative Monster",
                isLimited: true,
                abilityType: typeof(SecondSisterAbility),
                tags: new List<Tags>
                {
                    Tags.DarkSide
                },
                extraUpgradeIcon: UpgradeType.ForcePower
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class SecondSisterAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnGenerateDiceModificationsCompareResults += TrySecondSisterDiceMofication;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGenerateDiceModificationsCompareResults -= TrySecondSisterDiceMofication;
        }

        private void TrySecondSisterDiceMofication(GenericShip host)
        {
            if (Combat.DiceRollAttack.Successes > Combat.DiceRollDefence.Successes)
            {
                AddDiceModification(host);
            }
        }

        private void AddDiceModification(GenericShip host)
        {
            GenericAction newAction = new ActionsList.SecondEdition.SecondSisterDiceModification()
            {
                ImageUrl = HostShip.ImageUrl,
                HostShip = host,
            };
            host.AddAvailableDiceModificationOwn(newAction);
        }
    }
}


namespace ActionsList.SecondEdition
{
    public class SecondSisterDiceModification : GenericAction
    {
        public SecondSisterDiceModification()
        {
            Name = DiceModificationName = "Second Sister's ability";
            DiceModificationTiming = DiceModificationTimingType.CompareResults;
        }

        public override int GetDiceModificationPriority()
        {
            int result = 0;

            if (Combat.Defender.State.ShieldsCurrent < Combat.DiceRollAttack.Successes) result = 100;
            result -= ActionsHolder.CountEnemiesTargeting(HostShip) * 50;

            return result;
        }

        public override bool IsDiceModificationAvailable()
        {
            bool result = false;

            if (Combat.DiceRollAttack.Successes > Combat.DiceRollDefence.Successes && Combat.Attacker.State.Force >= 2)
            {
                result = true;
            }

            return result;
        }

        public override void ActionEffect(System.Action callBack)
        {
            Combat.DiceRollAttack.ChangeAll(DieSide.Success, DieSide.Crit, false);
            Combat.Attacker.State.SpendForce(2, callBack);
        }

    }
}