using UnityEngine;

using Definitions.Scenes.CameraBassShake;
using Definitions.Scenes.Delays.BackgroundChanger;
using Definitions.Scenes.Earnings;
using Definitions.Waves;
using NaughtyAttributes;

namespace Definitions.Scenes.Cards
{
    [CreateAssetMenu(fileName = "New Scene Card",
        menuName = StaticKeys.PROJECT_NAME + "/Definitions/Scenes/Scene Card")]
    public class SceneCard : ScriptableObject
    {
        [field: SerializeField] public string SceneFileName { get; private set; } = "theFish";
        [field: SerializeField] public string ScenePreviewName { get; private set; } = "The Fish";
        
        [field: Space, Header("With Delays")]
        [field: SerializeField] public LevelWaves LevelWaves { get; private set; }
        [field: SerializeField] public DelaysWave DelaysWave { get; private set; }

        [field: SerializeField] public bool UseRandomBackgroundColor { get; private set; }= false;
        [field: SerializeField] public bool UseLevelWaveAsDelay { get; private set; } = false;
        
        [field: HideIf(nameof(UseLevelWaveAsDelay))]
        [field: SerializeField] public BackgroundChangeDelays BackgroundChangeDelays { get; private set; }

        [field: Space, Header("Earnings")] 
        [field: SerializeField] public EarningSettingsInSceneCard EarningSettingsInSceneCard;
        
        [field: Space, Header("Music")]
        [field: SerializeField] public AudioClip LevelClip { get; private set; }
        [field: SerializeField] public float Volume { get; private set; } = 0.8f;
        
        [field: Space, Header("Other")]
        [field: SerializeField] public bool ShakeCameraWhenPlayerKillSomeone { get; private set; } = true;
    }
}