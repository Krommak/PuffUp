using Game.Data;
using Game.Signals;
using Game.Systems;
using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class BubbleMono : MonoBehaviour
    {
        public Renderer Renderer;
        public TMP_Text ScoreText;
        public int Score;
        public float Difference;
        public ParticleSystem[] Particle;
        public SpecialColor Color;
        public bool IsComplete = false;
        public bool OnPause = false;

        [SerializeField, Layer]
        private int _padlockLayer;

        [SerializeField]
        private Collider2D _collider2D;
        [SerializeField] 
        private ParticleSystem[] _particleComplete;
        public bool _inStack = false;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckContacts();

            if (!_inStack && IsComplete && collision.gameObject.layer == _padlockLayer)
            {
                TriggerListenerSystem.Trigger(new ConnectToStack()
                {
                    ObjectID = this.gameObject.GetInstanceID()
                });
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            CheckContacts();
        }

        private void CheckContacts()
        {
            if (IsComplete) return;

            Span<ContactPoint2D> numbers = stackalloc ContactPoint2D[5];

            var i = _collider2D.GetContacts(numbers.ToArray());

            if (i >= 3)
            {
                OnPause = true;
                return;
            }

            OnPause = false;
        }
    }
}