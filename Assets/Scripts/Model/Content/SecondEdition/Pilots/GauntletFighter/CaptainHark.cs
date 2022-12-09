using SubPhases;
using System;
using System.Collections.Generic;
using Upgrade;
using Movement;
using Ship;
using UpgradesList;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class CaptainHark : GauntletFighter
        {
            public CaptainHark() : base()
            {
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Captain Hark",
                    3,
                    51,
                    pilotTitle: "Obedient Underling",
                    isLimited: true,
                    //abilityType: typeof(Abilities.SecondEdition.CaptainHarkAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent},
                    factionOverride: Faction.Imperial
                );

                ModelInfo.SkinName = "Gray";

                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Title);

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/1/10/Captainhark.png";
            }
        }
    }
}

//namespace Abilities.SecondEdition
//{
//    public class CaptainHarkAbility : Abilities.SecondEdition.SwivelWingDownAbility
//    {
//        private GenericUpgrade SwivelWingUpgrade;
//        public override void ActivateAbility()
//        {
//            SwivelWingUpgrade = GetSwivelWing();
//            SwivelWingUpgrade.DeactivateAbility();
//            HostShip.OnManeuverIsRevealed += RegisterAskToRotate;
//        }

//        public override void DeactivateAbility()
//        {
//            HostShip.OnManeuverIsRevealed -= RegisterAskToRotate;
//            SwivelWingUpgrade = GetSwivelWing();
//            SwivelWingUpgrade.ActivateAbility();
//        }

//        protected override void RegisterAskToRotate(GenericShip ship)
//        {
//            if (ship.AssignedManeuver.Bearing == Movement.ManeuverBearing.Stationary)
//            {
//                RegisterAbilityTrigger(TriggerTypes.OnManeuverIsRevealed, AskToRotate);
//            }
//        }

//        protected override void AskToRotate(object sender, EventArgs e)
//        {
            
//            SwivelWingDecisionSubphase subphase = Phases.StartTemporarySubPhaseNew<SwivelWingDecisionSubphase>(
//                "Choose Wich Direction to Sideslip", 
//                delegate {
//                    (SwivelWingUpgrade as GenericDualUpgrade).Flip();        
//                    Triggers.FinishTrigger();
//                }
//             );

//            subphase.DescriptionShort = "Sideslip";
//            subphase.DescriptionLong = "Choose Wich Direction to Sideslip";
//            subphase.ImageSource = HostShip;

//            subphase.AddDecision("Left", DoSideSlipLeft);
//            subphase.AddDecision("Right", DoSideSlipRight);
            

            
//            //HostShip.WingsOpen();

//            subphase.Start();
//        }

//        private void FlipCard()
//        {
//            (SwivelWingUpgrade as GenericDualUpgrade).Flip();
//            Triggers.FinishTrigger();
//        }

//        private GenericUpgrade GetSwivelWing()
//        {
//            return HostShip.UpgradeBar.GetUpgradesAll().Find(n => n.UpgradeInfo.HasType(UpgradeType.Configuration));
//        }

//        private void DoSideSlipRight(object sender, EventArgs e)
//        {
//            GenericMovement movement = new SideslipBankMovement(
//                1,
//                ManeuverDirection.Right,
//                ManeuverBearing.SideslipBank,
//                GenericMovement.IncreaseComplexity(HostShip.RevealedManeuver.ColorComplexity)
//            );

//            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: Maneuver is changed to Sideslip");
//            HostShip.SetAssignedManeuver(movement);

//            Triggers.FinishTrigger();
//        }

//        private void DoSideSlipLeft(object sender, EventArgs e)
//        {
//            GenericMovement movement = new SideslipBankMovement(
//                1,
//                ManeuverDirection.Left,
//                ManeuverBearing.SideslipBank,
//                GenericMovement.IncreaseComplexity(HostShip.RevealedManeuver.ColorComplexity)
//            );

//            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: Maneuver is changed to Sideslip");
//            HostShip.SetAssignedManeuver(movement);

//            Triggers.FinishTrigger();
//        }
//    }
//}