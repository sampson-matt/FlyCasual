using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
using ActionsList;
using Movement;
using Arcs;
using System;
using Mods.ModsList;
using Ship;

namespace Ship
{
    namespace SecondEdition.Hwk290LightFreighter
    {
        public class DamagedHawk : Hwk290LightFreighter
        {
            public DamagedHawk() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Rebel Operative",
                    1,
                    0,
                    abilityType: typeof(Abilities.SecondEdition.DamagedHawkAbility),
                    seImageNumber: 45
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
                ShipInfo.ActionIcons.RemoveActions(typeof(BoostAction));
                DialInfo.ChangeManeuverComplexity(new ManeuverHolder(ManeuverSpeed.Speed4, ManeuverDirection.Forward, ManeuverBearing.Straight), MovementComplexity.Complex);
                ShipInfo.ArcInfo = new ShipArcsInfo(ArcType.None, 0);
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DamagedHawkAbility : GenericAbility
    {
        public override string Name { get { return "Damaged Hawk"; } }

        public override void ActivateAbility()
        {
             Phases.Events.OnSetupEnd += DealDamage;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnSetupEnd -= DealDamage;
        }

        private void DealDamage()
        {

            HostShip.State.ShieldsMax = Roster.Player1.Ships.Count - 1;
            HostShip.State.ShieldsCurrent = Roster.Player1.Ships.Count - 1;
            RegisterAbilityTrigger(TriggerTypes.OnSetupEnd, AssignCritCard);



        }

        private void AssignCritCard(object sender, EventArgs e)
        {
            Phases.CurrentSubPhase.Pause();

            DamageDeck Deck = DamageDecks.GetDamageDeck(HostShip.Owner.PlayerNo);
            GenericDamageCard critCard = (GenericDamageCard)System.Activator.CreateInstance(typeof(DamageDeckCardSE.DamagedSensorArray));
            Deck.PutOnTop(critCard);
            HostShip.SufferHullDamage(
                true,
                new DamageSourceEventArgs
                {
                    Source = HostShip,
                    DamageType = DamageTypes.CardAbility
                },
                CleanUp
            );
        }

        private void CleanUp()
        {
            Phases.CurrentSubPhase.Resume();
            Triggers.FinishTrigger();
        }
    }
}
