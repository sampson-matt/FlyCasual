using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class ColicoidInterceptor : DroidTriFighter
    {
        public ColicoidInterceptor()
        {
            PilotInfo = new PilotCardInfo(
                "Colicoid Interceptor",
                1,
                34
            );
        }
    }
}