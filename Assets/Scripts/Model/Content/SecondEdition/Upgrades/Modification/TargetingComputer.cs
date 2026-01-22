using Actions;
using ActionsList;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class TargetingComputer : GenericUpgrade
    {
        public TargetingComputer() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Targeting Computer",
                UpgradeType.Modification,
                cost: 2,
                addAction: new ActionInfo(typeof(TargetLockAction))
            );
        }
    }
}