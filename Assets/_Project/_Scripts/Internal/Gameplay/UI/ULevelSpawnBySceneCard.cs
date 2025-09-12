using Definitions.Scenes.Cards;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Internal.Gameplay.UI
{
    public class ULevelSpawnBySceneCard : MonoBehaviour
    {
        [SerializeField] private SceneCard[] _sceneCards;
        [SerializeField] private UButtonSceneChangerWithTransition _prefab;

        [SerializeField] private GridLayoutGroup _parent;

        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        private void Awake()
        {
            foreach (var sceneCard in _sceneCards)
            {
                var instantiated = _container.InstantiatePrefabForComponent<UButtonSceneChangerWithTransition>(
                    _prefab, _parent.transform);
                
                instantiated.SetSceneCardAndInitialize(sceneCard);
            }
        }
    }
}
