using System;
using DuszaCompetitionApplication.GameElements;

namespace DuszaCompetitionApplication;

public static class Utility
{
    public static void PrintArray(string[][] array, bool twoD)
	{
        if (twoD)
        {
            foreach (string[] arr in array)
            {
                foreach (string el in arr)
                {
                    Console.Write(el + " ");
                }
                Console.Write('\n');
            }
        }
    }
    public static void PrintArray(string[] array)
    {
        foreach (string el in array)
        {
            Console.Write(el + '\n');
        }

    }
    public static void PrintArray(Card[] array)
    {
        foreach (Card el in array)
        {
            Console.Write(el.Name + ' ');
        }

    }
}
