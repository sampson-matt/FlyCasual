namespace Ship.SecondEdition.Belbullab22Starfighter
{
    public class SkakoanAce : Belbullab22Starfighter
    {
        public SkakoanAce()
        {
            PilotInfo = new PilotCardInfo(
                "Skakoan Ace",
                3,
                37,
                extraUpgradeIcon: Upgrade.UpgradeType.Talent
            );
        }
    }
}