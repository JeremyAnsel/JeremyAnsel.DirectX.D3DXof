using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XAnimationSet
    {
        public string Name { get; set; }

        public List<XAnimation> Animations { get; } = new List<XAnimation>();
    }
}
