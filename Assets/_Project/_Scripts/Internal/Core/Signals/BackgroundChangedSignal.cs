using UnityEngine;

namespace Internal.Core.Signals
{
    public class BackgroundChangedSignal
    {
        public SpriteRenderer Background;
        public Color NewColor;
        public Color OppositeColor;

        public BackgroundChangedSignal(SpriteRenderer background, Color newColor, Color oppositeColor)
        {
            Background = background;
            NewColor = newColor;
            OppositeColor = oppositeColor;
        }
    }
}