using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using pdfGUI.e_inkDevices;
using pdfGUI.logicNshit.e_inkDevices;
using SixLabors.ImageSharp;
using System;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

namespace pdfGUI;

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
        monRepArray = monRepArray.OrderBy( file => Regex.Replace(file.Name, @"\d+", match => match.Value.PadLeft(4, '0'))).ToList();
        

        foreach (var folder in monRepArray)
        {
            DirectoryInfo imageRep = new DirectoryInfo(folder.FullName);
            List<FileInfo> mesImages = new List<FileInfo>(imageRep.GetFiles().Where(f => f.Extension == ".jpg" || f.Extension == ".jpeg" || f.Extension == ".png" || f.Extension == ".webp")); 
            mesImages = mesImages.OrderBy( file => Regex.Replace(file.Name, @"\d+", match => match.Value.PadLeft(4, '0'))).ToList();
            computeProgress(progress);
            
            foreach (var imageFile in mesImages)
            {
                PdfPage page = document.AddPage();

                page.Width = _device.resolution.X;
                page.Height = _device.resolution.Y;
                
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XImage image;

                if (imageFile.Extension == ".webp")
                { 
                    Image imageWebp = Image.Load($"{imageFile.FullName}");
                    var stream = new MemoryStream();
                    imageWebp.SaveAsJpeg(stream);
                    stream.Position = 0;
                    image = XImage.FromStream(stream);
                    imageWebp.Dispose();
                }
                else
                {
                    image = XImage.FromFile($"{imageFile.FullName}");    
                }
                
                double startX = Math.Max(
                    (page.Width - image.PixelWidth * ((float)page.Height / image.PixelHeight)) / 2,
                      0
                );
                double startY = 0;
                
                double width = Math.Min(
                    page.Height * ((float)image.PixelWidth / image.PixelHeight),
                    page.Width
                );
                double height = Math.Min(
                    page.Width * ((float)image.PixelHeight / image.PixelWidth),
                    page.Height
                );
                
                if ((float)image.PixelHeight / image.PixelWidth < 1)
                {
                    gfx.RotateTransform(90);
                    startX = 0;
                    startY = -page.Width;
                    width = Math.Min(
                        page.Width * ((float)image.PixelWidth  / image.PixelHeight),
                        page.Height
                    );
                    height =  Math.Min(
                        page.Height * ((float)image.PixelHeight / image.PixelWidth),
                        page.Width
                    );
                }
                
                gfx.DrawImage(
                    image,
                    startX,
                    startY, 
                    width, 
                    height
                );
                
                /*
                Console.WriteLine($"{imageFile.Name}");
                Console.WriteLine($"Height: {height}");
                Console.WriteLine($"Width: {width}");
                Console.WriteLine($"Page Height: {page.Height}");
                Console.WriteLine($"Page Width: {page.Width}");
                Console.WriteLine($"Image Height: {image.PixelHeight}");
                Console.WriteLine($"Image Width: {image.PixelWidth}");
                Console.WriteLine($"");
                */
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