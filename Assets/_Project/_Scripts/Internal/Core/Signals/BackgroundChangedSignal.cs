using UnityEngine;

namespace Internal.Core.Signals
{
    public class BackgroundChangedSignal
    {
        public SpriteRenderer Background;
        public Color NewColor;

        public BackgroundChangedSignal(SpriteRenderer background, Color newColor)
        {
            Background = background;
            NewColor = newColor;
        }
    }
}