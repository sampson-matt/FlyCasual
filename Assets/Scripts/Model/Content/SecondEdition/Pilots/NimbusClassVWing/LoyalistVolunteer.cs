using System;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.NimbusClassVWing
    {
        public class LoyalistVolunteer : NimbusClassVWing
        {
            public LoyalistVolunteer() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Loyalist Volunteer",
                    2,
                    26
                );
            }
        }
    }
}