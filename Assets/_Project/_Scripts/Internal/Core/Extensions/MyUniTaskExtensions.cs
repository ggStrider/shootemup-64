using System.Threading;
using UnityEngine;

namespace Tools
{
    public static class MyUniTaskExtensions
    {
        public static void SafeCancelAndCleanToken(ref CancellationTokenSource cts,
            bool createNewTokenAfter = false)
        {
            cts?.Cancel();
            cts?.Dispose();

            cts = null;
            if (createNewTokenAfter) cts = new();
        }

        public static bool IsExistAndNotCanceled(this CancellationTokenSource cts)
        {
            return cts != null && !cts.IsCancellationRequested;
        }

        public static bool IsExistAndCanceled(this CancellationTokenSource cts)
        {
            return cts != null && cts.IsCancellationRequested;
        }

        public static bool IsSameToken(this CancellationTokenSource cts, CancellationToken toCompare)
        {
            return cts != null && cts.Token == toCompare;
        }
    }
}