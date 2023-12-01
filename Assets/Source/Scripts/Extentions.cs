using System.Collections.Generic;
using UnityEngine;

namespace Game.Extentions
{
    public static class Extentions
    {
        public static void RemoveBySwap<T>(this List<T> list, int index)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }

        public static T RandomElement<T>(this T[] array)
        {
            var randIndex = Random.Range(0, array.Length);
            return array[randIndex];
        }

        public static T RandomElement<T>(this T[] array, int start)
        {
            var randIndex = Random.Range(start, array.Length);
            return array[randIndex];
        }

        public static T RandomElement<T>(this T[] array, int start, int end)
        {
            var randIndex = Random.Range(start, end);
            return array[randIndex];
        }
    }
}