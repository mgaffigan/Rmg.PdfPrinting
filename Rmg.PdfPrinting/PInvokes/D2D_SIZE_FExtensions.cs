using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows.Win32.Graphics.Direct2D.Common;

internal partial struct D2D_SIZE_F
{
    public static implicit operator D2D_SIZE_F(Windows.Foundation.Size size)
        => new D2D_SIZE_F() { width = (float)size.Width, height = (float)size.Height };
}
