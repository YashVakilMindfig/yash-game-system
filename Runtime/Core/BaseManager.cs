using UnityEngine;
using System;

namespace Yash.GameSystem.Core
{
    public abstract class BaseManager : MonoBehaviour, IManager
    {
        // Automatically returns the specific type (e.g., AudioManager)
        public Type ServiceType => GetType();
        
        protected bool IsInitialized { get; private set; }

        public void Initialize()
        {
            if (IsInitialized) return;
            
            OnInitialize();
            IsInitialized = true;
            Debug.Log($"[YashSystem] {GetType().Name} Initialized.");
        }

        protected abstract void OnInitialize();

        // Independent Mode: Allows testing this prefab in an empty scene
        protected virtual void Start()
        {
            if (!IsInitialized)
            {
                Debug.LogWarning($"[YashSystem] {GetType().Name} running in INDEPENDENT MODE.");
                Initialize();
            }
        }
    }
}