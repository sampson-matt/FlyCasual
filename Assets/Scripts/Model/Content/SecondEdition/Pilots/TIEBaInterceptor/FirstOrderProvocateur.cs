using System;
using System.Linq;
using BoardTools;
using Ship;
using SubPhases;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEBaInterceptor
    {
        public class FirstOrderProvocateur : TIEBaInterceptor
        {
            public FirstOrderProvocateur() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "First Order Provocateur",
                    3,
                    41
                );
            }
        }
    }
}
