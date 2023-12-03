using Game.Signals;
using Game.Systems;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class LevelMono : MonoStateListener, IListener<NextLevelPart>
    {
        public GameObject[] Parts;
        public float LevelPartStepY;
        public float Duration = 1f;
        public int CurrentPart { get; private set; } = -1;

        [OnState(GameState.LoadingExit)]
        private void OnState()
        {
            TriggerListenerSystem.AddListener<NextLevelPart>(this);
            ActivateNext();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            TriggerListenerSystem.RemoveListener<NextLevelPart>(this);
        }

        public void Trigger(NextLevelPart signal)
        {
            ActivateNext();
        }

        private void ActivateNext()
        {
            CurrentPart++;
            
            if(Parts.Length >= CurrentPart)
            {
                Parts[CurrentPart].SetActive(true);
                TriggerListenerSystem.Trigger(new LevelMoveToCamera() 
                {
                    LevelMono = this,
                    OnEndAction = () =>
                    {
                        if (CurrentPart != 0)
                        {
                            Parts[CurrentPart - 1].SetActive(false);
                        }
                    }
                });
            }
            else
            {
                TriggerListenerSystem.Trigger(new SetGameState()
                {
                    GameState = GameState.WinOpen
                });
            }

        }
    }
}