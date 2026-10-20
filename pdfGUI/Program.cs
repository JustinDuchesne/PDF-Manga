using Avalonia;
using System;

namespace pdfGUI;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        //PdfWrite pdf = new PdfWrite("/mnt/disque2/coding shenanigans/personal/pdf/testshit/", "/mnt/disque2/coding shenanigans/personal/pdf/ootpoot/", "hmmm", e_inkDevice.Remarkable2);
        //pdf.Write();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .With(new X11PlatformOptions
            {
                UseDBusFilePicker = false
            })
            .WithInterFont()
            .LogToTrace();
}