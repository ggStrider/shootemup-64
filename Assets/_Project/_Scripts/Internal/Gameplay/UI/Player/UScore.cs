using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.UI.Player
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
            _gameTimer.CurrentTimeReactive.OnValueChanged += UpdateScoreText;
        }

        private void UpdateScoreText(float lastValue, float newValue)
        {
            _scoreText.text = newValue.ToString("F1");
        }
    }
}