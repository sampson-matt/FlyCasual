using Content;
using System.Collections.Generic;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class ColicoidInterceptor : DroidTriFighter
    {
        public ColicoidInterceptor()
        {
            PilotInfo = new PilotCardInfo(
                "Colicoid Interceptor",
                1,
                34,
                tags: new List<Tags>
                {
                    Tags.Droid
                }
            );
        }
    }
}