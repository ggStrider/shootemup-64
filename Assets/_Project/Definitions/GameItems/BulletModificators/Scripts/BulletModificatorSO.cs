using Internal.Core.DataModel;
using Shoot;

namespace Definitions.BulletModificators.Scripts
{
    public abstract class BulletModificatorSO : BuyableGameItem
    {
        public abstract void ApplyModificator(BulletBehaviour bullet);

        public override bool TryBuyItem(PlayerData playerData, bool autoDecreaseAmountOfPrice = true)
        {
            if (playerData.Coins < Price) return false;
            
            playerData.AddBulletModificatorInInventory(this);
            if (autoDecreaseAmountOfPrice) playerData.SubtractCoins(Price);
            
            return true;
        }
    }
}
