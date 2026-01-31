using UnityEngine;
using Yash.GameSystem.Core;

namespace Yash.GameSystem.Utils
{
    public static class SystemBootstrapper
    {
        // Runs automatically before the first scene loads
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            // Safety check
            if (Object.FindAnyObjectByType<MainManager>() != null) return;

            // Load from Resources/GameSystem
            var systemPrefab = Resources.Load<GameObject>("GameSystem");
            
            if (systemPrefab == null) 
            {
                // Only log if you expect it to be there. 
                // Sometimes you might want to suppress this if testing unrelated things.
                return; 
            }

            var instance = Object.Instantiate(systemPrefab);
            instance.name = "--- YASH SYSTEM ---";
            Object.DontDestroyOnLoad(instance);
        }
    }
}