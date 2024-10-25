using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows.Win32.Graphics.Direct2D.Common;

internal partial struct D2D_SIZE_U
{
    public static implicit operator D2D_SIZE_U(Windows.Foundation.Size size)
        => new D2D_SIZE_U() { width = (uint)size.Width, height = (uint)size.Height };

    public D2D_SIZE_U()
    {
    }

    public D2D_SIZE_U(uint width, uint height)
    {
        this.width = width;
        this.height = height;
    }
}
