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
        
        public void LoadScene(SceneCard sceneCard, bool setupMusicForMusicManager = false)
        {
            _musicManager.FadeCurrentClip();
            if(setupMusicForMusicManager) _musicManager.StartAndUnFadeClip(sceneCard.LevelClip);
            
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

        public void LoadSceneWithTransition(SceneCard sceneCard, bool setupMusicForMusicManager = false)
        {
            TransitionImageMover.Instance.MoveTo(TransitionImageMover.MoveToTypes.OverlayScreen,
                () => LoadScene(sceneCard, setupMusicForMusicManager));
        }

        public void ReloadSceneWithTransition()
        {
            TransitionImageMover.Instance.MoveTo(TransitionImageMover.MoveToTypes.OverlayScreen,
                ReloadScene);
        }
    }
}