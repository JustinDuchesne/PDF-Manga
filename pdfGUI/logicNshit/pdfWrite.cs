using System.Linq;
using System.Reflection;

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


    public PdfWrite()
    {
        _inpoot = "";
        _outpoot = "";
        _name = "";
        _device = e_inkDevice.Remarkable2;
    }
    
    public PdfWrite(string inpoot, string outpoot, string name, e_inkDevice device)
    {
        _inpoot = inpoot;
        _outpoot = outpoot;
        _name = name;
        _device = device;
    }

    public void setInpoot(string inpoot)
    {
        _inpoot = inpoot;
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
    
    
    public void Write()
    {
        PdfDocument document = new PdfDocument();
        
        DirectoryInfo monRep = new DirectoryInfo(@$"{_inpoot}");
        
        DirectoryInfo[] mesFichiers = monRep.GetDirectories()
            .OrderBy(d => d.Name)
            .ToArray();
        
        for (int i = 0; i < mesFichiers.Length; i++)
        {
            DirectoryInfo imageRep = new DirectoryInfo(mesFichiers[i].FullName);
            FileInfo[] mesImages = imageRep.GetFiles("*.jpg");
            for (int j = 0; j < mesImages.Length; j++)
            {
                double ratio = 0;
                bool rotate = false;
                PdfPage page = document.AddPage();

                page.Width = EInkDeviceResolution.GetResolution(_device).X;
                page.Height = EInkDeviceResolution.GetResolution(_device).Y;
                
                XGraphics gfx = XGraphics.FromPdfPage(page);
                Console.WriteLine($"{mesImages[j]}");
                XImage image = XImage.FromFile($"{mesImages[j]}");
                
                if (image.PixelHeight / image.PixelWidth < 0.9)
                    rotate = true;

                if (rotate)
                {
                    gfx.RotateTransform(90);

                    gfx.DrawImage(
                        image,
                        0,
                        -page.Width,
                        page.Height,
                        page.Width
                    );
                }
                else
                {
                    gfx.DrawImage(image, 0, 0, page.Width, page.Height);   
                }
            }
        }
        document.Save($"{_outpoot}/{_name}.pdf");
    }
}