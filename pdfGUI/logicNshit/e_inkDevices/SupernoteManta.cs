using System.Numerics;
using pdfGUI.e_inkDevices;

namespace pdfGUI.logicNshit.e_inkDevices;
public class SupernoteManta : devices
{
    public override Vector2 resolution
    {
        get;
        set;
    } = new Vector2(1920, 2560);

    public override string name { get; set; } =  "SuperNote Manta";
}