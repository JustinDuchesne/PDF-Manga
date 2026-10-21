using System.Numerics;
using pdfGUI.e_inkDevices;

namespace pdfGUI.logicNshit.e_inkDevices;
public class TabS9Fe : devices
{
    public override Vector2 resolution
    {
        get;
        set;
    } = new Vector2(2304, 1440);

    public override string name { get; set; } =  "Tab S9 Fe";
}