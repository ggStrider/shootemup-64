using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class UButtonQuitGame : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;
        
        private void Awake()
        {
            _buttonToBind.onClick.AddListener(Application.Quit);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _buttonToBind ??= GetComponent<Button>();
        }
#endif
    }
}
