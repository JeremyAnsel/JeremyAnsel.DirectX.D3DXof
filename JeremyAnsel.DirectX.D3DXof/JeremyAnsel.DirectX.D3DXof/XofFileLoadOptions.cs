using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    /// <summary>
    //   This flag is used to specify where to load the file from. Each flag is
    //   mutually exclusive, indicates the data location of the file, and also
    //   chooses which additional data will specify the location.
    //   _FROMFILE is paired with a filename (LPCSTR)
    //   _FROMWFILE is paired with a filename (LPWSTR)
    //   _FROMRESOURCE is paired with a (D3DXF_FILELOADRESOUCE*) description.
    //   _FROMMEMORY is paired with a (D3DXF_FILELOADMEMORY*) description.
    /// </summary>
    internal enum XofFileLoadOptions
    {
        FromFile = 0,

        FromWFile = 1,

        FromResource = 2,

        FromMemory = 3
    }
}
