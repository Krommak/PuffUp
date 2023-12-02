using UnityEngine;

namespace Game
{
    public sealed class Player
    {
        public int TurnCount = 5;
        //{
        //    get => PlayerPrefs.GetInt("TurnCount", 5);
        //    set => PlayerPrefs.SetInt("TurnCount", value);
        //}

        public int Money;

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