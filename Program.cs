using Avalonia;
using System;

namespace DuszaCompetitionApplication;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args[0] == "--ui")
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args); 
            }   
        }
        else
        {
            Console.WriteLine("Running in console mode");
        }
    } 

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
