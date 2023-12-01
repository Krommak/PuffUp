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

        private LevelMono _loadedLevel;

        [Inject]
        public LevelSystem(StaticData staticData, RuntimeData runtimeData)
        {
            _staticData = staticData;
            _runtimeData = runtimeData;
        }

        private void LoadCurrent()
        {
            var level = _runtimeData.Player.CurrentLevel;

            var totalLevels = _staticData.LevelsSettings.Levels.Length;
            var index = level;
            if (level >= totalLevels)
            {
                index = level % totalLevels;
                index %= totalLevels;
            }

            var levelSettings = _staticData.LevelsSettings.Levels[index];

            _loadedLevel = GameObject.Instantiate<LevelMono>(levelSettings.LevelPrefab);

            _loadedLevel.Init();

            _runtimeData.LevelData = levelSettings;

            TriggerListenerSystem.Trigger(new LevelLoaded());
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener(this);

            LoadCurrent();
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