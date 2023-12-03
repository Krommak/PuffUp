using DG.Tweening;
using Game.Signals;
using Game.Systems;
using System;
using TMPro;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class LevelPart : MonoStateListener, IListener<UpdatePadlockScore>
    {
        public int TargetValueForWin;
        public Renderer[] Obstacles;
        public ParticleSystem[] EndParticles;
        public int Turnes;
        public Padlock Padlock;
        public Rigidbody2D[] Rigidbodies;

        [SerializeField]
        private TMP_Text _padLockText;

        private int _currentValue;

        [OnState(GameState.LevelPartTransitionEnd)]
        private void OnState()
        {
            _currentValue = TargetValueForWin;

            UpdateScoreText();

            foreach (var item in Rigidbodies)
            {
                item.simulated = true;
            }

            TriggerListenerSystem.AddListener<UpdatePadlockScore>(this);

            TriggerListenerSystem.Trigger(new LevelPartLoaded()
            {
                LevelPart = this
            });
        }

        void IListener<UpdatePadlockScore>.Trigger(UpdatePadlockScore signal)
        {
            _currentValue -= signal.AddedScore;

            UpdateScoreText();

            if (_currentValue <= 0)
            {
                TriggerListenerSystem.Trigger(new LevelPartIsCancelled()
                {
                    LevelPart = this
                });
            }
        }

        private void UpdateScoreText()
        {
            var sequence = DOTween.Sequence(this);

            var startValue = int.Parse(_padLockText.text);
            var duration = MathF.Abs(_currentValue - startValue) / 5;
            var tweener = DOTween.To(() =>
                _padLockText.text,
                x => _padLockText.text =
                ((int)Mathf.MoveTowards(int.Parse(_padLockText.text), _currentValue, duration)).ToString(),
                _currentValue.ToString(), duration);

            sequence.Join(tweener);
            sequence.Play();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            TriggerListenerSystem.RemoveListener<UpdatePadlockScore>(this);
        }
    }

    [Serializable]
    public class Padlock
    {
        public Transform Lock;
        public Rigidbody2D Rigidbody;
        public Rigidbody2D UpRigidbody;
        public Joint2D[] Connections;
    }
}