using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XAnimationKey
    {
        public string Name { get; set; }

        public XAnimationKeyType KeyType { get; set; }

        public List<Tuple<int, float[]>> Keys { get; } = new List<Tuple<int, float[]>>();
    }
}
