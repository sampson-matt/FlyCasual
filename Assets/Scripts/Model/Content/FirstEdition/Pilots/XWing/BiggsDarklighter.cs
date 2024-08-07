﻿using System.Collections;
using System.Collections.Generic;
using Ship;
using Abilities.FirstEdition;
using System;
using Conditions;
using Tokens;

namespace Ship
{
    namespace FirstEdition.XWing
    {
        public class BiggsDarklighter : XWing
        {
            public BiggsDarklighter() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Biggs Darklighter",
                    5,
                    25,
                    isLimited: true,
                    abilityType: typeof(BiggsDarklighterAbility)
                );

                ModelInfo.SkinName = "Biggs Darklighter";
            }
        }
    }
}

namespace Abilities.FirstEdition
{
    public class BiggsDarklighterAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += RegisterAskBiggsAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= RegisterAskBiggsAbility;
        }

        private void RegisterAskBiggsAbility()
        {
            if (!IsAbilityUsed)
            {
                RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, AskUseAbility);
            }
        }

        private void AskUseAbility(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                ActivateBiggsAbility,
                descriptionLong: "Do you want to activate ability? (Until the end of the round, other friendly ships at Range 1 cannot be targeted by attacks if the attacker could target you instead)",
                imageHolder: HostShip
            );
        }

        private void ActivateBiggsAbility(object sender, System.EventArgs e)
        {
            IsAbilityUsed = true;
            HostShip.Tokens.AssignCondition(typeof(BiggsDarklighterCondition));

            GenericShip.OnTryPerformAttackGlobal += CanPerformAttack;

            HostShip.OnShipIsDestroyed += RemoveBiggsDarklighterAbility;
            Phases.Events.OnCombatPhaseEnd_NoTriggers += RemoveBiggsDarklighterAbility;

            SubPhases.DecisionSubPhase.ConfirmDecision();
        }

        public void CanPerformAttack(ref bool result, List<string> stringList)
        {
            bool shipIsProtected = false;
            if (Selection.AnotherShip.ShipId != HostShip.ShipId)
            {
                if (Tools.IsFriendly(Selection.AnotherShip, HostShip))
                {
                    BoardTools.DistanceInfo positionInfo = new BoardTools.DistanceInfo(Selection.AnotherShip, HostShip);
                    if (positionInfo.Range <= 1)
                    {
                        if (!Selection.ThisShip.ShipsBumped.Contains(HostShip))
                        {
                            if (Combat.ChosenWeapon.IsShotAvailable(HostShip)) shipIsProtected = true;
                        }
                    }
                }
            }

            if (shipIsProtected)
            {
                if (Roster.GetPlayer(Phases.CurrentPhasePlayer).GetType() == typeof(Players.HumanPlayer))
                {
                    stringList.Add("Biggs DarkLighter: You cannot attack target ship");
                }
                result = false;
            }
        }

        private void RemoveBiggsDarklighterAbility(GenericShip ship, bool isFled)
        {
            RemoveBiggsDarklighterAbility();
        }

        private void RemoveBiggsDarklighterAbility(object sender, System.EventArgs e)
        {
            RemoveBiggsDarklighterAbility();
        }

        private void RemoveBiggsDarklighterAbility()
        {
            HostShip.Tokens.RemoveCondition(typeof(BiggsDarklighterCondition));

            GenericShip.OnTryPerformAttackGlobal -= CanPerformAttack;

            HostShip.OnShipIsDestroyed -= RemoveBiggsDarklighterAbility;
            Phases.Events.OnCombatPhaseEnd_NoTriggers -= RemoveBiggsDarklighterAbility;

            Phases.Events.OnCombatPhaseStart_Triggers -= RegisterAskBiggsAbility;
        }
    }
}

namespace Conditions
{
    public class BiggsDarklighterCondition : GenericToken
    {
        public BiggsDarklighterCondition(GenericShip host) : base(host)
        {
            Name = ImageName = "Buff Token";
            Temporary = false;
            Tooltip = new Ship.FirstEdition.XWing.BiggsDarklighter().ImageUrl;
        }
    }
}
