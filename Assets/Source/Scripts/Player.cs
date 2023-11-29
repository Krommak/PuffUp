using UnityEngine;
using Zlodey;

namespace Game
{
    public sealed class Player
    {
        public IntValue Money;
        public int CurrentLevel
        {
            get => PlayerPrefs.GetInt("CurrentLevel", 0);
            set => PlayerPrefs.SetInt("CurrentLevel", value);
        }

        public bool TryDecrease(int count)
        {
            return Money >= 0;
        }
        
        public void Increase(int count)
        {
            Money += count;
        }
        
        public void Decrease(int count)
        {
            Money -= count;
        }
    }
}