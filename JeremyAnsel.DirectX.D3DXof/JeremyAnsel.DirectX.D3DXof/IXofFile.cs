using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    [SecurityCritical, SuppressUnmanagedCodeSecurity]
    [ComImport, Guid("cef08cf9-7b4f-4429-9624-2a690a933201")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IXofFile
    {
        IXofFileEnumObject CreateEnumObject(
            IntPtr source,
            XofFileLoadOptions option);

        IXofFileSaveObject CreateSaveObject(
            IntPtr data,
            XofFileSaveOptions option,
            XofFileFormats format);

        void RegisterTemplates(
            IntPtr data,
            IntPtr size);

        void RegisterEnumTemplates(
            IXofFileEnumObject enumObject);
    }
}
