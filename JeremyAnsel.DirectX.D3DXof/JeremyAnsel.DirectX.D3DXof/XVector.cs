using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XVector
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2})", this.X, this.Y, this.Z);
        }
    }
}
