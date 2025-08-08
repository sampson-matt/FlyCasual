using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using System;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class AdonFoxBoELSL : ASF01BWing
        {
            public AdonFoxBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Adon Fox",
                    1,
                    46,
                    isLimited: true,
                    abilityType: typeof(AdonFoxBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.LsL
                    },
                    charges: 2,
                    regensCharges: 1
                );
                ShipAbilities.Add(new GyroCockpit());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(TargetLockAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ReloadAction), ActionColor.Red));
                ModelInfo.SkinName = "Blue";
                PilotNameCanonical = "adonfox-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AdonFoxBattleOverEndorAbility : GenericAbility
    {
        //While you defend, if you are stressed, roll 1 additional die.
        public override void ActivateAbility()
        {
            HostShip.OnDefenceStartAsDefender += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDefenceStartAsDefender -= CheckAbility;
        }
        private void CheckAbility()
        {
            if (HostShip.IsStressed)
            {
                RegisterAbilityTrigger(TriggerTypes.OnDefenseStart, UseAbility);
            }
        }

        private void UseAbility(object sender, EventArgs e)
        {
            if (Combat.Defender == HostShip)
            {
                Messages.ShowInfo($"{HostShip.PilotInfo.PilotName} adds an extra defense die");
                HostShip.AfterGotNumberOfDefenceDice += AddDefenseDie;
                Triggers.FinishTrigger();
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void AddDefenseDie(ref int dieCount)
        {
            HostShip.AfterGotNumberOfDefenceDice -= AddDefenseDie;
            dieCount++;
        }
    }
}
