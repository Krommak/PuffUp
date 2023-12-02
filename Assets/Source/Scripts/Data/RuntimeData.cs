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
        public Level LevelData;
        //public GameState GameState;
        public float LevelStartedTime = 5;
        
        public LevelMono LoadedLevel;

        public Vector3 MousePosition;
        public float DelayTime;

        public MaterialPropertyBlock Block;
    }
}