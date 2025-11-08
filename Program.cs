using Avalonia;
using DuszaCompetitionApplication.GameElements;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DuszaCompetitionApplication;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Használat: [--ui | <test_dir_path>]");
            return;
        }
        if (args[0] == "--ui")
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            return;
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
    private static void RunTestMode(string path)
    {
        Console.WriteLine($"Test Mode started {path}");
        GameManager gManager = new GameManager(path);
        gManager.StartGame();

    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
