using Game.Data;
using Game.Signals;
using Game.Systems;
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
        public bool InStack = false;

        [SerializeField]
        private LayerMask _padlockLayers;

        [SerializeField]
        private Collider2D _collider2D;
        [SerializeField] 
        private ParticleSystem[] _particleComplete;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckContactsWithObstacles();
            CheckContactWithPadlock(collision.otherCollider);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckContactWithPadlock(other);
        }

        private void CheckContactWithPadlock(Collider2D other)
        {
            if (!InStack && IsComplete)
            {
                TriggerListenerSystem.Trigger(new ContactWithObject()
                {
                    ContactedMono = this,
                    OtherObjectID = other.gameObject.GetInstanceID(),
                    IsPadlock = (_padlockLayers.value & (1 << other.gameObject.layer)) == 0
                });
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            CheckContactsWithObstacles();
        }

        private void CheckContactsWithObstacles()
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