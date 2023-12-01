using Game.MonoBehaviours;
using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class RuntimeData
    {
        public RuntimeData()
        {
            Player = new Player();
            Block = new MaterialPropertyBlock();
        }

        public Player Player;
        public int LevelNum;
        public Level LevelData;
        //public GameState GameState;
        public float LevelStartedTime;

        public Vector3 MousePosition;
        public float DelayTime;
        public int LevelRoom = -1;

        public Unlocker Unlocker;

        public BubbleMono ScaledBubble;

        public bool ChestActivated;

        public MaterialPropertyBlock Block;
    }

    public class Unlocker
    {
        private int ValueForUnlock;
        private int CurrentValue;
        private Action OnUnlock;

        public void Increase(int value)
        {
            CurrentValue += value;
            if(ValueForUnlock <= CurrentValue)
            {
                OnUnlock?.Invoke();
            }
        }
    }
}