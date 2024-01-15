using Game.Signals;
using Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public abstract class MonoStateListener : MonoBehaviour, IListener<ChangeGameState>
    {
        private Dictionary<GameState, List<Action>> _onStateActions;

        protected virtual void Awake()
        {
            TriggerListenerSystem.AddListener(this);

            _onStateActions = new Dictionary<GameState, List<Action>>();

            var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var item in methods)
            {
                object[] attributes = item.GetCustomAttributes(typeof(OnState), false);

                foreach (var attribute in attributes)
                {
                    if (attribute is OnState onState)
                    {
                        if (_onStateActions.ContainsKey(onState.TargetState))
                        {
                            _onStateActions[onState.TargetState].Add(() => item.Invoke(this, null));
                        }
                        else
                        {
                            _onStateActions.Add(onState.TargetState, new List<Action>() { () => item.Invoke(this, null) });
                        }
                    }
                }
            }
        }

        protected virtual void OnDisable()
        {
            TriggerListenerSystem.RemoveListener(this);
        }

        void IListener<ChangeGameState>.Trigger(ChangeGameState signal)
        {
            if (_onStateActions.ContainsKey(signal.GameState))
            {
                foreach (var item in _onStateActions[signal.GameState].ToList())
                {
                    item.Invoke();
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class OnState : Attribute
    {
        public GameState TargetState;

        public OnState(GameState targetState)
        {
            TargetState = targetState;
        }
    }
}