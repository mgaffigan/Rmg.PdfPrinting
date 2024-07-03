// edited Write MarshalAs to use VARIANT handling
// https://learn.microsoft.com/en-us/dotnet/framework/interop/default-marshalling-for-objects#marshalling-system-types-to-variant

#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981
using global::System;
using global::System.Diagnostics;
using global::System.Diagnostics.CodeAnalysis;
using global::System.Runtime.CompilerServices;
using global::System.Runtime.InteropServices;
using global::System.Runtime.Versioning;
using winmdroot = global::Windows.Win32;
namespace Windows.Win32
{
    namespace System.Com.StructuredStorage
    {
        [Guid("22F55882-280B-11D0-A8A9-00A0C90C2004"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport()]
        internal interface IPropertyBag2
        {
            void _VtblGap0_1();

            unsafe void Write(int cProperties, in PROPBAG2 pPropBag, in object pvarValue);

            void _VtblGap1_3();
        }
    }
}
