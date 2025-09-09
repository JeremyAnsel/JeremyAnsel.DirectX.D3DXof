using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    [SecurityCritical, SuppressUnmanagedCodeSecurity]
    [ComImport, Guid("cef08cfa-7b4f-4429-9624-2a690a933201")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IXofFileSaveObject
    {
        IXofFile GetFile();

        IXofFileSaveData AddDataObject(
            ref Guid template,
            [MarshalAs(UnmanagedType.LPStr)] string? name,
            IntPtr id,
            IntPtr size,
            IntPtr data);

        void Save();
    }
}
