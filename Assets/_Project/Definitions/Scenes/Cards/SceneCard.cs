using Definitions.Scenes.CameraBassShake;
using Definitions.Scenes.Earnings;
using Definitions.Waves;
using UnityEngine;

namespace Definitions.Scenes.Cards
{
    [CreateAssetMenu(fileName = "New Scene Card",
        menuName = StaticKeys.PROJECT_NAME + "/Definitions/Scenes/Scene Card")]
    public class SceneCard : ScriptableObject
    {
        [field: SerializeField] public string SceneFileName { get; private set; } = "theFish";
        [field: SerializeField] public string ScenePreviewName { get; private set; } = "The Fish";
        
        [field: Space, Header("Spawners")]
        [field: SerializeField] public LevelWaves LevelWaves { get; private set; }

        [field: Space, Header("Earnings")] 
        [field: SerializeField] public EarningSettingsInSceneCard EarningSettingsInSceneCard;
        
        [field: Space, Header("Music")]
        [field: SerializeField] public AudioClip LevelClip { get; private set; }
        [field: SerializeField] public float Volume { get; private set; } = 0.8f;
        [field: SerializeField] public CameraBassShakeWaves CameraBassShakeWaves { get; private set; }
        
        [field: Space, Header("Other")]
        [field: SerializeField] public bool ShakeCameraWhenPlayerKillSomeone { get; private set; } = true;
    }
}