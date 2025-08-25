using UnityEngine;

namespace Internal.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static T GetRandomElement<T>(this T[] array)
        {
            if (array.Length == 0) return default;

            var randomIndex = Random.Range(0, array.Length);
            return array[randomIndex];
        }

        public static bool HasElements<T>(this T[] array)
        {
            return array != null && array.Length > 0;
        }
    }
}