using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XMesh
    {
        public string? Name { get; set; }

        public List<XVector> Vertices { get; } = new List<XVector>();

        public List<List<int>> FacesIndices { get; } = new List<List<int>>();

        public List<XMaterial> Materials { get; } = new List<XMaterial>();

        public List<int> MaterialsFacesIndices { get; } = new List<int>();

        public List<XVector> Normals { get; } = new List<XVector>();

        public List<List<int>> FacesNormalsIndices { get; } = new List<List<int>>();

        public List<XCoords2d> TextureCoords { get; } = new List<XCoords2d>();

        public int OriginalVerticesCount { get; set; }


        [SuppressMessage("Performance", "CA1819:Les propriétés ne doivent pas retourner de tableaux", Justification = "Reviewed.")]
        public int[]? VertexDuplicationIndices { get; set; }

        public List<Tuple<int, XColorRgba>> VertexColors { get; } = new List<Tuple<int, XColorRgba>>();

        public short MaxSkinWeightsPerVertex { get; set; }

        public short MaxSkinWeightsPerFace { get; set; }

        public short BonesCount { get; set; }

        public List<XSkinWeights> SkinWeights { get; } = new List<XSkinWeights>();

        public uint FVF { get; set; }

        public List<uint> FVFData { get; } = new List<uint>();

        public List<XVertexElement> VertexElements { get; } = new List<XVertexElement>();


        [SuppressMessage("Performance", "CA1819:Les propriétés ne doivent pas retourner de tableaux", Justification = "Reviewed.")]
        public uint[]? VertexElementsData { get; set; }

        public override string? ToString()
        {
            return this.Name;
        }
    }
}
