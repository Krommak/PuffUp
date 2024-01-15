using Game.MonoBehaviours;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class StaticData
    {
        [SerializeField]
        public string GameSceneName;
        [SerializeField]
        public string UISceneName;

        [Header("Material for Sphere")]

        public Material TransparentMaterial;
        public Material VoodooMaterial;

        [Header("Levels")]
        public LevelsSettings LevelsSettings;

        [Header("Gameplay variable")] 
        public float TimeToWinLevel = 1; //для примера - время в секундах после которого уровень выигрывается
        public BubbleMono PrefabBubble;

        public float Tick;

        public int StartTurn;

        public GameObject PrefabCoin;

        public MaterialPropertyBlock MaterialPropertyBlock;

        public Sprite[] PositiveEmoji;
        public Sprite[] NegativeEmoji;
    }

    [Serializable]
    public struct SpecialColor
    {
        [Layer]
        public int LayerColor;
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
        public LevelMono LevelMono;
        public ColorScheme ColorScheme;
        public int StartTurn;
    }
}