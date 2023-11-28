using Game.Systems;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameStartup : MonoBehaviour
    {
        private SystemsContainer _systems;

        [Inject]
        public void Construct()
        {
        }

        private void Awake()
        {
            _systems = new SystemsContainer();
        }

        private void Start()
        {

        }

        private void Update()
        {
            _systems.UpdateSystems();
        }

        private void OnDestroy()
        {
            _systems.Dispose();
        }
    }
}