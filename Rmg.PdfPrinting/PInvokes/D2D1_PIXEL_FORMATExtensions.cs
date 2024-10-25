using Windows.Win32.Graphics.Dxgi.Common;

namespace Windows.Win32.Graphics.Direct2D.Common;

internal partial struct D2D1_PIXEL_FORMAT
{
    public D2D1_PIXEL_FORMAT()
    {
    }

    public D2D1_PIXEL_FORMAT(DXGI_FORMAT format, D2D1_ALPHA_MODE alphaMode)
    {
        this.format = format;
        this.alphaMode = alphaMode;
    }
}
