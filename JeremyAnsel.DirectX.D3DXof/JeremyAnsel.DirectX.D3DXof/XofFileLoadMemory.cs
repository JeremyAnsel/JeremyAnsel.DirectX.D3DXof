using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    internal struct XofFileLoadMemory
    {
        public IntPtr Memory;

        public IntPtr Size;

        public XofFileLoadMemory(IntPtr memory, IntPtr size)
        {
            this.Memory = memory;
            this.Size = size;
        }
    }
}
