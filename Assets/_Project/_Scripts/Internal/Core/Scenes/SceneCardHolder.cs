using Definitions.Scenes.Cards;
using UnityEngine;

namespace Internal.Core.Scenes
{
    public class SceneCardHolder : MonoBehaviour
    {
        [SerializeField] private SceneCard _startSceneCard;
        [field: SerializeField] public SceneCard CurrentSceneCard { get; private set; }

        private void Awake()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            {
                CurrentSceneCard = _startSceneCard;
            }
        }

        public void SetCurrentSceneCard(SceneCard sceneCard)
        {
            CurrentSceneCard = sceneCard;
        }
    }
}