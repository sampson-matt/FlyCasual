using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Content;
using Tokens;
using Upgrade;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class DisT81SoC : DroidTriFighter
    {
        public DisT81SoC()
        {
            PilotInfo = new PilotCardInfo(
                "DIS-T81",
                4,
                38,
                true,
                extraUpgradeIcon: UpgradeType.Talent,
                abilityType: typeof(Abilities.SecondEdition.DisT81SoCAbility),
                pilotTitle: "Siege of Coruscant",
                tags: new List<Tags>
                {
                    Tags.SoC
                }
            );

            PilotNameCanonical = "dist81-soc";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/dist81-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DisT81SoCAbility : GenericAbility
    {
        private int numberRerolled;
        public override void ActivateAbility()
        {
            AddDiceModification(
                "DIS-T81",
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Reroll,
                GetRerollCount,
                payAbilityPostCost: PayAbilityCost
            );
        }

        private int GetRerollCount()
        {
            return int.MaxValue;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private bool IsAvailable()
        {
            return (Combat.AttackStep == CombatStep.Attack && Combat.Attacker == HostShip) ||
               (Combat.AttackStep == CombatStep.Defence && Combat.Defender == HostShip);
        }

        private int GetAiPriority()
        {            
            return 81;
        }

        private void PayAbilityCost()
        {
            numberRerolled = Combat.CurrentDiceRoll.DiceWereSelectedForRerollCount;
            if (Combat.Attacker == HostShip)
            {
                HostShip.AfterAttackWindow += AssignDepelete;
            }
            else
            {
                for (int i = 0; i < numberRerolled; i++)
                {
                    HostShip.Tokens.AssignToken(typeof(StrainToken), delegate { });
                }
            }
        }

        private void AssignDepelete()
        {
            HostShip.AfterAttackWindow -= AssignDepelete;
            for (int i = 0; i < numberRerolled; i++)
            {
                HostShip.Tokens.AssignToken(typeof(DepleteToken), delegate { });
            }
        }
    }
}
