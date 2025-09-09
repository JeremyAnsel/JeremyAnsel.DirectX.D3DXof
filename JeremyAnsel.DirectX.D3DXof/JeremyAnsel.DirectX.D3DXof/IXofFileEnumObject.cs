using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    [SecurityCritical, SuppressUnmanagedCodeSecurity]
    [ComImport, Guid("cef08cfc-7b4f-4429-9624-2a690a933201")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IXofFileEnumObject
    {
        IXofFile? GetFile();

        IntPtr GetChildren();

        IXofFileData? GetChild(
            IntPtr id);

        IXofFileData? GetDataObjectById(
            ref Guid id);

        IXofFileData? GetDataObjectByName(
            [MarshalAs(UnmanagedType.LPStr)] string name);
    }
}
