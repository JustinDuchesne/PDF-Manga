using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.Threading.Tasks;

namespace pdfGUI;

public partial class MainWindow : Window
{
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Click!");
        Console.WriteLine($"Click! pdfName={pdfName.Text}");
        Console.WriteLine($"Click! inpoot={inpoot.Text}");
        Console.WriteLine($"Click! outpoot={outpoot.Text}");
        //if (double.TryParse(Celsius.Text, out double C))
        //{
        //    var F = C * (9d / 5d) + 32;
        //}
        PdfWrite pdf = new PdfWrite(inpoot.Text, outpoot.Text, pdfName.Text, e_inkDevice.Remarkable2);
        pdf.Write();
    }
    public MainWindow()
    {
        InitializeComponent();
    }
}