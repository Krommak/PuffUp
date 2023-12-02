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
            var level = _runtimeData.Player.CurrentLevel;

            var totalLevels = _staticData.LevelsSettings.Levels.Length;
            var index = level;
            if (level >= totalLevels)
            {
                index = level % totalLevels;
                index %= totalLevels;
            }

            var levelSettings = _staticData.LevelsSettings.Levels[index];

            _runtimeData.LoadedLevel = GameObject.Instantiate<LevelMono>(levelSettings.LevelPrefab);
            _runtimeData.LoadedLevel.Init();
            _runtimeData.LevelData = levelSettings;

            TriggerListenerSystem.Trigger(new LevelLoaded());
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