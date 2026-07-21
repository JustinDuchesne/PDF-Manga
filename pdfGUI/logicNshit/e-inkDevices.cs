using System.Numerics;

namespace pdfGUI;

public enum e_inkDevice
{
    Remarkable2
}

public static class EInkDeviceResolution
{
    public static Vector2 GetResolution(this e_inkDevice device)
    {
        Vector2 resolution;
        switch (device)
        {
            case e_inkDevice.Remarkable2:
                resolution = new Vector2(1404, 1872);
                break;
            default:
                resolution = new Vector2(1, 1);
                break;
                
        };
        return resolution;
    }
}