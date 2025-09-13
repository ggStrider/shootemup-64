using Definitions.Scenes.CameraBassShake;
using Internal.Core.Extensions;
using NaughtyAttributes;
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
        [Header("Color (Any1) - color of bg")] [Header("Bool (Any2) - smooth changing of bg")] 
        [SerializeField] private DelaysWave _copySource;

        [Button]
        private void CopyTimings()
        {
            var targetCount = _copySource.Count;
            if (DelaysWith2T.Count > targetCount)
            {
                DelaysWith2T.RemoveRange(targetCount, DelaysWith2T.Count - targetCount);
            }
            else if (DelaysWith2T.Count < targetCount)
            {
                for (int i = DelaysWith2T.Count; i < targetCount; i++)
                {
                    DelaysWith2T.Add(new DelayWith2T(0f));
                }
            }

            for (var i = 0; i < _copySource.Delays.Count; i++)
            {
                var delay = _copySource.Delays[i];
                DelaysWith2T[i].SetDelay(delay);
            }
        }
    }
}