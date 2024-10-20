using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct2D.Common;
using Windows.Win32.Graphics.DirectWrite;

namespace Rmg.PdfPrinting;

internal class OneBppCommandSinkProxy(ID2D1DeviceContext dc) : CommandSinkToDeviceContextProxy(dc)
{
    public override void SetTextAntialiasMode(D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode)
    {
        // nop, want 1bpp
    }

    public override void SetTextRenderingParams(IDWriteRenderingParams textRenderingParams)
    {
        // nop, want 1bpp
    }

    public override void SetAntialiasMode(D2D1_ANTIALIAS_MODE antialiasMode)
    {
        // nop, want 1bpp
    }

    public override unsafe void Clear([Optional] D2D1_COLOR_F* color)
    {
        // nop - should only apply to the image, but applies to the whole DC if forwarded
    }
}