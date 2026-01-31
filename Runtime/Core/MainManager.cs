using UnityEngine;
using System;
using System.Collections.Generic;

namespace Yash.GameSystem.Core
{
    public class MainManager : MonoBehaviour
    {
        public static MainManager Instance { get; private set; }
        
        private readonly Dictionary<Type, IManager> _services = new Dictionary<Type, IManager>();

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(gameObject); 
                return; 
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeServices();
        }

        private void InitializeServices()
        {
            var managers = GetComponentsInChildren<IManager>();
            foreach (var manager in managers)
            {
                if (!_services.ContainsKey(manager.ServiceType))
                {
                    _services.Add(manager.ServiceType, manager);
                }
            }

            // Initialize all registered services
            foreach (var manager in _services.Values)
            {
                manager.Initialize();
            }
        }

        public T Get<T>() where T : class, IManager
        {
            if (_services.TryGetValue(typeof(T), out var service))
                return service as T;
                
            Debug.LogError($"[YashSystem] Manager '{typeof(T).Name}' not found! Check your Prefab.");
            return null;
        }
    }
}