using UnityEngine;

namespace Definitions.Scenes.Cards
{
    [CreateAssetMenu(fileName = "New Scene Card",
        menuName = StaticKeys.PROJECT_NAME + "/Definitions/Scenes/Scene Card")]
    public class SceneCard : ScriptableObject
    {
        [field: SerializeField] public string SceneFileName { get; private set; } = "theFish";
        [field: SerializeField] public string ScenePreviewName { get; private set; } = "The Fish";
    }
}