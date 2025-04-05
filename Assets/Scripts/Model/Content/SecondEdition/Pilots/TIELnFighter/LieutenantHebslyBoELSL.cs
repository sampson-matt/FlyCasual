using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class LieutenantHebslyBoELSL : TIELnFighter
        {
            public LieutenantHebslyBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lieutenant Hebsly",
                    3,
                    38,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE
                    },
                    abilityType: typeof(LieutenantHebsly),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipAbilities.Add(new FormedUpBoEAbility());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(EvadeAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BoostAction), ActionColor.Red));
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                PilotNameCanonical = "lieutenant-hebsly-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you defend, you may perform a red boost action, even while stressed.
    public class LieutenantHebsly : GenericAbility
    {
        
        public override void ActivateAbility()
        {
            HostShip.OnAttackFinishAsDefender += CheckAttackFinishCondition;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackFinishAsDefender -= CheckAttackFinishCondition;
        }

        private void CheckAttackFinishCondition(GenericShip ship)
        {
            RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskUseAbility);
        }
        private void AskUseAbility(object sender, EventArgs e)
        {
            Selection.ThisShip = HostShip;
            Selection.ChangeActiveShip(HostShip);
            CameraScript.RestoreCamera();
            HostShip.AskPerformFreeAction(
                new BoostAction() { CanBePerformedWhileStressed = true, Color = ActionColor.Red },
                Triggers.FinishTrigger,
                HostShip.PilotInfo.PilotName,
                "After you defend, you may perform a red boost action, even while stressed.",
                HostShip
            );
        }
    }
}