using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    /// <summary>
    ///   This flag is used to specify what file type to use when saving to disk.
    ///   _BINARY, and _TEXT are mutually exclusive, while
    ///   _COMPRESSED is an optional setting that works with all file types.
    /// </summary>
    [Flags]
    public enum XofFileFormats
    {
        Binary = 0,

        Text = 1,

        Compressed = 2
    }
}
