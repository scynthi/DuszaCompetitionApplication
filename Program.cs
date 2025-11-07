using Avalonia;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DuszaCompetitionApplication;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Használat: WpfApp1.exe [--ui | <test_dir_path>]");
            return;
        }
        if (args[0] == "--ui")
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            return;
        }
        try // If the absolute path is wrong
        {
            RunTestMode(args[0]);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    private static void RunTestMode(string path)
    {
        Console.WriteLine($"Test Mode started {path}");
        
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
