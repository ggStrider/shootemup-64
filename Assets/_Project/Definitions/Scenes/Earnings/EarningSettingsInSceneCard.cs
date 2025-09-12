using UnityEngine;

namespace Definitions.Scenes.Earnings
{
    [CreateAssetMenu(fileName = "New Earnings Settings",
        menuName = StaticKeys.PROJECT_NAME + "/Definitions/Scenes/Earning Settings")]
    public class EarningSettingsInSceneCard : ScriptableObject
    {
        [field: Header("Enemy")]
        [field: SerializeField, Min(0)] public int AddCoinsOnRealEnemyKilled = 2;
        [field: SerializeField, Min(0)] public int SubtractCoinsOnRealEnemyHitInPlayer = 1;
        
        [field: Space, Header("Fake Enemy")]
        [field: SerializeField, Min(0)] public int SubtractCoinsOnFakeEnemyKilled = 2;
        [field: SerializeField, Min(0)] public int AddCoinsOnFakeEnemyHitInPlayer = 1;
    }
}