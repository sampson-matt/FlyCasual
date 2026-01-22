using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class B6BladeWingPrototype : GenericUpgrade
    {
        public B6BladeWingPrototype() : base()
        {
            UpgradeInfo = new UpgradeCardInfo
            (
                "B6 Blade Wing Prototype",
                UpgradeType.Title,
                cost: 2,
                isLimited: true,
                addSlot: new UpgradeSlot(UpgradeType.Gunner),
                restrictions: new UpgradeCardRestrictions
                (
                    new FactionRestriction(Faction.Rebel),
                    new ShipRestriction(typeof(Ship.SecondEdition.ASF01BWing.ASF01BWing))
                )
            );

            NameCanonical = "b6bladewingprototype1";
        }        
    }
}