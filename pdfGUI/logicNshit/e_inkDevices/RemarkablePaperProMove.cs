using System.Numerics;
using pdfGUI.e_inkDevices;

namespace pdfGUI.logicNshit.e_inkDevices;
public class RemarkablePaperProMove : devices
{
    public override Vector2 resolution
    {
        get;
        set;
    } = new Vector2(954, 1696);

    public override string name { get; set; } =  "Remarkable Paper Pro Move";
}