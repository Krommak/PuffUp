using DG.Tweening;
using Game.Data;
using Game.Signals;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Systems
{
    public class UISystem : IInitializable,
        IListener<UpdateUI>, IListener<UILoaded>,
        IListener<Lose>, IListener<ShowRewardPanel>
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
            TriggerListenerSystem.AddListener<ShowRewardPanel>(this);
            TriggerListenerSystem.AddListener<UpdateUI>(this);
            TriggerListenerSystem.AddListener<UILoaded>(this);
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

        void IListener<Lose>.Trigger(Lose signal)
        {
            InitUI();
        }

        void IListener<ShowRewardPanel>.Trigger(ShowRewardPanel signal)
        {
            ShowRewardPanel(signal.MovesCount, 3f);
        }

        private void InitUI()
        {
            _UI.StartCoroutine(AnimateHeader(_UI.AnimatedElementsTransform, () => UpdateUI()));
        }

        private void UpdateUI()
        {
            var sequence = DOTween.Sequence(this);
            sequence.Join(UpdateTextCounter(_UI.GoldCounter, _runtimeData.Player.Money));
            sequence.Join(UpdateTextCounter(_UI.MovesCounter, _runtimeData.Player.TurnCount));
            sequence.Play();
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

        private void ShowRewardPanel(int reward, float duration)
        {
            var text = _UI.RewardText;
            var background = _UI.RewardBackground;
            text.text = $"+{reward} ходов";
            var sequence = DOTween.Sequence(this);

            var color = background.color;

            sequence.Join(DOTween.To(() => text.alpha, x => text.alpha = x, 1, 2));
            sequence.Join(background.DOColor(new Color(color.r, color.g, color.b, 1), 2));
            sequence.Join(UpdateTextCounter(_UI.MovesCounter, _runtimeData.Player.TurnCount));
            sequence.AppendInterval(duration);
            sequence.Join(DOTween.To(() => text.alpha, x => text.alpha = x, 0, 1));
            sequence.Join(background.DOColor(new Color(color.r, color.g, color.b, 0), 2));
            sequence.Play();
        }

        private Tweener UpdateTextCounter(TMP_Text text, int final)
        {
            var startCounter = int.Parse(text.text);

            var duration = MathF.Abs(final - startCounter) / 2;
            var tweener = DOTween.To(() =>
                text.text,
                x => text.text =
                ((int)Mathf.MoveTowards(int.Parse(text.text), final, duration)).ToString(),
                text.ToString(), duration);

            return tweener;
        }

        private IEnumerator ShowLosePanel()
        {
            yield return new WaitForSeconds(0.2f);
        }
    }
}