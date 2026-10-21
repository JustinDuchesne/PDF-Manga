using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using pdfGUI.e_inkDevices;
using pdfGUI.logicNshit.e_inkDevices;

namespace pdfGUI;

public partial class MainWindow : Window
{
    List<Uri> _folders = new List<Uri>();
    private string output;
    
    public List<devices> Devices { get; } = new()
    {
        new Remarkable2(), new RemarkablePaperPro(), new TabS9Fe()
    };

    public devices _selectedDevice { get; set; } = new Remarkable2();
    
    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var progress = new Progress<int>(percent =>
        {
            Console.WriteLine(percent);
            ProgressBar.Value = percent;
        });
        
        PdfWrite pdf = new PdfWrite(_folders, output, pdfName.Text, _selectedDevice);
        await Task.Run(() => pdf.Write(progress));
    }
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private async void OpenFileButtonInput_Clicked(object sender, RoutedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        
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
    
    private async void OpenFileButtonOutput_Clicked(object sender, RoutedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });
        if (folders.Count == 0) return;
        
        output = folders[0].Path.LocalPath;
    }
}