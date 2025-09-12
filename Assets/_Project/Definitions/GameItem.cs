using UnityEngine;

namespace Definitions
{
    public class GameItem : ScriptableObject
    {
        [field: TextArea(2, 4)]
        [field: SerializeField] public string ItemName { get; private set; } = "A fish...";
        [field: SerializeField] public Sprite ItemIcon { get; private set; }
        [field: SerializeField] public Color IconColor { get; private set; } = Color.white;
    }
}