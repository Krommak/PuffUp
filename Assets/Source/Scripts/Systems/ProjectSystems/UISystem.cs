using DG.Tweening;
using Game.Data;
using Game.Extentions;
using Game.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Zlodey;

namespace Game.Systems
{
    public class UISystem : IInitializable,
        IListener<UpdateUI>, IListener<UILoaded>,
        IListener<Win>, IListener<Lose>
    {
        private StaticData _staticData;
        private RuntimeData _runtimeData;
        private UIMono _UI;

        [Inject]
        public UISystem(StaticData staticData, RuntimeData runtimeData)
        {
            _staticData = staticData;
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener<UpdateUI>(this);
            TriggerListenerSystem.AddListener<UILoaded>(this);
            TriggerListenerSystem.AddListener<Win>(this);
            TriggerListenerSystem.AddListener<Lose>(this);

            SceneManager.LoadSceneAsync(_staticData.UISceneName, LoadSceneMode.Additive);
        }

        void IListener<UpdateUI>.Trigger(UpdateUI update)
        {
            UpdateUI();
        }

        void IListener<UILoaded>.Trigger(UILoaded signal)
        {
            _UI = signal.UIMono;
            InitUI();
        }

        void IListener<Win>.Trigger(Win update)
        {
            UpdateUI();
        }

        void IListener<Lose>.Trigger(Lose signal)
        {
            InitUI();
        }

        private void InitUI()
        {
            _UI.StartCoroutine(AnimateHeader(_UI.AnimatedElementsTransform, () => UpdateUI()));
        }

        private void UpdateUI()
        {
            List<Tweener> tweeners = new List<Tweener>();

            var startCounter = int.Parse(_UI.GoldCounter.text);
            var finalCounter = _runtimeData.Player.Money;

            if (finalCounter - startCounter != 0)
            {
                var duration = MathF.Abs(finalCounter - startCounter) / 2;
                var tweener = DOTween.To(() =>
                    _UI.GoldCounter.text,
                    x => _UI.GoldCounter.text =
                    ((int)Mathf.MoveTowards(int.Parse(_UI.GoldCounter.text), finalCounter, duration)).ToString(),
                    finalCounter.ToString(), duration);
                tweeners.Add(tweener);
            }

            var startMoves = int.Parse(_UI.MovesCounter.text);
            var finalMoves = _runtimeData.Player.TurnCount;
            if (finalMoves - startMoves != 0)
            {
                var duration = MathF.Abs(finalMoves - startMoves) / 2;
                var tweener = DOTween.To(() =>
                    _UI.MovesCounter.text,
                    x => _UI.MovesCounter.text =
                    ((int)Mathf.MoveTowards(int.Parse(_UI.MovesCounter.text), finalMoves, duration)).ToString(),
                    finalCounter.ToString(), duration);
                tweeners.Add(tweener);
            }

            if (tweeners.Count != 0)
            {
                var sequence = DOTween.Sequence(this);
                foreach (var item in tweeners)
                {
                    sequence.Join(item);
                }
                sequence.Play();
            }
        }

        private IEnumerator AnimateHeader(RectTransform[] rects, Action onEndCallback)
        {
            foreach (var item in rects)
            {
                Sequence sequence = DOTween.Sequence(this);
                var finalPos = item.anchoredPosition;
                finalPos.y = 0;

                var moveTween = DOTween.To(() => item.anchoredPosition,
                    x => item.anchoredPosition = x, finalPos, 0.5f);
                var scaleTween = item.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InBounce);

                sequence
                    .Join(moveTween)
                    .Append(scaleTween)
                    .Play();

                yield return new WaitForSeconds(0.2f);
            }

            onEndCallback?.Invoke();
        }

        private IEnumerator ShowWinPanel()
        {
            yield return new WaitForSeconds(0.2f);
        }

        private IEnumerator ShowLosePanel()
        {
            yield return new WaitForSeconds(0.2f);
        }
    }
}