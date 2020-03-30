using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XFrame
    {
        public string Name { get; set; }

        public XMatrix4x4 TransformMatrix { get; set; }

        public XFrameCamera FrameCamera { get; set; }

        public List<XMesh> Meshes { get; } = new List<XMesh>();

        public List<XFrame> Frames { get; } = new List<XFrame>();

        public Dictionary<int, string> MeshesNames { get; } = new Dictionary<int, string>();

        public float CameraRotationScaler { get; set; } = 1.0f;

        public float CameraMoveScaler { get; set; } = 1.0f;
    }
}
