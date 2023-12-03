using Game.Data;
using Game.Signals;
using Game.Systems;
using System;
using TMPro;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class BubbleMono : MonoStateListener
    {
        public Renderer Renderer;
        public TMP_Text ScoreText;
        public int Score;
        public float Difference;
        public ParticleSystem[] Particle;
        public ParticleSystem[] ParticleComplete;
        public SpecialColor Color;
        public bool OnPause = false;
        public bool InStack = false;

        [SerializeField]
        private LayerMask _padlockLayers;
        [SerializeField]
        private Collider2D _collider2D;
        private bool _isComplete { get; set; } = false;
        private Action _onCompleteAction;

        [OnState(GameState.LevelPartTransitionStart)]
        private void OnState()
        {
            Destroy(gameObject);
        }

        public void SetComplete()
        {
            _isComplete = true;
            _onCompleteAction?.Invoke();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckContactsWithObstacles();
            CheckContactWithPadlock(collision.otherCollider);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            CheckContactsWithObstacles();
        }

        private void CheckContactWithPadlock(Collider2D other)
        {
            if (!InStack && _isComplete)
            {
                Contact(other);
            }
            else if(!_isComplete)
            {
                _onCompleteAction += () => Contact(other);
            }
        }

        private void Contact(Collider2D other)
        {
            TriggerListenerSystem.Trigger(new ContactWithObject()
            {
                ContactedMono = this,
                OtherObjectID = other.gameObject.GetInstanceID(),
                IsPadlock = (_padlockLayers.value & (1 << other.gameObject.layer)) == 0
            });
        }

        private void CheckContactsWithObstacles()
        {
            if (_isComplete) return;

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