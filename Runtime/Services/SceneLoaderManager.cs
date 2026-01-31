using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Yash.GameSystem.Core;
using Yash.GameSystem.Utils;

namespace Yash.GameSystem.Services
{
    // --- Events ---
    public struct SceneLoadStartEvent { public string SceneName; }
    public struct SceneProgressEvent { public float Progress; }
    public struct SceneLoadCompleteEvent { public string SceneName; }

    // --- The Manager ---
    public class SceneLoaderManager : BaseManager
    {
        public bool IsLoading { get; private set; }

        protected override void OnInitialize() 
        {
            // Optional: Subscribe to Unity's native event if needed
        }

        public void LoadScene(string sceneName)
        {
            if (IsLoading)
            {
                Debug.LogWarning("[YashSystem] Already loading a scene.");
                return;
            }
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            IsLoading = true;
            EventBus.Raise(new SceneLoadStartEvent { SceneName = sceneName });

            yield return new WaitForSeconds(0.2f); // Small delay for UI transitions

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                // Unity's progress stops at 0.9
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                EventBus.Raise(new SceneProgressEvent { Progress = progress });

                // Check if ready to activate
                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }

            IsLoading = false;
            EventBus.Raise(new SceneLoadCompleteEvent { SceneName = sceneName });
        }
    }
}