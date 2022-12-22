using Upgrade;
using Content;
using BoardTools;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class Backstabber : TIELnFighter
        {
            public Backstabber() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Backstabber\"",
                    5,
                    30,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.BackstabberAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/backstabber-boy.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BackstabberAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnShotStartAsAttacker += CheckConditionsOffense;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnShotStartAsAttacker -= CheckConditionsOffense;
        }

        private void CheckConditionsOffense()
        {
            int countLeft = Board.GetShipsInArcAtRange(HostShip, Arcs.ArcType.Left, new UnityEngine.Vector2(0, 1), Team.Type.Friendly).FindAll(s => s.PilotInfo.PilotName == "Darth Vader" || s.PilotInfo.PilotName == "\"Mauler\" Mithel").Count;
            int countRight = Board.GetShipsInArcAtRange(HostShip, Arcs.ArcType.Right, new UnityEngine.Vector2(0, 1), Team.Type.Friendly).FindAll(s => s.PilotInfo.PilotName == "Darth Vader" || s.PilotInfo.PilotName == "\"Mauler\" Mithel").Count;
            if (countLeft + countRight >= 1)
            {
                HostShip.AfterGotNumberOfAttackDice += RollExtraAttackDice;
            }
        }

        private void RollExtraAttackDice(ref int count)
        {
            count++;
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " gains +1 attack die");
            HostShip.AfterGotNumberOfAttackDice -= RollExtraAttackDice;
        }
    }
}