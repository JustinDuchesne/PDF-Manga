using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace pdfGUI;
using System;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

using System.IO;

public class PdfWrite
{
    private string _inpoot { get; set; }
    private string _outpoot { get; set; }
    private string _name { get; set; }
    public e_inkDevice _device { get; set; }
    
    //For progress tracker (the little bar smile :)
    public int _currentProgress { get; set; } = 0;
    public int _numberFiles { get; set; } = 0;
    
    public float _percentage { get; set; } = 0;


    public PdfWrite()
    {
        _inpoot = "";
        _outpoot = "";
        _name = "";
        _device = e_inkDevice.Remarkable2;
        computeTotalDirs();
    }
    
    public PdfWrite(string inpoot, string outpoot, string name, e_inkDevice device)
    {
        _inpoot = inpoot;
        _outpoot = outpoot;
        _name = name;
        _device = device;
        computeTotalDirs();
    }

    public void setInpoot(string inpoot)
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
    public void setDevice(e_inkDevice device)
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
        progress.Report(Convert.ToInt32(0));
        PdfDocument document = new PdfDocument();
        DirectoryInfo monRep = new DirectoryInfo(@$"{_inpoot}");
        DirectoryInfo[] mesFichiers = monRep.GetDirectories()
                                            .OrderBy(d => d.Name)
                                            .ToArray();
        
        for (int i = 0; i < mesFichiers.Length; i++)
        {
            DirectoryInfo imageRep = new DirectoryInfo(mesFichiers[i].FullName);
            FileInfo[] mesImages = imageRep.GetFiles().Where(f => f.Extension == ".jpg" || f.Extension == ".jpeg" || f.Extension == ".png" || f.Extension == ".webp").ToArray();
            computeProgress(progress);
            for (int j = 0; j < mesImages.Length; j++)
            {
                bool rotate = false;
                PdfPage page = document.AddPage();

                page.Width = EInkDeviceResolution.GetResolution(_device).X;
                page.Height = EInkDeviceResolution.GetResolution(_device).Y;
                
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XImage image = XImage.FromFile($"{mesImages[j]}");
                //Console.WriteLine($"{mesImages[j]}");
                
                if (image.PixelHeight / image.PixelWidth < 0.9)
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
        DirectoryInfo monRep = new DirectoryInfo(@$"{_inpoot}");
        DirectoryInfo[] mesFichiers = monRep.GetDirectories()
            .OrderBy(d => d.Name)
            .ToArray();

        setNumberFiles(mesFichiers.Count());
    }

    public void computeProgress(IProgress<int> progress)
    {
        setCurrentProgress(_currentProgress + 1);
        _percentage = (float)_currentProgress / _numberFiles * 100;
        progress.Report(Convert.ToInt32(_percentage));
        
    }
}