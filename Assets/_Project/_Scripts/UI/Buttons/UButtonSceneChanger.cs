using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class UButtonSceneChanger : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;
        
        [Space] [SerializeField] private string _sceneNameToLoad;

        private void Awake()
        {
            _buttonToBind.onClick.AddListener(() => {
                SceneManager.LoadScene(_sceneNameToLoad);
            });
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _buttonToBind ??= GetComponent<Button>();
        }
#endif
    }
}
