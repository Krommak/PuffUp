using Game.Data;
using Game.MonoBehaviours;
using Game.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Systems
{
    public class LevelSystem : IInitializable, IListener<LoadLevel>
    {
        private StaticData _staticData;
        private RuntimeData _runtimeData;

        [Inject]
        public LevelSystem(StaticData staticData, RuntimeData runtimeData)
        {
            _staticData = staticData;
            _runtimeData = runtimeData;
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener(this);

            LoadCurrent();
        }

        private void LoadCurrent()
        {
            TriggerListenerSystem.Trigger(new SetGameState()
            {
                GameState = GameState.LoadingStart,
            });
            var level = _runtimeData.Player.CurrentLevel;

            var totalLevels = _staticData.LevelsSettings.Levels.Length;
            var index = level;
            if (level >= totalLevels)
            {
                index = level % totalLevels;
                index %= totalLevels;
            }

            var levelSettings = _staticData.LevelsSettings.Levels[index];

            _runtimeData.LevelData = levelSettings;

            GameObject.Instantiate<LevelMono>(levelSettings.LevelMono);

            TriggerListenerSystem.Trigger(new SetGameState()
            {
                GameState = GameState.LoadingExit,
            });
        }

        void IListener<LoadLevel>.Trigger(LoadLevel loadLevel)
        {
            ReloadScene();
            LoadCurrent();
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(_staticData.GameSceneName);
        }
    }
}