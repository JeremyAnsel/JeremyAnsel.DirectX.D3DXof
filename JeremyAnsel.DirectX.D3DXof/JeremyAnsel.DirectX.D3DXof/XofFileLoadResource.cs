using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    internal struct XofFileLoadResource
    {
        public IntPtr Module;

        public IntPtr Name;

        public IntPtr Type;

        public XofFileLoadResource(IntPtr module, IntPtr name, IntPtr type)
        {
            this.Module = module;
            this.Name = name;
            this.Type = type;
        }
    }
}
