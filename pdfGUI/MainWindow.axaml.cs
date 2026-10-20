using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using pdfGUI.e_inkDevices;

namespace pdfGUI;

public partial class MainWindow : Window
{
    List<Uri> _folders = new List<Uri>();
    private PdfWrite _pdf;
    
    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var progress = new Progress<int>(percent =>
        {
            Console.WriteLine(percent);
            ProgressBar.Value = percent;
        });
        
        PdfWrite pdf = new PdfWrite(_folders, outpoot.Text, pdfName.Text, new Remarkable2());
        await Task.Run(() => pdf.Write(progress));
    }
    public MainWindow()
    {
        InitializeComponent();
        _pdf = new PdfWrite();
    }
    
    private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = true
        });
        foreach (var folder in folders)
        {
            _folders.Add(folder.Path);
        }
        
    }
}