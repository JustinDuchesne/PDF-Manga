using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using pdfGUI.e_inkDevices;

namespace pdfGUI;
using System;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

using System.IO;

public class PdfWrite
{
    List<Uri> _inpoot = new List<Uri>();
    private string _outpoot { get; set; }
    private string _name { get; set; }
    public devices _device { get; set; }
    
    //For progress tracker (the little bar smile :)
    public int _currentProgress { get; set; } = 0;
    public int _numberFiles { get; set; } = 0;
    
    public float _percentage { get; set; } = 0;


    public PdfWrite()
    {
        _device = new Remarkable2();
        computeTotalDirs();
    }

    public bool CanPdfWrite()
    {
        if (_inpoot.Count == 0)
            return false;
        return true;
    }
    
    public PdfWrite(List<Uri> inpoot, string outpoot, string name, devices device)
    {
        _inpoot = inpoot;
        _outpoot = outpoot;
        _name = name;
        _device = device;
        computeTotalDirs();
    }

    public void setInpoot(List<Uri> inpoot)
    {
        _inpoot = inpoot;
        computeTotalDirs();
    }
    public void setOutpoot(string outpoot)
    {
        _outpoot = outpoot;
    }
    public void setName(string name)
    {
        _name = name;
    }
    public void setDevice(devices device)
    {
        _device = device;
    }
    public void setCurrentProgress(int currentProgress)
    {
        _currentProgress = currentProgress;
    }
    public void setNumberFiles(int numberFiles)
    {
        _numberFiles = numberFiles;
    }
    
    public void Write(IProgress<int> progress)
    {
        if (!CanPdfWrite()) return;
        
        progress.Report(Convert.ToInt32(0));
        PdfDocument document = new PdfDocument();

        List<DirectoryInfo> monRepArray = new List<DirectoryInfo>();
        foreach (var path in _inpoot)
        {
            monRepArray.Add(new DirectoryInfo(path.LocalPath));
        }
        monRepArray = monRepArray.OrderBy(d => d.Name).ToList();
        

        foreach (var folder in monRepArray)
        {
            Console.WriteLine(folder.FullName);
            DirectoryInfo imageRep = new DirectoryInfo(folder.FullName);
            List<FileInfo> mesImages = new List<FileInfo>(imageRep.GetFiles().Where(f => f.Extension == ".jpg" || f.Extension == ".jpeg" || f.Extension == ".png" || f.Extension == ".webp")); 
            computeProgress(progress);
            foreach (var imageFile in mesImages)
            {
                bool rotate = false;
                PdfPage page = document.AddPage();

                page.Width = _device.resolution.X;
                page.Height = _device.resolution.Y;
                
                XGraphics gfx = XGraphics.FromPdfPage(page);
                Console.WriteLine($"{imageFile.FullName}");
                XImage image = XImage.FromFile($"{imageFile.FullName}");
                
                if ((float)image.PixelHeight / image.PixelWidth < 0.9)
                    rotate = true;

                if (rotate)
                {
                    gfx.RotateTransform(90);
                    gfx.DrawImage( image, 0, -page.Width, page.Height, page.Width);
                }
                else
                {
                    gfx.DrawImage(image, 0, 0, page.Width, page.Height);   
                }
            }
        }
        document.Save($"{_outpoot}/{_name}.pdf");
    }

    private void computeTotalDirs()
    {
        setNumberFiles(_inpoot.Count);
    }

    public void computeProgress(IProgress<int> progress)
    {
        setCurrentProgress(_currentProgress + 1);
        _percentage = (float)_currentProgress / _numberFiles * 100;
        progress.Report(Convert.ToInt32(_percentage));
        
    }
}