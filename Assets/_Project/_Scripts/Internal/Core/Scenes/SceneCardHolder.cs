using Definitions.Scenes.Cards;
using UnityEngine;

namespace Internal.Core.Scenes
{
    public class SceneCardHolder : MonoBehaviour
    {
        [field: SerializeField] public SceneCard CurrentSceneCard { get; private set; }

        public void SetCurrentSceneCard(SceneCard sceneCard)
        {
            CurrentSceneCard = sceneCard;
        }
    }
}