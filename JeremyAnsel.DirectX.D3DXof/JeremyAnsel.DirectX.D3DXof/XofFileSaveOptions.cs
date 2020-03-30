using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    /// <summary>
    ///   This flag is used to specify where to save the file to. Each flag is
    ///   mutually exclusive, indicates the data location of the file, and also
    ///   chooses which additional data will specify the location.
    ///   _TOFILE is paired with a filename (LPCSTR)
    ///   _TOWFILE is paired with a filename (LPWSTR)
    /// </summary>
    internal enum XofFileSaveOptions
    {
        ToFile = 0,

        ToWFile = 1
    }
}
