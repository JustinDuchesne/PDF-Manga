using System.Numerics;
using pdfGUI.e_inkDevices;

namespace pdfGUI.logicNshit.e_inkDevices;
public class KindleScribeColorsoft : devices
{
    public override Vector2 resolution
    {
        get;
        set;
    } = new Vector2(1980, 2640);

    public override string name { get; set; } =  "Kindle Scribe Colorsoft";
}