﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XColorRgba
    {
        public float Red { get; set; }

        public float Green { get; set; }

        public float Blue { get; set; }

        public float Alpha { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", this.Red, this.Green, this.Blue, this.Alpha);
        }
    }
}
