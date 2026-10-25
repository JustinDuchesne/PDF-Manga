using System.Numerics;
using pdfGUI.e_inkDevices;

namespace pdfGUI.logicNshit.e_inkDevices;
public class Remarkable2 : devices
{
    public override Vector2 resolution
    {
        get;
        set;
    } = new Vector2(1404, 1872);

    public override string name { get; set; } =  "Remarkable 2";
}