using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Dxgi;
using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Direct2D.Common;
using Windows.Win32.Graphics.Imaging;
using Windows.Win32.Graphics.DirectWrite;
using Windows.Win32.Graphics.Dxgi.Common;

namespace Rmg.PdfPrinting
{
    internal class CommandSinkToDeviceContextProxy : ID2D1CommandSink
    {
        protected readonly ID2D1DeviceContext dc;

        public CommandSinkToDeviceContextProxy(ID2D1DeviceContext dc)
        {
            this.dc = dc;
        }

        public virtual void BeginDraw()
        {
            // nop
        }
        public virtual void EndDraw()
        {
            // nop
        }

        public virtual void SetAntialiasMode(D2D1_ANTIALIAS_MODE antialiasMode)
        {
            dc.SetAntialiasMode(antialiasMode);
        }

        public virtual void SetTags(ulong tag1, ulong tag2)
        {
            dc.SetTags(tag1, tag2);
        }

        public virtual void SetTextAntialiasMode(D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode)
        {
            dc.SetTextAntialiasMode(textAntialiasMode);
        }

        public virtual void SetTextRenderingParams(IDWriteRenderingParams textRenderingParams)
        {
            dc.SetTextRenderingParams(textRenderingParams);
        }

        public virtual unsafe void SetTransform(D2D_MATRIX_3X2_F* transform)
        {
            dc.SetTransform(transform);
        }

        public virtual void SetPrimitiveBlend(D2D1_PRIMITIVE_BLEND primitiveBlend)
        {
            dc.SetPrimitiveBlend(primitiveBlend);
        }

        public virtual void SetUnitMode(D2D1_UNIT_MODE unitMode)
        {
            dc.SetUnitMode(unitMode);
        }

        public virtual unsafe void Clear([Optional] D2D1_COLOR_F* color)
        {
            dc.Clear(color);
        }

        public virtual unsafe void DrawGlyphRun(D2D_POINT_2F baselineOrigin, in DWRITE_GLYPH_RUN glyphRun, [Optional] DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription, ID2D1Brush foregroundBrush, DWRITE_MEASURING_MODE measuringMode)
        {
            dc.DrawGlyphRun(baselineOrigin, glyphRun, glyphRunDescription, foregroundBrush, measuringMode);
        }

        public virtual void DrawLine(D2D_POINT_2F point0, D2D_POINT_2F point1, ID2D1Brush brush, float strokeWidth, ID2D1StrokeStyle strokeStyle)
        {
            dc.DrawLine(point0, point1, brush, strokeWidth, strokeStyle);
        }

        public virtual void DrawGeometry(ID2D1Geometry geometry, ID2D1Brush brush, float strokeWidth, ID2D1StrokeStyle strokeStyle)
        {
            dc.DrawGeometry(geometry, brush, strokeWidth, strokeStyle);
        }

        public virtual unsafe void DrawRectangle(D2D_RECT_F* rect, ID2D1Brush brush, float strokeWidth, ID2D1StrokeStyle strokeStyle)
        {
            dc.DrawRectangle(rect, brush, strokeWidth, strokeStyle);
        }

        public virtual unsafe void DrawBitmap(ID2D1Bitmap bitmap, [Optional] D2D_RECT_F* destinationRectangle, float opacity, D2D1_INTERPOLATION_MODE interpolationMode, [Optional] D2D_RECT_F* sourceRectangle, [Optional] D2D_MATRIX_4X4_F* perspectiveTransform)
        {
            dc.DrawBitmap(bitmap, destinationRectangle, opacity, interpolationMode, sourceRectangle, perspectiveTransform);
        }

        public virtual unsafe void DrawImage(ID2D1Image image, [Optional] D2D_POINT_2F* targetOffset, [Optional] D2D_RECT_F* imageRectangle, D2D1_INTERPOLATION_MODE interpolationMode, D2D1_COMPOSITE_MODE compositeMode)
        {
            dc.DrawImage(image, targetOffset, imageRectangle, interpolationMode, compositeMode);
        }

        public virtual unsafe void DrawGdiMetafile(ID2D1GdiMetafile gdiMetafile, [Optional] D2D_POINT_2F* targetOffset)
        {
            dc.DrawGdiMetafile(gdiMetafile, targetOffset);
        }

        public virtual void FillMesh(ID2D1Mesh mesh, ID2D1Brush brush)
        {
            dc.FillMesh(mesh, brush);
        }

        public virtual unsafe void FillOpacityMask(ID2D1Bitmap opacityMask, ID2D1Brush brush, [Optional] D2D_RECT_F* destinationRectangle, [Optional] D2D_RECT_F* sourceRectangle)
        {
            dc.FillOpacityMask(opacityMask, brush, destinationRectangle, sourceRectangle);
        }

        public virtual void FillGeometry(ID2D1Geometry geometry, ID2D1Brush brush, ID2D1Brush opacityBrush)
        {
            dc.FillGeometry(geometry, brush, opacityBrush);
        }

        public virtual unsafe void FillRectangle(D2D_RECT_F* rect, ID2D1Brush brush)
        {
            dc.FillRectangle(rect, brush);
        }

        public virtual unsafe void PushAxisAlignedClip(D2D_RECT_F* clipRect, D2D1_ANTIALIAS_MODE antialiasMode)
        {
            dc.PushAxisAlignedClip(clipRect, antialiasMode);
        }

        public virtual void PushLayer(in D2D1_LAYER_PARAMETERS1 layerParameters1, ID2D1Layer layer)
        {
            dc.PushLayer(layerParameters1, layer);
        }

        public virtual void PopAxisAlignedClip()
        {
            dc.PopAxisAlignedClip();
        }

        public virtual void PopLayer()
        {
            dc.PopLayer();
        }
    }
}
