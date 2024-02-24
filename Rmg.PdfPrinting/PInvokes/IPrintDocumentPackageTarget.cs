// Based on CSWin32 but specifying type of ppvTarget

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
    namespace Storage.Xps.Printing
    {
        [Guid("1B8EFEC4-3019-4C27-964E-367202156906"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport()]
        [SupportedOSPlatform("windows8.0")]
        [global::System.CodeDom.Compiler.GeneratedCode("Microsoft.Windows.CsWin32", "0.3.49-beta+91f5c15987")]
        internal interface IPrintDocumentPackageTarget
        {
            /// <summary>Enumerates the supported target types.</summary>
            /// <param name="targetCount">The number of supported target types.</param>
            /// <param name="targetTypes">The array of supported target types. An array of GUIDs.</param>
            /// <returns>If the <b>GetPackageTargetTypes</b> method completes successfully, it returns an S_OK. Otherwise it returns the appropriate HRESULT error code.</returns>
            /// <remarks>In the case of a multi-format driver, the first GUID returned in the <i>targetTypes</i> array is the XPS format preferred by the driver.</remarks>
            unsafe void GetPackageTargetTypes(out uint targetCount, global::System.Guid** targetTypes);

            /// <summary>Retrieves the pointer to the specific document package target, which allows the client to add a document with the given target type. Clients can call this method multiple times but they always have to use the same target ID.</summary>
            /// <param name="guidTargetType">The target type GUID obtained from <a href="https://docs.microsoft.com/windows/desktop/api/documenttarget/nf-documenttarget-iprintdocumentpackagetarget-getpackagetargettypes">GetPackageTargetTypes</a>.</param>
            /// <param name="riid">The identifier of the interface being requested.</param>
            /// <param name="ppvTarget">The requested document target interface. The returned pointer is a pointer to an <a href="https://docs.microsoft.com/windows/desktop/api/xpsobjectmodel_1/nn-xpsobjectmodel_1-ixpsdocumentpackagetarget">IXpsDocumentPackageTarget</a> interface.</param>
            /// <returns>If the <b>GetPackageTarget</b> method completes successfully, it returns an S_OK. Otherwise it returns the appropriate HRESULT error code.</returns>
            /// <remarks>
            /// <para><see href="https://learn.microsoft.com/windows/win32/api/documenttarget/nf-documenttarget-iprintdocumentpackagetarget-getpackagetarget">Learn more about this API from docs.microsoft.com</see>.</para>
            /// </remarks>
            unsafe void GetPackageTarget(global::System.Guid* guidTargetType, global::System.Guid* riid, nint** ppvTarget);

            /// <summary>Cancels the current print job.</summary>
            /// <returns>If the <b>Cancel</b> method completes successfully, it returns an S_OK. Otherwise it returns the appropriate HRESULT error code.</returns>
            /// <remarks>
            /// <para><see href="https://learn.microsoft.com/windows/win32/api/documenttarget/nf-documenttarget-iprintdocumentpackagetarget-cancel">Learn more about this API from docs.microsoft.com</see>.</para>
            /// </remarks>
            void Cancel();
        }
    }
}
