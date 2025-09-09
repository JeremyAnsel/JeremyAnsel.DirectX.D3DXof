using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    [SecurityCritical, SuppressUnmanagedCodeSecurity]
    [ComImport, Guid("cef08cfd-7b4f-4429-9624-2a690a933201")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IXofFileData
    {
        IXofFileEnumObject GetEnum();

        void GetName(
            IntPtr name,
            IntPtr size);

        Guid GetId();

        void Lock(
            IntPtr size,
            IntPtr data);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool Unlock();

        Guid GetTemplateType();

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool IsReference();

        IntPtr GetChildren();

        IXofFileData? GetChild(
            IntPtr id);
    }
}
