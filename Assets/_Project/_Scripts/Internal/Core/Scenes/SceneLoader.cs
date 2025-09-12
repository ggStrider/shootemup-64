using Audio;
using Definitions.Scenes.Cards;
using UI.Images;
using UnityEngine.SceneManagement;
using Zenject;

namespace Internal.Core.Scenes
{
    public class SceneLoader
    {
        private SceneCardHolder _sceneCardHolder;
        private MusicManager _musicManager;
        
        [Inject]
        private void Construct(SceneCardHolder sceneCardHolder, MusicManager musicManager)
        {
            _sceneCardHolder = sceneCardHolder;
            _musicManager = musicManager;
        }
        
        public void LoadScene(SceneCard sceneCard)
        {
            _musicManager.FadeCurrentClip();
            _sceneCardHolder.SetCurrentSceneCard(sceneCard);
            SceneManager.LoadScene(sceneCard.SceneFileName);
        }

        public void ReloadScene()
        {
            _musicManager.FadeCurrentClip();
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