using Definitions.Scenes.Cards;
using UnityEngine.SceneManagement;

namespace Internal.Core.Scenes
{
    public class SceneLoader
    {
        public void LoadScene(SceneCard sceneCard)
        {
            SceneManager.LoadScene(sceneCard.SceneFileName);
        }
    }
}