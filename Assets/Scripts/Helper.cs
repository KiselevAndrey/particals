using System;
using System.Collections.Generic;

public static class Helper
{
    static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    #region List<T>.Ind
    /// <summary>
    /// Определяет элемент от любого числа без IndexOutRange
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T Ind<T>(this List<T> list, int index)
    {
        index = Math.Abs(index);
        return list[index % list.Count];
    }

    /// <summary>
    /// Определяет элемент от любого числа без IndexOutRange
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T Ind<T>(this List<T> list, ref int index, bool shuffle = false)
    {
        if (shuffle && index >= list.Count) list.Shuffle();

        index = Math.Abs(index) % list.Count;
        return list[index];
    }
    #endregion

    #region RandomBool
    public static bool RandomBool()
    {
        return rng.Next(2) != 0;
    }
    
    /// <summary>
    /// Шанс получить true в соотношение value к 100
    /// </summary>
    public static bool RandomBoolPercent(int value) => rng.Next(100) <= value;
    #endregion
}
