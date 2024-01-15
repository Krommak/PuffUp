using DG.Tweening;
using Game.MonoBehaviours;
using Game.Signals;
using System;
using UnityEngine;
using Zenject;

namespace Game.Systems
{
    public class WinSystem :
        IInitializable,
        IListener<LevelPartIsCancelled>, ILateDisposable
    {
        public void Initialize()
        {
            TriggerListenerSystem.AddListener<LevelPartIsCancelled>(this);
        }

        public void LateDispose()
        {
            TriggerListenerSystem.RemoveListener<LevelPartIsCancelled>(this);
        }

        void IListener<LevelPartIsCancelled>.Trigger(LevelPartIsCancelled signal)
        {
            NextPartEffects(signal.LevelPart, () =>
            {
                TriggerListenerSystem.Trigger(new NextLevelPart());
            });
        }

        private void NextPartEffects(LevelPart levelPart, Action endAction)
        {
            var padlock = levelPart.Padlock;

            foreach (var item in levelPart.EndParticles)
            {
                item.transform.SetParent(null);
                item.Play();
            }

            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    foreach(var item in padlock.Connections)
                    {
                        item.enabled = false;
                    }
                    foreach (var item in levelPart.Rigidbodies)
                    {
                        item.gravityScale = -0.5f;
                    }

                    padlock.Lock.GetComponent<Collider2D>().enabled = false;
                    padlock.Lock.GetComponent<FixedJoint2D>().enabled = false;

                    var pos = padlock.Lock.localPosition;
                    pos.z = -1.5f;
                    padlock.Lock.localPosition = pos;

                    padlock.Lock.parent = null;

                    padlock.Rigidbody.AddForce(Vector2.up * 2.2f, ForceMode2D.Impulse);
                    padlock.Rigidbody.AddTorque(UnityEngine.Random.Range(0, 2) == 0 ? -180 : 180);
                })
                .AppendInterval(2f)
                .AppendCallback(() =>
                {
                    GameObject.Destroy(padlock.Lock.gameObject);
                    endAction?.Invoke();
                })
                .Play();
        }
    }
}
