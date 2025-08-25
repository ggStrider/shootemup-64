using UnityEngine;

namespace Tools
{
    public static class ToolsForEasyDebug
    {
        public static void Destroy(Object obj)
        {
            // Debug.Log($"Object destroyed: {obj.name}");
            Object.Destroy(obj);
        }
    }
}