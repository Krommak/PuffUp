using Game.Data;
using Game.Systems;
using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField]
        private ScreenSettings _screenSettings;

        [SerializeField]
        private StaticData _staticData;

        [SerializeField]
        private RuntimeData _runtimeData;

        public override void InstallBindings()
        {
            _screenSettings.ApplySettings();

            Container.Bind<StaticData>()
                .FromInstance(_staticData)
                .AsSingle();

            Container.Bind<RuntimeData>()
                .FromInstance(_runtimeData)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<LevelSystem>()
                .AsSingle();
        }

        private void OnDestroy()
        {
            TriggerListenerSystem.ClearAllListeners();
        }
    }

    [Serializable]
    public class ScreenSettings
    {
        [SerializeField]
        private bool _autorotateToPortrait = true;
        [SerializeField]
        private bool _autorotateToPortraitUpsideDown = true;
        [SerializeField]
        private bool _autorotateToLandscapeLeft = false;
        [SerializeField]
        private bool _autorotateToLandscapeRight = false;
        [SerializeField]
        private ScreenOrientation _orientation = ScreenOrientation.Portrait;

        public void ApplySettings()
        {
            UnityEngine.Screen.autorotateToPortrait = _autorotateToPortrait;
            UnityEngine.Screen.autorotateToPortraitUpsideDown = _autorotateToPortraitUpsideDown;
            UnityEngine.Screen.autorotateToLandscapeLeft = _autorotateToLandscapeLeft;
            UnityEngine.Screen.autorotateToLandscapeRight = _autorotateToLandscapeRight;
            UnityEngine.Screen.orientation = _orientation;
        }
    }
}