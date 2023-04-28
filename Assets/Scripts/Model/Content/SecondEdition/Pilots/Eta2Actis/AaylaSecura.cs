using BoardTools;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;
using Content;

namespace Ship.SecondEdition.Eta2Actis
{
    public class AaylaSecura : Eta2Actis
    {
        public AaylaSecura()
        {
            PilotInfo = new PilotCardInfo(
                "Aayla Secura",
                5,
                48,
                true,
                force: 2,
                abilityType: typeof(Abilities.SecondEdition.AaylaSecuraActisAbility),
                tags: new List<Tags>
                {
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcon: UpgradeType.Talent
            );

            ModelInfo.SkinName = "Blue";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AaylaSecuraActisAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                "Aayla Secura",
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Change,
                count: 1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Blank },
                sideCanBeChangedTo: DieSide.Focus,
                isGlobal: true
            );
        }

        private bool IsAvailable()
        {
            bool result = false;

            if (Combat.AttackStep == CombatStep.Defence && Combat.Attacker.Owner.PlayerNo != HostShip.Owner.PlayerNo)
            {
                ShotInfoArc arcInfo = new ShotInfoArc(
                    HostShip,
                    Combat.Attacker,
                    HostShip.SectorsInfo.Arcs.First(n => n.Facing == Arcs.ArcFacing.Front)
                );

                if (arcInfo.InArc && arcInfo.Range <= 1) result = true;
            }

            return result;
        }

        private int GetAiPriority()
        {
            return 100;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }
    }
}
