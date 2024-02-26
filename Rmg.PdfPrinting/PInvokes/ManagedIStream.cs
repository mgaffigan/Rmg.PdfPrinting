using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace Rmg.PdfPrinting
{
    // Based on https://source.dot.net/#PresentationFramework/MS/Internal/IO/Packaging/ManagedIStream.cs
    internal class ManagedIStream : IStream
    {
        private readonly Stream _ioStream;

        public ManagedIStream(Stream ioStream)
        {
            _ioStream = ioStream ?? throw new ArgumentNullException(nameof(ioStream));
        }

        public unsafe HRESULT Read(void* pv, uint cb, uint* pcbRead)
        {
            var proxy = new byte[cb];
            var result = _ioStream.Read(proxy, 0, (int)cb);
            if (pcbRead != null)
            {
                *pcbRead = (uint)result;
            }
            Marshal.Copy(proxy, 0, (nint)pv, (int)*pcbRead);
            return result == cb ? HRESULT.S_OK : HRESULT.S_FALSE;
        }

        public unsafe HRESULT Write(void* pv, uint cb, uint* pcbWritten)
        {
            var proxy = new byte[cb];
            Marshal.Copy((nint)pv, proxy, 0, proxy.Length);
            _ioStream.Write(proxy, 0, proxy.Length);
            if (pcbWritten != null)
            {
                *pcbWritten = cb;
            }
            return HRESULT.S_OK;
        }

        unsafe void IStream.Seek(long dlibMove, SeekOrigin dwOrigin, ulong* plibNewPosition)
        {
            var result = (ulong)_ioStream.Seek(dlibMove, dwOrigin);
            if (plibNewPosition != null)
            {
                *plibNewPosition = result;
            }
        }

        void IStream.SetSize(ulong libNewSize)
        {
            _ioStream.SetLength((long)libNewSize);
        }

        unsafe void IStream.Stat(STATSTG* pstatstg, STATFLAG grfStatFlag)
        {
            pstatstg->type = 2 /* STGTY_STREAM */;
            pstatstg->cbSize = (ulong)_ioStream.Length;
            // Don't need to check CanRead, since STGM_READ == 0
            pstatstg->grfMode = _ioStream.CanWrite ? STGM.STGM_READWRITE : STGM.STGM_READ;
        }

        unsafe void IStream.CopyTo(IStream pstm, ulong cb, ulong* pcbRead, ulong* pcbWritten) => throw new NotSupportedException();

        void IStream.Commit(STGC grfCommitFlags) => throw new NotSupportedException();

        void IStream.Revert() => throw new NotSupportedException();

        void IStream.LockRegion(ulong libOffset, ulong cb, LOCKTYPE dwLockType) => throw new NotSupportedException();

        void IStream.UnlockRegion(ulong libOffset, ulong cb, uint dwLockType) => throw new NotSupportedException();

        void IStream.Clone(out IStream ppstm) => throw new NotSupportedException();
    }
}
