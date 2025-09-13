using UnityEngine;

namespace Internal.Core.Signals
{
    public class BackgroundChangedSignal
    {
        public SpriteRenderer Background;
        public Color NewColor;
        public Color OppositeColor;
        public bool CompletelyChanged;

        public BackgroundChangedSignal(SpriteRenderer background, Color newColor, Color oppositeColor,
            bool completelyChanged)
        {
            Background = background;
            NewColor = newColor;
            OppositeColor = oppositeColor;
            CompletelyChanged = completelyChanged;
        }
    }
}