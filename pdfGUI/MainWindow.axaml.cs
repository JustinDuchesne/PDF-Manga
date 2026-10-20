using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.Threading.Tasks;

namespace pdfGUI;

public partial class MainWindow : Window
{
    
    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var progress = new Progress<int>(percent =>
        {
            Console.WriteLine(percent);
            ProgressBar.Value = percent;
        });
        
        PdfWrite pdf = new PdfWrite(inpoot.Text, outpoot.Text, pdfName.Text, e_inkDevice.Remarkable2);
        await Task.Run(() => pdf.Write(progress));
    }
    public MainWindow()
    {
        InitializeComponent();
    }
}