using System.Collections.Generic;

namespace Game.Extentions
{
    public static class Extentions
    {
        public static void RemoveBySwap<T>(this List<T> list, int index)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
    }
}