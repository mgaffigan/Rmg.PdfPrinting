using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct2D.Common;
using Windows.Win32.Graphics.DirectWrite;

namespace Rmg.PdfPrinting;

/// <summary>
/// Interceptor for attempts by PdfRendererNative to set antialiasing
/// </summary>
internal class OneBppDeviceContext : D2D1DeviceContextProxy
{
    public OneBppDeviceContext(ID2D1DeviceContext dc)
        : base(dc)
    {
        // nop
    }

    public override unsafe void DrawImage(ID2D1Image image, [Optional] D2D_POINT_2F* targetOffset, 
        [Optional] D2D_RECT_F* imageRectangle, D2D1_INTERPOLATION_MODE interpolationMode, D2D1_COMPOSITE_MODE compositeMode)
    {
        // Entire page seems to be wrapped in an ID2D1CommandList, unwrap
        if (targetOffset == null && imageRectangle == null
            && image is ID2D1CommandList cl)
        {
            // pass the original dc instead of this to avoid recursion
            cl.Stream(new OneBppCommandSinkProxy(dc));
        }
        else
        {
            base.DrawImage(image, targetOffset, imageRectangle, interpolationMode, compositeMode);
        }
    }

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
}
