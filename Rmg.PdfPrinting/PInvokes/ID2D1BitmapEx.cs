using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Direct2D.Common;

namespace Windows.Win32.Graphics.Direct2D;

// *screams*
// https://github.com/microsoft/CsWin32/issues/167
[Guid("A2296057-EA42-4099-983B-539FB6505426"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport()]
internal unsafe interface ID2D1BitmapEx : ID2D1Bitmap
{
    void _VtblGap_1();

    [PreserveSig()]
    D2D_SIZE_F* GetSize(D2D_SIZE_F* result);

    [PreserveSig()]
    D2D_SIZE_U* GetPixelSize(D2D_SIZE_U* result);
}

internal static class ID2D1BitmapExExtensions
{
    public static unsafe D2D_SIZE_F GetSizeEx(this ID2D1BitmapEx bitmap)
    {
        D2D_SIZE_F result = default;
        bitmap.GetSize(&result);
        return result;
    }

    public static unsafe D2D_SIZE_U GetPixelSizeEx(this ID2D1BitmapEx bitmap)
    {
        D2D_SIZE_U result = default;
        bitmap.GetPixelSize(&result);
        return result;
    }
}