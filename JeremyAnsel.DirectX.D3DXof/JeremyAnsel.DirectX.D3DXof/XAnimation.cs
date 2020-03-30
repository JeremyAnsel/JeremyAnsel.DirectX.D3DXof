using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XAnimation
    {
        public string Name { get; set; }

        public string FrameReference { get; set; }

        public List<XAnimationKey> Keys { get; } = new List<XAnimationKey>();

        public int OpenClosedOption { get; set; }

        public int PositionQualityOption { get; set; }
    }
}
