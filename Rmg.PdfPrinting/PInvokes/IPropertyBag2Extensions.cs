using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.System.Com.StructuredStorage;

namespace Rmg.PdfPrinting
{
    internal static class IPropertyBag2Extensions
    {
        public static unsafe void SetProperty(this IPropertyBag2 bag, string name, object val)
        {
            fixed (char* pName = name)
            {
                bag.Write(1, new() { pstrName = pName }, val);
            }
        }
    }
}
