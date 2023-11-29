using Game.MonoBehaviours;
using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class StaticData
    {
        [SerializeField]
        public string GameSceneName;

        [Header("Material for Sphere")]

        public Material TransparentMaterial;
        public Material VoodooMaterial;

        [Header("Levels")]
        public LevelsSettings LevelsSettings;

        //[Header("Required prefabs")]        
        //      public UI UI;

        //[Layer] public int IgnoreLayer;

        [Header("Gameplay variable")] public float TimeToWinLevel = 1; //для примера - время в секундах после которого уровень выигрывается
        public GameObject PrefabBubble;
        public SpecialColor[] Colors;
        public float Tick;

        public int StartTurn;

        //public LevelData DefaultLevel;

        public GameObject PrefabCoin;

        public MaterialPropertyBlock MaterialPropertyBlock;
    }

    [Serializable]
    public struct SpecialColor
    {
        public Color BaseColor;
        public Color ShadowColor;
    }

    [Serializable]
    public class LevelsSettings
    {
        public Level[] Levels;
    }

    [Serializable]
    public class Level
    {
        public Vector2 ClampSize;
        public LevelMono LevelPrefab;
        public SpecialColor BackgroundColor;
        public SpecialColor WallColor;
        public SpecialColor ObstacleColor;
        public int StartTurn;
    }
}