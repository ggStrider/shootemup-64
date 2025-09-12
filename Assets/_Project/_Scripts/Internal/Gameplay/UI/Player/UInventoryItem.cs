using System;
using Definitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Gameplay.UI.Player
{
    [Serializable]
    public class UInventoryItem : MonoBehaviour
    {
        [field: SerializeField] public Image SpritePlaceholder { get; private set; }
        [field: SerializeField] public TextMeshProUGUI AmountLabel { get; private set; }
        [field: SerializeField] public GameItem GameItem { get; private set; }

        public void SetAmount(int amount)
        {
            AmountLabel.text = "x" + amount;
        }

        public void SetSpriteFromGameItem()
        {
            SpritePlaceholder.sprite = GameItem.ItemIcon;
            SpritePlaceholder.color = GameItem.IconColor;
        }
    }
}