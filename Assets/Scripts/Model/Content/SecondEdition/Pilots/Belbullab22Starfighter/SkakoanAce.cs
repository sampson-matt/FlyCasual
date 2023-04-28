namespace Ship.SecondEdition.Belbullab22Starfighter
{
    public class SkakoanAce : Belbullab22Starfighter
    {
        public SkakoanAce()
        {
            PilotInfo = new PilotCardInfo(
                "Skakoan Ace",
                3,
                38,
                extraUpgradeIcon: Upgrade.UpgradeType.Talent
            );
        }
    }
}