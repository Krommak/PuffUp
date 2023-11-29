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
        private Player _player;
        private StaticData _staticData;
        private RuntimeData _runtimeData;

        private LevelMono _loadedLevel;

        [Inject]
        public LevelSystem(Player player, StaticData staticData, RuntimeData runtimeData)
        {
            _player = player;
            _staticData = staticData;
            _runtimeData = runtimeData;
        }

        private void LoadCurrent()
        {
            var level = _player.CurrentLevel;

            var totalLevels = _staticData.LevelsSettings.Levels.Length;
            var index = level;
            if (level >= totalLevels)
            {
                index = level % totalLevels;
                index %= totalLevels;
            }

            var levelSettings = _staticData.LevelsSettings.Levels[index];

            _loadedLevel = GameObject.Instantiate(levelSettings.LevelPrefab);

            _loadedLevel.Init();

            _runtimeData.LevelData = levelSettings;

            TriggerListenerSystem.Trigger<LevelLoaded>();
        }

        public void Initialize()
        {
            TriggerListenerSystem.AddListener(this);

            LoadCurrent();
        }

        void IListener<LoadLevel>.Trigger<LoadLevel>()
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