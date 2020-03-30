using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XSkinWeights
    {
        public string Name { get; set; }

        public string TransformNodeName { get; set; }

        public List<int> VertexIndices { get; } = new List<int>();

        public List<float> Weights { get; } = new List<float>();

        public XMatrix4x4 MatrixOffset { get; set; }
    }
}
