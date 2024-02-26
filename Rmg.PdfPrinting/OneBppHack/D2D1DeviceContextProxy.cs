using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Dxgi;
using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Direct2D.Common;
using Windows.Win32.Graphics.Imaging;
using Windows.Win32.Graphics.DirectWrite;
using Windows.Win32.Graphics.Dxgi.Common;

namespace Rmg.PdfPrinting;

internal class D2D1DeviceContextProxy : ID2D1DeviceContext
{
    protected readonly ID2D1DeviceContext dc;

    public D2D1DeviceContextProxy(ID2D1DeviceContext dc)
    {
        this.dc = dc;
    }

    public virtual void GetFactory(out ID2D1Factory factory)
    {
        dc.GetFactory(out factory);
    }

    public virtual unsafe void CreateBitmap(D2D_SIZE_U size, [Optional] void* srcData, uint pitch, D2D1_BITMAP_PROPERTIES* bitmapProperties, out ID2D1Bitmap bitmap)
    {
        dc.CreateBitmap(size, srcData, pitch, bitmapProperties, out bitmap);
    }

    public virtual unsafe void CreateBitmapFromWicBitmap(IWICBitmapSource wicBitmapSource, [Optional] D2D1_BITMAP_PROPERTIES* bitmapProperties, out ID2D1Bitmap bitmap)
    {
        dc.CreateBitmapFromWicBitmap(wicBitmapSource, bitmapProperties, out bitmap);
    }

    public virtual unsafe void CreateSharedBitmap(Guid* riid, void* data, [Optional] D2D1_BITMAP_PROPERTIES* bitmapProperties, out ID2D1Bitmap bitmap)
    {
        dc.CreateSharedBitmap(riid, data, bitmapProperties, out bitmap);
    }

    public virtual unsafe void CreateBitmapBrush(ID2D1Bitmap bitmap, [Optional] D2D1_BITMAP_BRUSH_PROPERTIES* bitmapBrushProperties, [Optional] D2D1_BRUSH_PROPERTIES* brushProperties, out ID2D1BitmapBrush bitmapBrush)
    {
        dc.CreateBitmapBrush(bitmap, bitmapBrushProperties, brushProperties, out bitmapBrush);
    }

    public virtual unsafe void CreateSolidColorBrush(D2D1_COLOR_F* color, [Optional] D2D1_BRUSH_PROPERTIES* brushProperties, out ID2D1SolidColorBrush solidColorBrush)
    {
        dc.CreateSolidColorBrush(color, brushProperties, out solidColorBrush);
    }

    public virtual unsafe void CreateGradientStopCollection(D2D1_GRADIENT_STOP* gradientStops, uint gradientStopsCount, D2D1_GAMMA colorInterpolationGamma, D2D1_EXTEND_MODE extendMode, out ID2D1GradientStopCollection gradientStopCollection)
    {
        dc.CreateGradientStopCollection(gradientStops, gradientStopsCount, colorInterpolationGamma, extendMode, out gradientStopCollection);
    }

    public virtual unsafe void CreateLinearGradientBrush(D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES* linearGradientBrushProperties, [Optional] D2D1_BRUSH_PROPERTIES* brushProperties, ID2D1GradientStopCollection gradientStopCollection, out ID2D1LinearGradientBrush linearGradientBrush)
    {
        dc.CreateLinearGradientBrush(linearGradientBrushProperties, brushProperties, gradientStopCollection, out linearGradientBrush);
    }

    public virtual unsafe void CreateRadialGradientBrush(D2D1_RADIAL_GRADIENT_BRUSH_PROPERTIES* radialGradientBrushProperties, [Optional] D2D1_BRUSH_PROPERTIES* brushProperties, ID2D1GradientStopCollection gradientStopCollection, out ID2D1RadialGradientBrush radialGradientBrush)
    {
        dc.CreateRadialGradientBrush(radialGradientBrushProperties, brushProperties, gradientStopCollection, out radialGradientBrush);
    }

    public virtual unsafe void CreateCompatibleRenderTarget([Optional] D2D_SIZE_F* desiredSize, [Optional] D2D_SIZE_U* desiredPixelSize, [Optional] D2D1_PIXEL_FORMAT* desiredFormat, D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS options, out ID2D1BitmapRenderTarget bitmapRenderTarget)
    {
        dc.CreateCompatibleRenderTarget(desiredSize, desiredPixelSize, desiredFormat, options, out bitmapRenderTarget);
    }

    public virtual unsafe void CreateLayer([Optional] D2D_SIZE_F* size, out ID2D1Layer layer)
    {
        dc.CreateLayer(size, out layer);
    }

    public virtual void CreateMesh(out ID2D1Mesh mesh)
    {
        dc.CreateMesh(out mesh);
    }

    public virtual void DrawLine(D2D_POINT_2F point0, D2D_POINT_2F point1, ID2D1Brush brush, float strokeWidth, ID2D1StrokeStyle strokeStyle)
    {
        dc.DrawLine(point0, point1, brush, strokeWidth, strokeStyle);
    }

    public virtual unsafe void DrawRectangle(D2D_RECT_F* rect, ID2D1Brush brush, float strokeWidth, ID2D1StrokeStyle strokeStyle)
    {
        dc.DrawRectangle(rect, brush, strokeWidth, strokeStyle);
    }

    public virtual unsafe void FillRectangle(D2D_RECT_F* rect, ID2D1Brush brush)
    {
        dc.FillRectangle(rect, brush);
    }

    public virtual unsafe void DrawRoundedRectangle(D2D1_ROUNDED_RECT* roundedRect, ID2D1Brush brush, float strokeWidth, ID2D1StrokeStyle strokeStyle)
    {
        dc.DrawRoundedRectangle(roundedRect, brush, strokeWidth, strokeStyle);
    }

    public virtual unsafe void FillRoundedRectangle(D2D1_ROUNDED_RECT* roundedRect, ID2D1Brush brush)
    {
        dc.FillRoundedRectangle(roundedRect, brush);
    }

    public virtual unsafe void DrawEllipse(D2D1_ELLIPSE* ellipse, ID2D1Brush brush, float strokeWidth, ID2D1StrokeStyle strokeStyle)
    {
        dc.DrawEllipse(ellipse, brush, strokeWidth, strokeStyle);
    }

    public virtual unsafe void FillEllipse(D2D1_ELLIPSE* ellipse, ID2D1Brush brush)
    {
        dc.FillEllipse(ellipse, brush);
    }

    public virtual void DrawGeometry(ID2D1Geometry geometry, ID2D1Brush brush, float strokeWidth, ID2D1StrokeStyle strokeStyle)
    {
        dc.DrawGeometry(geometry, brush, strokeWidth, strokeStyle);
    }

    public virtual void FillGeometry(ID2D1Geometry geometry, ID2D1Brush brush, ID2D1Brush opacityBrush)
    {
        dc.FillGeometry(geometry, brush, opacityBrush);
    }

    public virtual void FillMesh(ID2D1Mesh mesh, ID2D1Brush brush)
    {
        dc.FillMesh(mesh, brush);
    }

    public virtual unsafe void FillOpacityMask(ID2D1Bitmap opacityMask, ID2D1Brush brush, D2D1_OPACITY_MASK_CONTENT content, [Optional] D2D_RECT_F* destinationRectangle, [Optional] D2D_RECT_F* sourceRectangle)
    {
        dc.FillOpacityMask(opacityMask, brush, content, destinationRectangle, sourceRectangle);
    }

    public virtual unsafe void DrawBitmap(ID2D1Bitmap bitmap, [Optional] D2D_RECT_F* destinationRectangle, float opacity, D2D1_BITMAP_INTERPOLATION_MODE interpolationMode, [Optional] D2D_RECT_F* sourceRectangle)
    {
        dc.DrawBitmap(bitmap, destinationRectangle, opacity, interpolationMode, sourceRectangle);
    }

    public virtual unsafe void DrawText(PCWSTR @string, uint stringLength, IDWriteTextFormat textFormat, D2D_RECT_F* layoutRect, ID2D1Brush defaultFillBrush, D2D1_DRAW_TEXT_OPTIONS options, DWRITE_MEASURING_MODE measuringMode)
    {
        dc.DrawText(@string, stringLength, textFormat, layoutRect, defaultFillBrush, options, measuringMode);
    }

    public virtual void DrawTextLayout(D2D_POINT_2F origin, IDWriteTextLayout textLayout, ID2D1Brush defaultFillBrush, D2D1_DRAW_TEXT_OPTIONS options)
    {
        dc.DrawTextLayout(origin, textLayout, defaultFillBrush, options);
    }

    public virtual void DrawGlyphRun(D2D_POINT_2F baselineOrigin, in DWRITE_GLYPH_RUN glyphRun, ID2D1Brush foregroundBrush, DWRITE_MEASURING_MODE measuringMode)
    {
        dc.DrawGlyphRun(baselineOrigin, glyphRun, foregroundBrush, measuringMode);
    }

    public virtual unsafe void SetTransform(D2D_MATRIX_3X2_F* transform)
    {
        dc.SetTransform(transform);
    }

    public virtual unsafe void GetTransform(D2D_MATRIX_3X2_F* transform)
    {
        dc.GetTransform(transform);
    }

    public virtual void SetAntialiasMode(D2D1_ANTIALIAS_MODE antialiasMode)
    {
        dc.SetAntialiasMode(antialiasMode);
    }

    public virtual D2D1_ANTIALIAS_MODE GetAntialiasMode()
    {
        return dc.GetAntialiasMode();
    }

    public virtual void SetTextAntialiasMode(D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode)
    {
        dc.SetTextAntialiasMode(textAntialiasMode);
    }

    public virtual D2D1_TEXT_ANTIALIAS_MODE GetTextAntialiasMode()
    {
        return dc.GetTextAntialiasMode();
    }

    public virtual void SetTextRenderingParams(IDWriteRenderingParams textRenderingParams)
    {
        dc.SetTextRenderingParams(textRenderingParams);
    }

    public virtual void GetTextRenderingParams(out IDWriteRenderingParams textRenderingParams)
    {
        dc.GetTextRenderingParams(out textRenderingParams);
    }

    public virtual void SetTags(ulong tag1, ulong tag2)
    {
        dc.SetTags(tag1, tag2);
    }

    public virtual unsafe void GetTags([Optional] ulong* tag1, [Optional] ulong* tag2)
    {
        dc.GetTags(tag1, tag2);
    }

    public virtual void PushLayer(in D2D1_LAYER_PARAMETERS layerParameters, ID2D1Layer layer)
    {
        dc.PushLayer(layerParameters, layer);
    }

    public virtual void PopLayer()
    {
        dc.PopLayer();
    }

    public virtual unsafe void Flush([Optional] ulong* tag1, [Optional] ulong* tag2)
    {
        dc.Flush(tag1, tag2);
    }

    public virtual void SaveDrawingState(ID2D1DrawingStateBlock drawingStateBlock)
    {
        dc.SaveDrawingState(drawingStateBlock);
    }

    public virtual void RestoreDrawingState(ID2D1DrawingStateBlock drawingStateBlock)
    {
        dc.RestoreDrawingState(drawingStateBlock);
    }

    public virtual unsafe void PushAxisAlignedClip(D2D_RECT_F* clipRect, D2D1_ANTIALIAS_MODE antialiasMode)
    {
        dc.PushAxisAlignedClip(clipRect, antialiasMode);
    }

    public virtual void PopAxisAlignedClip()
    {
        dc.PopAxisAlignedClip();
    }

    public virtual unsafe void Clear([Optional] D2D1_COLOR_F* clearColor)
    {
        dc.Clear(clearColor);
    }

    public virtual void BeginDraw()
    {
        dc.BeginDraw();
    }

    public virtual unsafe HRESULT EndDraw([Optional] ulong* tag1, [Optional] ulong* tag2)
    {
        return dc.EndDraw(tag1, tag2);
    }

    public virtual D2D1_PIXEL_FORMAT GetPixelFormat()
    {
        return dc.GetPixelFormat();
    }

    public virtual void SetDpi(float dpiX, float dpiY)
    {
        dc.SetDpi(dpiX, dpiY);
    }

    public virtual void GetDpi(out float dpiX, out float dpiY)
    {
        dc.GetDpi(out dpiX, out dpiY);
    }

    public virtual D2D_SIZE_F GetSize()
    {
        return dc.GetSize();
    }

    public virtual D2D_SIZE_U GetPixelSize()
    {
        return dc.GetPixelSize();
    }

    public virtual uint GetMaximumBitmapSize()
    {
        return dc.GetMaximumBitmapSize();
    }

    public virtual unsafe BOOL IsSupported(D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties)
    {
        return dc.IsSupported(renderTargetProperties);
    }

    public virtual unsafe void CreateBitmap(D2D_SIZE_U size, [Optional] void* sourceData, uint pitch, in D2D1_BITMAP_PROPERTIES1 bitmapProperties, out ID2D1Bitmap1 bitmap)
    {
        dc.CreateBitmap(size, sourceData, pitch, bitmapProperties, out bitmap);
    }

    public virtual void CreateBitmapFromWicBitmap(IWICBitmapSource wicBitmapSource, in D2D1_BITMAP_PROPERTIES1 bitmapProperties, out ID2D1Bitmap1 bitmap)
    {
        dc.CreateBitmapFromWicBitmap(wicBitmapSource, bitmapProperties, out bitmap);
    }

    public virtual unsafe void CreateColorContext(D2D1_COLOR_SPACE space, [Optional] byte* profile, uint profileSize, out ID2D1ColorContext colorContext)
    {
        dc.CreateColorContext(space, profile, profileSize, out colorContext);
    }

    public virtual void CreateColorContextFromFilename(PCWSTR filename, out ID2D1ColorContext colorContext)
    {
        dc.CreateColorContextFromFilename(filename, out colorContext);
    }

    public virtual void CreateColorContextFromWicColorContext(IWICColorContext wicColorContext, out ID2D1ColorContext colorContext)
    {
        dc.CreateColorContextFromWicColorContext(wicColorContext, out colorContext);
    }

    public virtual void CreateBitmapFromDxgiSurface(IDXGISurface surface, in D2D1_BITMAP_PROPERTIES1 bitmapProperties, out ID2D1Bitmap1 bitmap)
    {
        dc.CreateBitmapFromDxgiSurface(surface, bitmapProperties, out bitmap);
    }

    public virtual unsafe void CreateEffect(Guid* effectId, out ID2D1Effect effect)
    {
        dc.CreateEffect(effectId, out effect);
    }

    public virtual unsafe void CreateGradientStopCollection(D2D1_GRADIENT_STOP* straightAlphaGradientStops, uint straightAlphaGradientStopsCount, D2D1_COLOR_SPACE preInterpolationSpace, D2D1_COLOR_SPACE postInterpolationSpace, D2D1_BUFFER_PRECISION bufferPrecision, D2D1_EXTEND_MODE extendMode, D2D1_COLOR_INTERPOLATION_MODE colorInterpolationMode, out ID2D1GradientStopCollection1 gradientStopCollection1)
    {
        dc.CreateGradientStopCollection(straightAlphaGradientStops, straightAlphaGradientStopsCount, preInterpolationSpace, postInterpolationSpace, bufferPrecision, extendMode, colorInterpolationMode, out gradientStopCollection1);
    }

    public virtual unsafe void CreateImageBrush(ID2D1Image image, D2D1_IMAGE_BRUSH_PROPERTIES* imageBrushProperties, [Optional] D2D1_BRUSH_PROPERTIES* brushProperties, out ID2D1ImageBrush imageBrush)
    {
        dc.CreateImageBrush(image, imageBrushProperties, brushProperties, out imageBrush);
    }

    public virtual unsafe void CreateBitmapBrush(ID2D1Bitmap bitmap, [Optional] D2D1_BITMAP_BRUSH_PROPERTIES1* bitmapBrushProperties, [Optional] D2D1_BRUSH_PROPERTIES* brushProperties, out ID2D1BitmapBrush1 bitmapBrush)
    {
        dc.CreateBitmapBrush(bitmap, bitmapBrushProperties, brushProperties, out bitmapBrush);
    }

    public virtual void CreateCommandList(out ID2D1CommandList commandList)
    {
        dc.CreateCommandList(out commandList);
    }

    public virtual BOOL IsDxgiFormatSupported(DXGI_FORMAT format)
    {
        return dc.IsDxgiFormatSupported(format);
    }

    public virtual BOOL IsBufferPrecisionSupported(D2D1_BUFFER_PRECISION bufferPrecision)
    {
        return dc.IsBufferPrecisionSupported(bufferPrecision);
    }

    public virtual unsafe void GetImageLocalBounds(ID2D1Image image, D2D_RECT_F* localBounds)
    {
        dc.GetImageLocalBounds(image, localBounds);
    }

    public virtual unsafe void GetImageWorldBounds(ID2D1Image image, D2D_RECT_F* worldBounds)
    {
        dc.GetImageWorldBounds(image, worldBounds);
    }

    public virtual unsafe void GetGlyphRunWorldBounds(D2D_POINT_2F baselineOrigin, in DWRITE_GLYPH_RUN glyphRun, DWRITE_MEASURING_MODE measuringMode, D2D_RECT_F* bounds)
    {
        dc.GetGlyphRunWorldBounds(baselineOrigin, glyphRun, measuringMode, bounds);
    }

    public virtual void GetDevice(out ID2D1Device device)
    {
        dc.GetDevice(out device);
    }

    public virtual void SetTarget(ID2D1Image image)
    {
        dc.SetTarget(image);
    }

    public virtual void GetTarget(out ID2D1Image image)
    {
        dc.GetTarget(out image);
    }

    public virtual unsafe void SetRenderingControls(D2D1_RENDERING_CONTROLS* renderingControls)
    {
        dc.SetRenderingControls(renderingControls);
    }

    public virtual unsafe void GetRenderingControls(D2D1_RENDERING_CONTROLS* renderingControls)
    {
        dc.GetRenderingControls(renderingControls);
    }

    public virtual void SetPrimitiveBlend(D2D1_PRIMITIVE_BLEND primitiveBlend)
    {
        dc.SetPrimitiveBlend(primitiveBlend);
    }

    public virtual D2D1_PRIMITIVE_BLEND GetPrimitiveBlend()
    {
        return dc.GetPrimitiveBlend();
    }

    public virtual void SetUnitMode(D2D1_UNIT_MODE unitMode)
    {
        dc.SetUnitMode(unitMode);
    }

    public virtual D2D1_UNIT_MODE GetUnitMode()
    {
        return dc.GetUnitMode();
    }

    public virtual unsafe void DrawGlyphRun(D2D_POINT_2F baselineOrigin, in DWRITE_GLYPH_RUN glyphRun, [Optional] DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription, ID2D1Brush foregroundBrush, DWRITE_MEASURING_MODE measuringMode)
    {
        dc.DrawGlyphRun(baselineOrigin, glyphRun, glyphRunDescription, foregroundBrush, measuringMode);
    }

    public virtual unsafe void DrawImage(ID2D1Image image, [Optional] D2D_POINT_2F* targetOffset, [Optional] D2D_RECT_F* imageRectangle, D2D1_INTERPOLATION_MODE interpolationMode, D2D1_COMPOSITE_MODE compositeMode)
    {
        dc.DrawImage(image, targetOffset, imageRectangle, interpolationMode, compositeMode);
    }

    public virtual unsafe void DrawGdiMetafile(ID2D1GdiMetafile gdiMetafile, [Optional] D2D_POINT_2F* targetOffset)
    {
        dc.DrawGdiMetafile(gdiMetafile, targetOffset);
    }

    public virtual unsafe void DrawBitmap(ID2D1Bitmap bitmap, [Optional] D2D_RECT_F* destinationRectangle, float opacity, D2D1_INTERPOLATION_MODE interpolationMode, [Optional] D2D_RECT_F* sourceRectangle, [Optional] D2D_MATRIX_4X4_F* perspectiveTransform)
    {
        dc.DrawBitmap(bitmap, destinationRectangle, opacity, interpolationMode, sourceRectangle, perspectiveTransform);
    }

    public virtual void PushLayer(in D2D1_LAYER_PARAMETERS1 layerParameters, ID2D1Layer layer)
    {
        dc.PushLayer(layerParameters, layer);
    }

    public virtual unsafe void InvalidateEffectInputRectangle(ID2D1Effect effect, uint input, D2D_RECT_F* inputRectangle)
    {
        dc.InvalidateEffectInputRectangle(effect, input, inputRectangle);
    }

    public virtual void GetEffectInvalidRectangleCount(ID2D1Effect effect, out uint rectangleCount)
    {
        dc.GetEffectInvalidRectangleCount(effect, out rectangleCount);
    }

    public virtual unsafe void GetEffectInvalidRectangles(ID2D1Effect effect, D2D_RECT_F* rectangles, uint rectanglesCount)
    {
        dc.GetEffectInvalidRectangles(effect, rectangles, rectanglesCount);
    }

    public virtual unsafe void GetEffectRequiredInputRectangles(ID2D1Effect renderEffect, [Optional] D2D_RECT_F* renderImageRectangle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] D2D1_EFFECT_INPUT_DESCRIPTION[] inputDescriptions, D2D_RECT_F* requiredInputRects, uint inputCount)
    {
        dc.GetEffectRequiredInputRectangles(renderEffect, renderImageRectangle, inputDescriptions, requiredInputRects, inputCount);
    }

    public virtual unsafe void FillOpacityMask(ID2D1Bitmap opacityMask, ID2D1Brush brush, [Optional] D2D_RECT_F* destinationRectangle, [Optional] D2D_RECT_F* sourceRectangle)
    {
        dc.FillOpacityMask(opacityMask, brush, destinationRectangle, sourceRectangle);
    }
}
