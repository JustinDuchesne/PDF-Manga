using System.Numerics;
using pdfGUI.e_inkDevices;

namespace pdfGUI.logicNshit.e_inkDevices;
public class TabS9Fe : devices
{
    public override Vector2 resolution
    {
        get;
        set;
    } = new Vector2(1440, 2304);

    public override string name { get; set; } =  "Tab S9 Fe";
}