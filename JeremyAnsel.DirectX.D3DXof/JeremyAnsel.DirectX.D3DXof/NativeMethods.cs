using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    [SecurityCritical, SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        [DllImport("d3dx9_43.dll", EntryPoint = "D3DXFileCreate", PreserveSig = false)]
        public static extern void D3DXFileCreate(
            out IXofFile xfile);
    }
}
