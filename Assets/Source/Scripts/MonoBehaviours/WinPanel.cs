using DG.Tweening;
using Game.Data;
using Game.MonoBehaviours;
using Game.Signals;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems
{
    public class WinPanel : MonoStateListener
    {
        [SerializeField]
        private Image[] _stars;
        [SerializeField]
        private Image _emoji;
        [SerializeField]
        private TMP_Text _currentLevel;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _resumeButton;

        private RectTransform _rectTransform;

        private RuntimeData _runtimeData;
        private StaticData _staticData;

        private Vector2 _startPosition;

        protected override void Awake()
        {
            base.Awake();

            _rectTransform = GetComponent<RectTransform>();
            _startPosition = _rectTransform.anchoredPosition;
        }

        [OnState(GameState.WinOpen)]
        private void OnWinStart()
        {
            _resumeButton.onClick.AddListener(() =>
            {
                _runtimeData.Player.CurrentLevel++;
                TriggerListenerSystem.Trigger(new ChangeGameState()
                {
                    GameState = GameState.WinClose
                });
                TriggerListenerSystem.Trigger(new LoadLevel());
            });
            _restartButton.onClick.AddListener(() =>
            {
                TriggerListenerSystem.Trigger(new ChangeGameState()
                {
                    GameState = GameState.WinClose
                });
                TriggerListenerSystem.Trigger(new LoadLevel());
            });

            _rectTransform.DOAnchorPos(new Vector2(0, 0), 1f);
        }

        public void Init(RuntimeData runtimeData, StaticData staticData)
        {
            _runtimeData = runtimeData;
            _staticData = staticData;
        }

        [OnState(GameState.WinClose)]
        private void OnWinClose()
        {
            _resumeButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
            _rectTransform.DOAnchorPos(_startPosition, 1f);
        }
    }
}