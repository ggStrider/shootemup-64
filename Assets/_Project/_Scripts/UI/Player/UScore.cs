using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Player
{
    public class UScore : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        private GameTimer _gameTimer;

        [Inject]
        private void Construct(GameTimer gameTimer)
        {
            _gameTimer = gameTimer;
        }

        private void Start()
        {
            _gameTimer.OnCurrentTimeChanged += UpdateScoreText;
        }

        private void UpdateScoreText(float currentTime)
        {
            _scoreText.text = currentTime.ToString("F1");
        }
    }
}