using Definitions.Scenes.CameraBassShake;
using UnityEngine;

namespace Definitions.Scenes.Delays.BackgroundChanger
{
    /// <summary>
    /// T1 - Color of bg; T2 - Smooth changing bg
    /// </summary>
    [CreateAssetMenu(fileName = "New Background Change Delays", menuName =
        StaticKeys.PROJECT_NAME + "/Definitions/Delays/Background Change Delays")]
    public class BackgroundChangeDelays : DelaysWaveWithClassTwoGeneric<Color, bool>
    {
        [Header("Color (Any1) - color of bg")] 
        [Header("Bool (Any2) - smooth changing of bg")] 
        
        // [SerializeField]
        // private ///// _dummyForHeader;
    }
}