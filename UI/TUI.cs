using DuszaCompetitionApplication.Enums;
using DuszaCompetitionApplication.GameElements;
using System;
using System.Collections.Generic;

namespace DuszaCompetitionApplication.UI;

public class TUI
{
    private enum Menus
    {
        MainMenu,
        Dungeons,
        Deck,
        WorldCards
    }
    GameManager gManager;

    private string[] mainMenuButtons = { "Dungeons", "Deck", "All cards" };
    private string[][] menus;
    private Menus currentMenu = Menus.MainMenu;

    private int currentSelected = 0;
    private const ConsoleColor BACKGROUNDCOLOR = ConsoleColor.DarkGreen;
    private const ConsoleColor TEXTCOLOR = ConsoleColor.Black;
    private const ConsoleColor HIGHLIGHTCOLOR = ConsoleColor.White;
    private const ConsoleColor SELECTCOLOR = ConsoleColor.Gray;

    List<Card> pakli;

    public TUI()
    {
        Console.BackgroundColor = BACKGROUNDCOLOR;
        Console.ForegroundColor = TEXTCOLOR;
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        pakli = new List<Card>();

        gManager = new GameManager("", Enums.GameModes.Game);
        gManager.StartGameMode();
        List<string> dungeonMenu = gManager.GetKazamatas();
        dungeonMenu.Insert(0, "Back");
        List<string> deckMenu = gManager.GetCardsCollection();
        deckMenu.Insert(0, "Back");

        menus = new[] { mainMenuButtons, dungeonMenu.ToArray(), deckMenu.ToArray() };

        //Console.WriteLine($"Game Mode started");
        GameLoop();
    }

    public void GameLoop()
    {
        PrintButtons(mainMenuButtons, currentSelected);
        ConsoleKeyInfo pressed;
        Card tempCard = new Card();
        Card tempCardArray = new Card() { };
        List<int> CardsSelected = new List<int>();
        do
        {
            pressed = Console.ReadKey();
            switch (pressed.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (currentSelected > 0)
                    {
                        currentSelected--;
                        Console.Clear();
                        ButtonSwitchBeep();
                        PrintButtons(menus[(int)currentMenu], currentSelected);
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (currentSelected < menus[(int)currentMenu].Length - 1)
                    {
                        currentSelected++;
                        Console.Clear();
                        ButtonSwitchBeep();
                        PrintButtons(menus[(int)currentMenu], currentSelected);
                    }
                    break;
                case ConsoleKey.Enter:
                    switch (currentMenu)
                    {
                        case Menus.MainMenu:
                            currentMenu = (Menus)currentSelected + 1;
                            currentSelected = 0;
                            break;
                        case Menus.Dungeons:
                            if (currentSelected == 0)
                            {
                                currentMenu = Menus.MainMenu;
                                currentSelected = 0;
                                break;
                            }
                            Console.Clear();
                            gManager.BattleLoop(menus[(int)currentMenu][currentSelected], "");
                            currentSelected = 0;
                            Console.WriteLine("Press Enter to exit the battle!");
                            while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                            Console.Clear();
                            break;
                        case Menus.Deck:
                            List<string> deckMenu = gManager.GetCardsCollection();
                            Console.WriteLine(gManager.GetCardsCollection().Count);
                            deckMenu.Insert(0, "Back");
                            menus[(int)Menus.Deck] = deckMenu.ToArray();
                            if (currentSelected == 0)
                            {
                                currentMenu = Menus.MainMenu;
                                currentSelected = 0;
                                break;
                            }
                            Console.Clear();
                            if (CardsSelected.Contains(currentSelected))
                            {
                                gManager.TryReturnCardFromName(menus[(int)currentMenu][currentSelected], gManager.GetPakli().ToArray(), out tempCard);
                                CardsSelected.Remove(currentSelected);
                                gManager.RemoveFromPakli(tempCard);
                            }
                            else
                            {
                                if (CardsSelected.Count < Math.Ceiling((double)gManager.GetCollection().Count / 2))
                                {
                                    CardsSelected.Add(currentSelected);
                                    CardsSelected.Sort();
                                    gManager.ClearPakli();
                                    pakli.Clear();
                                    foreach (int index in CardsSelected)
                                    {
                                        gManager.TryReturnCardFromName(menus[(int)currentMenu][index], gManager.GetCollection().ToArray(), out tempCard);
                                        pakli.Add(tempCard);
                                    }
                                    gManager.TryAddCardToPakli(pakli.ToArray());
                                }
                            }
                            break;

                    }
                    ButtonSelectBeep();
                    break;
            }
            Console.Clear();
            if (currentMenu == Menus.Deck)
            {
                if (gManager.TryReturnCardFromName(menus[(int)currentMenu][currentSelected], gManager.GetCollection().ToArray(), out tempCard)) PrintCardInfo(tempCard);
                PrintButtons(menus[(int)currentMenu], CardsSelected, currentSelected);
            }
            else
            {
                PrintButtons(menus[(int)currentMenu], currentSelected);
            }
            Utility.PrintArray(gManager.GetPakli());
        } while (pressed.Key != ConsoleKey.Escape);
    }

    private void PrintButtons(string[] buttons, int currIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Console.BackgroundColor = currIndex == i ? HIGHLIGHTCOLOR : BACKGROUNDCOLOR;
            Console.Write(buttons[i]);
            Console.BackgroundColor = BACKGROUNDCOLOR;
            Console.Write(' ');
        }
    }
    private void PrintButtons(string[] buttons, List<int> selected, int currIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == currIndex) Console.BackgroundColor = HIGHLIGHTCOLOR;
            else Console.BackgroundColor = selected.Contains(i) ? SELECTCOLOR : BACKGROUNDCOLOR;
            Console.Write(buttons[i]);
            Console.BackgroundColor = BACKGROUNDCOLOR;
            Console.Write(' ');
        }
    }

    private void ButtonSwitchBeep()
    {
        Console.Beep(1200, 50);
    }
    private void ButtonSelectBeep()
    {
        Console.Beep(500, 100);
    }
    private void PrintCardInfo(Card card)
    {
        Console.SetCursorPosition(0, 5);
        Console.WriteLine($"Card name: {card.Name}");
        Console.WriteLine($"Card attack: {card.Attack}");
        Console.WriteLine($"Card health: {card.Health}");
        Console.WriteLine($"Card element: {card.Element.ToString()}");
        Console.WriteLine($"Card type: {card.Type.ToString()}");
        Console.SetCursorPosition(0, 0);
    }
}
