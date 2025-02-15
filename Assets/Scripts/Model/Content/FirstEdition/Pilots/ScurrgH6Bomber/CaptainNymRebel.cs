﻿using Bombs;
using Ship;
using SubPhases;
using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.ScurrgH6Bomber
    {
        public class CaptainNymRebel : ScurrgH6Bomber
        {
            public CaptainNymRebel() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Captain Nym",
                    8,
                    30,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.CaptainNymRebelAbiliity),
                    extraUpgradeIcon: UpgradeType.Talent,
                    factionOverride: Faction.Rebel
                );

                ModelInfo.SkinName = "Captain Nym (Rebel)";
            }
        }
    }
}

namespace Abilities.FirstEdition
{
    public class CaptainNymRebelAbiliity : GenericAbility
    {
        public override void ActivateAbility()
        {
            BombsManager.OnCheckPermissionToDetonate += CheckCaptainNymAbility;
            Phases.Events.OnRoundEnd += ClearIsAbilityUsedFlag;
        }

        public override void DeactivateAbility()
        {
            BombsManager.OnCheckPermissionToDetonate -= CheckCaptainNymAbility;
            Phases.Events.OnRoundEnd -= ClearIsAbilityUsedFlag;
        }

        private void CheckCaptainNymAbility(GenericBomb bomb, GenericShip detonatedShip)
        {
            if (CanUseAbility() && Tools.IsFriendly(bomb.HostShip, HostShip))
            {
                RegisterAbilityTrigger(TriggerTypes.OnCheckPermissionToDetonate, AskToUseCaptainNymAbility);
            }
        }

        private void AskToUseCaptainNymAbility(object sender, System.EventArgs e)
        {
            if (CanUseAbility())
            {
                Messages.ShowInfoToHuman(string.Format("{0} token is ready for detonation", BombsManager.CurrentDevice.UpgradeInfo.Name));
                AskToUseAbility(
                    HostShip.PilotInfo.PilotName,
                    NeverUseByDefault,
                    UseAbility,
                    descriptionLong: "Do you want to prevent a friendly bomb from detonating?",
                    imageHolder: HostShip
                );
            }
            else
            {
                Messages.ShowErrorToHuman("Captain Nym has already used his ability");
                Triggers.FinishTrigger();
            }
        }

        private void UseAbility(object sender, System.EventArgs e)
        {
            BombsManager.DetonationIsAllowed = false;
            MarkAbilityAsUsed();

            DecisionSubPhase.ConfirmDecision();
        }

        protected virtual bool CanUseAbility()
        {
            return !IsAbilityUsed;
        }

        protected virtual void MarkAbilityAsUsed()
        {
            IsAbilityUsed = true;
        }
    }
}