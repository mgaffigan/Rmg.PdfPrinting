using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct2D.Common;

namespace Rmg.PdfPrinting;

internal class UpscalingComandSinkProxy(ID2D1DeviceContext dc, Func<ID2D1Bitmap, D2D_SIZE_U, D2D_RECT_F, ID2D1Bitmap> rescale) : CommandSinkToDeviceContextProxy(dc)
{
    public override unsafe void DrawBitmap(ID2D1Bitmap bitmap, [Optional] D2D_RECT_F* destinationRectangle, float opacity, D2D1_INTERPOLATION_MODE interpolationMode, [Optional] D2D_RECT_F* sourceRectangle, [Optional] D2D_MATRIX_4X4_F* perspectiveTransform)
    {
        // Work around XPS not being able to handle nearest neighbor
        if (interpolationMode == D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_NEAREST_NEIGHBOR
            // We don't support perspective transforms
            && perspectiveTransform == null)
        {
            DrawRescaledBitmap(bitmap, destinationRectangle, opacity, sourceRectangle);
        }
        else
        {
            base.DrawBitmap(bitmap, destinationRectangle, opacity, interpolationMode, sourceRectangle, perspectiveTransform);
        }
    }

    private unsafe void DrawRescaledBitmap(ID2D1Bitmap bitmap, D2D_RECT_F* destinationRectangle, float opacity, D2D_RECT_F* sourceRectangle)
    {
        // *screams in calling convention* https://github.com/microsoft/CsWin32/issues/167
        var bmpEx = (ID2D1BitmapEx)bitmap;
        var pxSize = bmpEx.GetPixelSizeEx();
        var size = bmpEx.GetSizeEx();

        // Size in drawing world the destinationRect or defaults to the bitmap size
        var widthX = size.width;
        var widthY = size.height;
        if (destinationRectangle != null)
        {
            widthX = destinationRectangle->right - destinationRectangle->left;
            widthY = destinationRectangle->bottom - destinationRectangle->top;
        }

        // Adjust for transform to render target units
        D2D_MATRIX_3X2_F transform = default;
        dc.GetTransform(&transform);
        widthX *= transform._11;
        widthY *= transform._22;

        // Adjust for DPI
        dc.GetDpi(out var dpiX, out var dpiY);
        widthX *= dpiX / 96f;
        widthY *= dpiY / 96f;

        // Find how many source pixels are being used
        var pxWidth = pxSize.width;
        var pxHeight = pxSize.height;
        D2D_RECT_F sourceRect = new(0, 0, size.width, size.height);
        if (sourceRectangle != null) sourceRect = *sourceRectangle;
        if (!(sourceRect.left == 0 && sourceRect.top == 0 && sourceRect.right == size.width && sourceRect.bottom == size.height))
        {
            // We're only using a portion of the image, and thus a portion of the pixels
            pxWidth *= (uint)((sourceRect.right - sourceRect.left) / size.width);
            pxHeight *= (uint)((sourceRect.bottom - sourceRect.top) / size.height);
        }

        // If we're scaling source pixels by more than 2x, rescale the bitmap
        var scaleX = widthX / pxWidth;
        var scaleY = widthY / pxHeight;
        var scale = Math.Max(scaleX, scaleY);
        if (scale > 2f)
        {
            var newPxSize = new D2D_SIZE_U((uint)widthX, (uint)widthY);
            bitmap = rescale(bitmap, newPxSize, sourceRect);
            var newWorldSize = ((ID2D1BitmapEx)bitmap).GetSizeEx();
            sourceRect = new(0, 0, newWorldSize.width, newWorldSize.height);
        }

        base.DrawBitmap(bitmap, destinationRectangle, opacity, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_NEAREST_NEIGHBOR, &sourceRect, null);
    }
}