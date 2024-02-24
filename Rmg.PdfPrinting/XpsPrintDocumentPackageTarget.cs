using System.Runtime.InteropServices;
using Windows.Win32.Storage.Packaging.Opc;
using Windows.Win32.Storage.Xps;
using Windows.Win32.Storage.Xps.Printing;
using Windows.Win32;
using Windows.Win32.System.Com;

namespace Rmg.PdfPrinting;

internal class XpsPrintDocumentPackageTarget : IPrintDocumentPackageTarget, IXpsDocumentPackageTarget
{
    private readonly IStream Stream;
    private readonly IXpsOMObjectFactory Factory;

    public XpsPrintDocumentPackageTarget(IStream stream)
    {
        this.Stream = stream;
        this.Factory = (IXpsOMObjectFactory)new XpsOMObjectFactory();
    }

    unsafe void IPrintDocumentPackageTarget.GetPackageTargetTypes(out uint targetCount, Guid** targetTypes)
    {
        targetCount = 1;
        var tt = Marshal.AllocCoTaskMem(Marshal.SizeOf<Guid>());
        Marshal.StructureToPtr(PInvoke.ID_DOCUMENTPACKAGETARGET_MSXPS, tt, false);
        *targetTypes = (Guid*)tt;
    }

    unsafe void IPrintDocumentPackageTarget.GetPackageTarget(Guid* guidTargetType, Guid* riid, nint** ppvTarget)
    {
        if (*guidTargetType == PInvoke.ID_DOCUMENTPACKAGETARGET_MSXPS && *riid == typeof(IXpsDocumentPackageTarget).GUID)
        {
            *ppvTarget = (nint*)Marshal.GetComInterfaceForObject<XpsPrintDocumentPackageTarget, IXpsDocumentPackageTarget>(this);
        }
        else
        {
            // E_NOINTERFACE
            throw new InvalidCastException();
        }
    }

    void IPrintDocumentPackageTarget.Cancel()
    {
        // nop
    }

    IXpsOMPackageWriter IXpsDocumentPackageTarget.GetXpsOMPackageWriter(IOpcPartUri documentSequencePartName, IOpcPartUri discardControlPartName)
    {
        return Factory.CreatePackageWriterOnStream(Stream, false, XPS_INTERLEAVING.XPS_INTERLEAVING_OFF, documentSequencePartName,
            null, null, null, discardControlPartName);
    }

    IXpsOMObjectFactory IXpsDocumentPackageTarget.GetXpsOMFactory()
    {
        return Factory;
    }

    XPS_DOCUMENT_TYPE IXpsDocumentPackageTarget.GetXpsType()
    {
        return XPS_DOCUMENT_TYPE.XPS_DOCUMENT_TYPE_XPS;
    }
}
