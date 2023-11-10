using Upgrade;
using System;
using Tokens;
using Ship;
using System.Collections.Generic;
using ActionsList;
using SubPhases;
using System.Linq;

namespace UpgradesList.SecondEdition
{
    public class ExpertHandlingHotac : GenericUpgrade
    {
        public ExpertHandlingHotac() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Expert Handling",
                UpgradeType.Talent,
                cost: 4,
                abilityType: typeof(Abilities.SecondEdition.ExpertHandlingHotacAbility)
            );
            NameCanonical = "experthandlinghotac";
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/talent/experthandling.png";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class ExpertHandlingHotacAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckAbility;
        }

        private void CheckAbility(GenericAction action)
        {
            if (action is BarrelRollAction && HostShip.Tokens.HasToken(typeof(RedTargetLockToken), '*'))
            {
                RegisterAbilityTrigger(TriggerTypes.OnActionDecisionSubPhaseEnd, RemoveTargetLock);

            }
        }

        private void RemoveTargetLock(object sender, System.EventArgs e)
        {
            List<char> tokens = new List<char>();
            foreach (var token in Selection.ThisShip.Tokens.GetAllTokens())
            {
                if (token.GetType() == typeof(RedTargetLockToken))
                {
                    tokens.Add((token as RedTargetLockToken).Letter);
                }
            }
            if (tokens.Count > 0)
            {
                Messages.ShowInfo("Expert Handling allows " + HostShip.PilotName + "to remove one Target Lock when performing a barrel roll.");
                HostShip.Tokens.RemoveToken(
                typeof(RedTargetLockToken),
                CleanUp,
                tokens[0]
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void CleanUp()
        {
            Triggers.FinishTrigger();
        }
    }
}