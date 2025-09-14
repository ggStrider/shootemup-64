using Internal.Core.DataModel;
using UnityEngine;

namespace Definitions
{
    public abstract class BuyableGameItem : GameItem
    {
        [field: Space]
        [field: SerializeField, Min(0)] public int Price { get; private set; } = 10;

        public abstract bool TryBuyItem(PlayerData playerData, bool autoDecreaseAmountOfPrice = true);
    }
}