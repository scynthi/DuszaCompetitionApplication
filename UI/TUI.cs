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

    public TUI()
    {
        Console.BackgroundColor = BACKGROUNDCOLOR;
        Console.ForegroundColor = TEXTCOLOR;
        Console.Clear();
        gManager = new GameManager("", Enums.GameModes.Game);
        gManager.StartGameMode();
        List<string> dMenus = gManager.GetKazamatas();
        dMenus.Insert(0, "Back");

        menus = new[] { mainMenuButtons, dMenus.ToArray() };

        //Console.WriteLine($"Game Mode started");
        GameLoop();
    }

    public void GameLoop()
    {
        PrintButtons(mainMenuButtons, currentSelected);
        ConsoleKeyInfo pressed;
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
                            while (Console.ReadKey().Key != ConsoleKey.Enter);
                            break;

                    }
                    ButtonSelectBeep();
                    Console.Clear();
                    PrintButtons(menus[(int)currentMenu], currentSelected);
                    break;
            }
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

    private void ButtonSwitchBeep()
    {
        Console.Beep(1200, 50);
    }
    private void ButtonSelectBeep()
    {
        Console.Beep(500, 100);
    }
}
