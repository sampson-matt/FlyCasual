using BoardTools;
using Movement;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEPhPhantom
    {
        public class CaptainSaxton : TIEPhPhantom
        {
            public CaptainSaxton() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Captain Saxton",
                    3,
                    43,
                    charges: 2,
                    regensCharges: 1,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CaptainSaxtonAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                PilotNameCanonical = "captainsaxton-wat1";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CaptainSaxtonAbility : GenericAbility
    {
        GenericShip FriendlyShip;
        List<ManeuverTemplate> AddedTemplates;
        public override void ActivateAbility()
        {
            GenericShip.OnBeforeDecloakGlobal += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnBeforeDecloakGlobal -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if(HostShip.State.Charges >= 2 &&
                BoardTools.Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0,2), Team.Type.Friendly).Contains(ship))
            {
                RegisterAbilityTrigger(TriggerTypes.OnBeforeDecloak, delegate
                {
                    FriendlyShip = ship;
                    AskToUseAbility(
                        HostShip.PilotInfo.PilotName,
                        NeverUseByDefault,
                        useAbility,
                        descriptionLong: $"Do you want to spend 2 charges to allow {ship.PilotInfo.PilotName} to use a template of 1 speed higher or lower?",
                        imageHolder: HostShip
                    );
                });
            }
        }

        private void useAbility(object sender, EventArgs e)
        {
            HostShip.SpendCharges(2);
            FriendlyShip.OnGetAvailableDecloakTemplates += ChangeDecloakTemplates;
            DecisionSubPhase.ConfirmDecision();
        }

        private void ChangeDecloakTemplates(List<ManeuverTemplate> availableTemplates)
        {
            FriendlyShip.OnGetAvailableDecloakTemplates -= ChangeDecloakTemplates;
            AddedTemplates = new List<ManeuverTemplate>();
            foreach (ManeuverTemplate availableTemplate in availableTemplates)
            {
                ManeuverSpeed reducedSpeed = availableTemplate.Speed - 1;
                ManeuverTemplate reducedSpeedTemplate = new ManeuverTemplate(availableTemplate.Bearing, availableTemplate.Direction, reducedSpeed);
                if (reducedSpeedTemplate.IsValidTemplate()
                    && !availableTemplates.Any(t => t.Name == reducedSpeedTemplate.Name)
                    && !AddedTemplates.Any(t => t.Name == reducedSpeedTemplate.Name)
                    )
                {
                    AddedTemplates.Add(reducedSpeedTemplate);
                }

                AddedTemplates.Add(availableTemplate);

                ManeuverSpeed increasedSpeed = availableTemplate.Speed + 1;
                ManeuverTemplate increasedSpeedTemplate = new ManeuverTemplate(availableTemplate.Bearing, availableTemplate.Direction, increasedSpeed);
                if (increasedSpeedTemplate.IsValidTemplate()
                    && !availableTemplates.Any(t => t.Name == increasedSpeedTemplate.Name)
                    && !AddedTemplates.Any(t => t.Name == increasedSpeedTemplate.Name)
                    )
                {
                    AddedTemplates.Add(increasedSpeedTemplate);
                }
            }
            availableTemplates.Clear();
            availableTemplates.AddRange(AddedTemplates);
        }
    }
}
