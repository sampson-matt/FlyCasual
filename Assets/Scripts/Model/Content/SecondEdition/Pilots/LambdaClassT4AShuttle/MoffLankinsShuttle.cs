using System.Collections.Generic;
using Mods.ModsList;
using System;
using Tokens;

namespace Ship
{
    namespace SecondEdition.LambdaClassT4AShuttle
    {
        public class MoffLankinsShuttle : LambdaClassT4AShuttle
        {
            public MoffLankinsShuttle() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Moff Lankin's Shuttle",
                    1,
                    0,
                    abilityType: typeof(Abilities.SecondEdition.MoffLankinsShuttleAbility)
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/omicrongrouppilot.png";

                RequiredMods = new List<Type>() { typeof(HotacEliteImperialPilotsModSE) };
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MoffLankinsShuttleAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnSetupEnd += AddShieldsAbility;
            HostShip.OnDamageWasSuccessfullyDealt += CheckDisabled;
            Phases.Events.OnRoundStart += CheckDisabledWeapons;
        }


        public override void DeactivateAbility()
        {
            Phases.Events.OnSetupEnd -= AddShieldsAbility;
            HostShip.OnDamageWasSuccessfullyDealt -= CheckDisabled;
            Phases.Events.OnRoundStart -= CheckDisabledWeapons;
        }

        private void CheckDisabled(Ship.GenericShip ship, bool flag)
        {
            if(!HostShip.State.IsDisabled &&HostShip.State.HullCurrent<=3 && HostShip.State.HullCurrent>0)
            {
                HostShip.ToggleIonized(true);
                HostShip.State.IsDisabled = true;
                HostShip.Tokens.AssignToken(typeof(WeaponsDisabledToken), DisplayMessage );
            }
            if(!HostShip.IsFleeing && (HostShip.State.ShieldsCurrent==0||HostShip.State.ShieldsCurrent<=HostShip.State.ShieldsMax-5))
            {
                HostShip.IsFleeing = true;
                HostShip.EscapeEdge = "north";
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + " is attempting to flee.");
            }
        }

        private void DisplayMessage()
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " has been disabled.");
        }


        private void CheckDisabledWeapons()
        {
            if (HostShip.State.IsDisabled)
            {
                RegisterAbilityTrigger(TriggerTypes.OnRoundStart, AssignWeaponsDisabledTrigger);
            }
        }

        private void AssignWeaponsDisabledTrigger(object sender, System.EventArgs e)
        {
            HostShip.Tokens.AssignToken(typeof(WeaponsDisabledToken), Triggers.FinishTrigger);
        }

        private void AddShieldsAbility()
        {
            RegisterAbilityTrigger(TriggerTypes.OnSetupEnd, AddShields);
            
        }

        private void AddShields(object sender, EventArgs e)
        {
            switch (Roster.Player1.Ships.Count)
            {
                case 3:
                    HostShip.State.ShieldsMax += 2;
                    HostShip.ChangeShieldBy(2);
                    break;
                case 4:
                    HostShip.State.ShieldsMax += 4;
                    HostShip.ChangeShieldBy(4);
                    break;
                case 5:
                    HostShip.State.ShieldsMax += 6;
                    HostShip.ChangeShieldBy(6);
                    break;
                case 6:
                    HostShip.State.ShieldsMax += 8;
                    HostShip.ChangeShieldBy(8);
                    break;
            }
            Triggers.FinishTrigger();
        }
    }
}
