using Definitions;
using Internal.Gameplay.UI.Buttons;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Internal.Gameplay.UI.GameItems
{
    public class UShopInitializator : MonoBehaviour
    {
        [SerializeField] private BuyableGameItem[] _shopItems;
        [SerializeField] private UButtonBuyGameItem _prefab;

        [Space] [SerializeField] private GridLayoutGroup _parent;

        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }
        
        private void Awake()
        {
            foreach (var item in _shopItems)
            {
                var instantiated = _container
                    .InstantiatePrefabForComponent<UButtonBuyGameItem>(_prefab, _parent.transform);
                
                instantiated.SetToBuyAndInitialize(item);
            }
        }
    }
}