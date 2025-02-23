﻿using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct2D.Common;

namespace Rmg.PdfPrinting;

internal class UpscalingDeviceContext(ID2D1DeviceContext dc, Func<ID2D1CommandList, ID2D1CommandList> imageFilter) : D2D1DeviceContextProxy(dc)
{
    public override unsafe void DrawImage(ID2D1Image image, [Optional] D2D_POINT_2F* targetOffset, 
        [Optional] D2D_RECT_F* imageRectangle, D2D1_INTERPOLATION_MODE interpolationMode, D2D1_COMPOSITE_MODE compositeMode)
    {
        // Entire page seems to be wrapped in an ID2D1CommandList, unwrap
        if (targetOffset == null && imageRectangle == null
            && image is ID2D1CommandList cl)
        {
            // buffer to newCl with a new device context
            var newCl = imageFilter(cl);

            // draw newCl using the proxy
            dc.DrawImage(newCl, targetOffset, imageRectangle, interpolationMode, compositeMode);
        }
        else
        {
            base.DrawImage(image, targetOffset, imageRectangle, interpolationMode, compositeMode);
        }
    }
}
