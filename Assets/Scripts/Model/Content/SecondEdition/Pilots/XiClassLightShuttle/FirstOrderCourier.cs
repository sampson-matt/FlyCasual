namespace Ship
{
    namespace SecondEdition.XiClassLightShuttle
    {
        public class FirstOrderCourier : XiClassLightShuttle
        {
            public FirstOrderCourier() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "First Order Courier",
                    2,
                    32
                );
            }
        }
    }
}

