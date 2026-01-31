using System;
using System.Collections.Generic;

namespace Yash.GameSystem.Utils
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public static void Subscribe<T>(Action<T> callback)
        {
            Type type = typeof(T);
            if (!_subscribers.ContainsKey(type)) _subscribers[type] = new List<Delegate>();
            _subscribers[type].Add(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            Type type = typeof(T);
            if (_subscribers.ContainsKey(type)) _subscribers[type].Remove(callback);
        }

        public static void Raise<T>(T eventData)
        {
            Type type = typeof(T);
            if (_subscribers.TryGetValue(type, out var callbacks))
            {
                // Iterate backwards to safely handle unsubscriptions during execution
                for (int i = callbacks.Count - 1; i >= 0; i--)
                {
                    (callbacks[i] as Action<T>)?.Invoke(eventData);
                }
            }
        }
    }
}