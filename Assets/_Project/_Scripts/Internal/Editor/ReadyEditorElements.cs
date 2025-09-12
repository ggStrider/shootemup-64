using System;
using UnityEngine;

namespace Internal.Editor
{
    public static class ReadyEditorElements
    {
        public static void DrawRichButton(string buttonText, Color buttonColor, Action onPressed,
            int fontSize = 13, float buttonHeight = 43f)
        {
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = buttonColor;

            var style = new GUIStyle(GUI.skin.button)
            {
                fontSize = fontSize,
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = buttonHeight,
                richText = true
            };

            if (GUILayout.Button(buttonText, style))
            {
                onPressed?.Invoke();
            }

            GUI.backgroundColor = oldColor;
        }
        
        public static void DrawRichLabel(string labelText, Color textColor, int fontSize = 13, 
            FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = fontSize,
                fontStyle = fontStyle,
                normal = { textColor = textColor },
                alignment = alignment
            };

            GUILayout.Label(labelText, style);
        }
    }
}