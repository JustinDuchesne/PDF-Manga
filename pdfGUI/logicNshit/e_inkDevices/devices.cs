using System.Numerics;

namespace pdfGUI.e_inkDevices;

public abstract class devices
{
    public abstract Vector2 resolution { get; set; }
    public abstract string name { get; set; }

}