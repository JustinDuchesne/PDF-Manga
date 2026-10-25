using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using pdfGUI.e_inkDevices;
using pdfGUI.logicNshit.e_inkDevices;

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
                XImage image = XImage.FromFile($"{imageFile.FullName}");



                double startX = (page.Width - image.PixelWidth * ((float)page.Height / image.PixelHeight)) / 2;
                double startY = 0;
                
                float Ratio = (float)image.PixelHeight / image.PixelWidth;
                if (Ratio < 0.9)
                {
                    rotate = true;
                    gfx.RotateTransform(90);
                    startX = 0;
                    startY = -page.Width;
                }
                
                double width = Math.Min(
                    page.Height * ((float)image.PixelWidth / image.PixelHeight),
                    page.Width
                );
                double height = Math.Min(
                    page.Width * ((float)image.PixelHeight / image.PixelWidth),
                    page.Height
                );
                
                
                gfx.DrawImage(
                    image,
                    startX,  //(page.Width - image.PixelWidth * ((float)page.Height / image.PixelHeight)) / 2
                    startY, 
                    width, 
                    height
                );
                
                /*
                if (rotate)
                {
                    gfx.RotateTransform(90);
                    float ratio = (float)page.Height * ((float)image.PixelHeight / image.PixelWidth);
                    
                    //Console.WriteLine($"RATIO: {ratio}");
                    //Console.WriteLine($"Page Height: {page.Height}");
                    //Console.WriteLine($"Page Width: {page.Width}");
                    //Console.WriteLine($"Image Height: {image.PixelHeight}");
                    //Console.WriteLine($"Image Width: {image.PixelWidth}");
                    //Console.WriteLine($"");
                    
                    gfx.DrawImage(image, 0, -page.Width, page.Height, ratio);
                    
                    //gfx.DrawImage(image, 0, -page.Width, image.PixelWidth * ((float)page.Width / image.PixelHeight), image.PixelWidth / ((float)page.Height / image.PixelHeight )); 
                    //gfx.DrawImage(image, 0, -page.Width, page.Height, image.PixelWidth / ((float)page.Height / image.PixelHeight )); 
                    //gfx.DrawImage( image, (page.Height - image.PixelHeight * ((float)page.Width / image.PixelWidth)) / 2, -(page.Height - image.PixelHeight * ((float)page.Width / image.PixelWidth)) / 2, page.Height, image.PixelHeight * (page.Width / image.PixelWidth )); 
                    //gfx.DrawImage(image,  (page.Width - image.PixelWidth * ((float)page.Height / image.PixelHeight)) / 2, 0, image.PixelWidth * ((float)page.Height / image.PixelHeight), page.Height); 
                    //gfx.DrawImage( image, 0, -page.Width / 1.2f, page.Height, image.PixelHeight * (page.Width / image.PixelWidth )); //(page.Height - image.PixelHeight)/3    -page.Width
                }
                else
                {
                    //gfx.DrawImage(image, (page.Width - image.PixelWidth)/3, 0, image.PixelWidth * (page.Height / image.PixelHeight ), page.Height); 
                    //gfx.DrawImage(image, (image.PixelWidth * ((float)page.Height / image.PixelHeight) - page.Width) / 2, 0, image.PixelWidth * ((float)page.Height / image.PixelHeight), page.Height);
                    
                    
                    //float ratio = (float)page.Width * ((float)image.PixelHeight / image.PixelWidth);
                    //double ratio = (float)page.Height * ((float)image.PixelWidth / image.PixelHeight);
                    //Props to Asde_dfs
                    double ratio = Math.Min(
                        (float)page.Height * ((float)image.PixelWidth / image.PixelHeight),
                        (float)page.Height * ((float)image.PixelWidth / image.PixelHeight)
                    );
                    

                    //Console.WriteLine($"RATIO: {ratio}");
                    //Console.WriteLine($"Page Height: {page.Height}");
                    //Console.WriteLine($"Page Width: {page.Width}");
                    //Console.WriteLine($"Image Height: {image.PixelHeight}");
                    //Console.WriteLine($"Image Width: {image.PixelWidth}");
                    //Console.WriteLine($"");
                    gfx.DrawImage(image, (page.Width - image.PixelWidth * ((float)page.Height / image.PixelHeight)) / 2, 0, ratio, page.Height);
                    
                    
                    //gfx.DrawImage(image, (page.Width - image.PixelWidth * ((float)page.Height / image.PixelHeight)) / 2, 0, image.PixelWidth * ((float)page.Height / image.PixelHeight), ratio, page.Height);
                    
                    
                    
                    //gfx.DrawImage(image,  (page.Width - image.PixelWidth * ((float)page.Height / image.PixelHeight)) / 2, 0, image.PixelWidth * ((float)page.Height / image.PixelHeight), page.Height); 
                }
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