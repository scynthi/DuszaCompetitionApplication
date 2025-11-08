using System;
using System.Collections.Generic;
using DuszaCompetitionApplication.GameElements;

namespace DuszaCompetitionApplication;

public static class Utility
{
    public static void PrintArray(string[][] array)
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
    public static void PrintArray(List<string[]> array)
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
    public static void PrintArray(string[] array)
    {
        foreach (string el in array)
        {
            Console.Write(el + ' ');
        }
        Console.WriteLine();
    }
    public static void PrintBattle(List<string> array)
    {
        foreach (string el in array)
        {
            Console.WriteLine(el);
        }
        Console.WriteLine();
    }

    public static void PrintArray(Card[] array)
    {
        foreach (Card el in array)
        {
            Console.Write(el.Name + ' ');
        }

    }
    public static void PrintArray(List<Card> array)
    {
        foreach (Card el in array)
        {
            Console.Write(el.Name + ' ');
        }
        Console.WriteLine();
    }
    public static void PrintArray(List<Kazamata> array)
    {
        foreach (Kazamata el in array)
        {
            Console.Write(el.Name + ' ');
        }
        Console.WriteLine();
    }
}
