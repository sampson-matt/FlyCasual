using Content;
using System;
using System.Collections.Generic;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class OOMUplinkPrototype : VultureClassDroidFighter
    {
        public OOMUplinkPrototype()
        {
            PilotInfo = new PilotCardInfo(
                "OOM Uplink Prototype",
                2,
                23,
                true,
                abilityType: typeof(Abilities.SecondEdition.OOMUplinkPrototypeAbility),
                pilotTitle: "Command Relay",
                tags: new List<Tags>
                {
                    Tags.Droid
                }
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/X2PO-homebrewPilot-wat00muplinkv33.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    //For the purposes of the Networked Calculations ship ability, all friendly ships at range 0-1 of you are treated as being at range 0-1 of each other
    //Note - the affects of this ability are coded under the Networked Calculations ability itself in the VultureClassDroodFighter.cs class
    public class OOMUplinkPrototypeAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }

       
    }
}
