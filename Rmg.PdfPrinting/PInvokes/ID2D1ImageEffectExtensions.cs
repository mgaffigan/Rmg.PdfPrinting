using Windows.Win32.Graphics.Direct2D;

namespace Rmg.PdfPrinting;

internal static class ID2D1ImageEffectExtensions
{
    public static void SetValue(this ID2D1Effect effect, D2D1_DISCRETETRANSFER_PROP prop, float[] table)
    {
        // C:\Program Files (x86)\Windows Kits\10\Include\10.0.26100.0\um\d2d1_1.h:1246
        // https://stackoverflow.com/questions/4635769/how-do-i-convert-an-array-of-floats-to-a-byte-and-back
        var bTable = new byte[table.Length * sizeof(float)];
        Buffer.BlockCopy(table, 0, bTable, 0, bTable.Length);
        effect.SetValue((uint)prop, D2D1_PROPERTY_TYPE.D2D1_PROPERTY_TYPE_UNKNOWN, bTable, (uint)bTable.Length);
    }
}