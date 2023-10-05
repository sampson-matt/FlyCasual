using Ship;
using System;
using System.Collections.Generic;
using Content;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class JagSoC : ARC170Starfighter
        {
            public JagSoC() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Jag\"",
                    3,
                    47,
                    isLimited: true,
                    factionOverride: Faction.Republic,
                    abilityType: typeof(Abilities.SecondEdition.JagSoCAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC
                    }
                );
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                PilotNameCanonical = "jag-siegeofcoruscant";

                ModelInfo.SkinName = "Red";

                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/jag-soc.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After a friendly ship at range 1-2 in your left or right arc performs an attack, if you are not strained, you may acquire a lock on the defender.
    public class JagSoCAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnAttackFinishGlobal += CheckAbility;
        }
        public override void DeactivateAbility()
        {
            GenericShip.OnAttackFinishGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
    {
            var rangeLeft = HostShip.SectorsInfo.RangeToShipBySector(Combat.Attacker, Arcs.ArcType.Left);
            var rangeRight = HostShip.SectorsInfo.RangeToShipBySector(Combat.Attacker, Arcs.ArcType.Right);

            if (Tools.IsFriendly(Combat.Attacker, HostShip)
                && !HostShip.IsStrained
                && ((HostShip.SectorsInfo.IsShipInSector(Combat.Attacker, Arcs.ArcType.Left) && rangeLeft >= 1 && rangeLeft <= 2)
                    || (HostShip.SectorsInfo.IsShipInSector(Combat.Attacker, Arcs.ArcType.Right) && rangeRight >= 1 && rangeRight <= 2)))
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, delegate
                {
                    AskToUseAbility(
                        HostShip.PilotInfo.PilotName,
                        AlwaysUseByDefault,
                        AcquireLock,
                        descriptionLong: "Do you want to acquire a lock on the Defender?",
                        imageHolder: HostShip
                    );
                });                
            }
        }

        private void AcquireLock(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostName + ": Acquires lock on " + Combat.Defender.PilotInfo.PilotName);
            ActionsHolder.AcquireTargetLock(HostShip, Combat.Defender, SubPhases.DecisionSubPhase.ConfirmDecision, SubPhases.DecisionSubPhase.ConfirmDecision);
        }
    }
}