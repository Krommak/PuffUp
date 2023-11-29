#pragma warning disable 0693

using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Extentions;
using Game.Signals;

namespace Game.Systems
{
    public static class TriggerListenerSystem
    {
        private static Dictionary<Type, List<object>> _listenersByType;

        public static void AddListener<T>(IListener<T> listener) where T : ISignal
        {
            _listenersByType ??= new Dictionary<Type, List<object>>();

            var type = typeof(T);
            if (_listenersByType.ContainsKey(type))
            {
                if (!_listenersByType[type].Contains(listener))
                    _listenersByType[type].Add(listener);
                else
                    Debug.LogWarning($"Listeners {type} contains in dictionary");
            }
            else
            {
                _listenersByType.Add(type, new List<object>() { listener });
            }
        }

        public static void Trigger<T>() where T : struct, ISignal
        {
            var type = typeof(T);
            if (_listenersByType.ContainsKey(type))
            {
                _listenersByType[type].ForEach(x =>
                {
                    var listener = x as IListener<T>;
                    listener.Trigger<T>();
            });
                return;
            }

            Debug.LogWarning($"Key {type} not contains in listeners dictionary");
        }

        public static void RemoveListener<T>(IListener<T> listener) where T : ISignal
        {
            foreach (var item in _listenersByType.Values)
            {
                if (item.Contains(listener))
                    item.RemoveBySwap(item.IndexOf(listener));
            }
        }

        public static void ClearAllListeners()
        {
            _listenersByType.Clear();
        }
    }
    
    public interface IListener<T> where T : ISignal
    {
        void Trigger<T>();
    }
}