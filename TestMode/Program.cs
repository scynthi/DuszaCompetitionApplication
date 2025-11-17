using Avalonia;
using DuszaCompetitionApplication.GameElements;
using DuszaCompetitionApplication.UI;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DuszaCompetitionApplication;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("undefined");
        Console.ResetColor();
        
        if (args.Length != 1)
        {
            Console.WriteLine("Használat: [--ui | <test_dir_path>]");
            return;
        }
        if (args[0] == "--ui")
        {
            Console.WriteLine("alma");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            return;
        }
        else if (args[0] == "--tui")
        {
            StartTUI();
        }
        else
        {
            try
            {
                RunTestMode(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("It's over");
            }
        }
    }
    private static void StartTUI() 
    {
        TUI tui = new TUI();
    }
    private static void RunTestMode(string path)
    {
        if (path.EndsWith('\\')) path = path.Substring(0, path.Length - 1);
        Console.WriteLine($"Test Mode started {path}");
        GameManager gManager = new GameManager(path, Enums.GameModes.Test);
        gManager.StartTestMode();

    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
