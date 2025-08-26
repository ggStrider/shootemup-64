using Definitions.Scenes.Cards;
using UI.Images;
using UnityEngine.SceneManagement;

namespace Internal.Core.Scenes
{
    public class SceneLoader
    {
        public void LoadScene(SceneCard sceneCard)
        {
            SceneManager.LoadScene(sceneCard.SceneFileName);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(GetActiveSceneName());
        }

        public string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        public void LoadSceneWithTransition(SceneCard sceneCard)
        {
            TransitionImageMover.Instance.MoveTo(TransitionImageMover.MoveToTypes.OverlayScreen,
                () => LoadScene(sceneCard));
        }

        public void ReloadSceneWithTransition()
        {
            TransitionImageMover.Instance.MoveTo(TransitionImageMover.MoveToTypes.OverlayScreen,
                ReloadScene);
        }
    }
}