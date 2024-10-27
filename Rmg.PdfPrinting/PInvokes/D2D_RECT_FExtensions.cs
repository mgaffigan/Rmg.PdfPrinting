namespace Windows.Win32.Graphics.Direct2D.Common;

internal partial struct D2D_RECT_F
{
    public D2D_RECT_F()
    {
        // nop
    }

    public D2D_RECT_F(float left, float top, float right, float bottom)
    {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
    }
}