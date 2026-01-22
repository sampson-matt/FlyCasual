using Arcs;
using BoardTools;
using Ship;
using System.Linq;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class Autoblasters : GenericSpecialWeapon
    {
        public Autoblasters() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Autoblasters",
                UpgradeType.Cannon,
                cost: 4,
                weaponInfo: new SpecialWeaponInfo(
                    attackValue: 2,
                    minRange: 1,
                    maxRange: 2,
                    arc: ArcType.Front
                ),
                abilityType: typeof(Abilities.SecondEdition.AutoblastersAbility)
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class AutoblastersAbility : GenericAbility
    {
        public override void ActivateAbility()
        {            
            HostShip.AfterGotNumberOfAttackDice += CheckForExtraDie;
            HostShip.OnDefenceStartAsAttacker += MakeCritsUncancellable;
        }


        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice -= CheckForExtraDie;
            HostShip.OnDefenceStartAsAttacker -= MakeCritsUncancellable;
        }

        private void CheckForExtraDie(ref int diceAmount)
        {
            if (Combat.ChosenWeapon.GetType() == HostUpgrade.GetType())
            {   
                if (Combat.Attacker.SectorsInfo.IsShipInSector(Combat.Defender, ArcType.Bullseye))
                {
                    Messages.ShowInfo("Target is in bullseye arc, Autoblaster rolls +1 attack die");
                    diceAmount++;
                }
            }
        }

        private void MakeCritsUncancellable()
        {
            if (Combat.ChosenWeapon.GetType() == HostUpgrade.GetType() && !Combat.Defender.SectorsInfo.IsShipInSector(Combat.Attacker, ArcType.Front))
            {
                foreach (Die die in Combat.DiceRollAttack.DiceList)
                {
                    if (die.Side == DieSide.Crit) die.IsUncancelable = true;
                }
            }
        }
    }
}