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
        private static Dictionary<Type, List<object>> _listenersBySignal;
        private static Dictionary<Type, List<object>> _listenersWithDataBySignal;

        public static void AddListener<T>(IListener<T> listener) where T : ISignal
        {
            _listenersBySignal ??= new Dictionary<Type, List<object>>();
            _listenersWithDataBySignal ??= new Dictionary<Type, List<object>>();

            var type = typeof(T);

            if (_listenersBySignal.ContainsKey(type))
            {
                if (!_listenersBySignal[type].Contains(listener))
                    _listenersBySignal[type].Add(listener);
                else
                    Debug.LogWarning($"Listeners {type} contains in dictionary");
            }
            else
            {
                _listenersBySignal.Add(type, new List<object>() { listener });
            }
        }

        public static void Trigger<T>(T signal) where T : struct, ISignal
        {
            var type = typeof(T);
            if (_listenersBySignal.ContainsKey(type))
            {
                _listenersBySignal[type].ForEach(x =>
                {
                    var listener = x as IListener<T>;
                    listener.Trigger(signal);
            });
                return;
            }

            Debug.LogWarning($"Key {type} not contains in listeners dictionary");
        }

        public static void RemoveListener<T>(IListener<T> listener) where T : ISignal
        {
            foreach (var item in _listenersBySignal.Values)
            {
                if (item.Contains(listener))
                    item.RemoveBySwap(item.IndexOf(listener));
            }
        }

        public static void ClearAllListeners()
        {
            _listenersBySignal.Clear();
            _listenersWithDataBySignal.Clear();
        }
    }
    
    public interface IListener<T> where T : ISignal
    {
        void Trigger(T signal);
    }
}