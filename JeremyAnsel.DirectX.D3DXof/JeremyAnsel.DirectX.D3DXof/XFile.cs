using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XFile
    {
        public Version FileVersion { get; set; }

        public int FileFlags { get; set; }

        public List<XMaterial> Materials { get; } = new List<XMaterial>();

        public List<XMesh> Meshes { get; } = new List<XMesh>();

        public List<XFrame> Frames { get; } = new List<XFrame>();

        public List<XAnimationSet> AnimationSets { get; } = new List<XAnimationSet>();

        public int AnimTicksPerSecond { get; set; }

        public static XFile FromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            return XFileReader.ReadFile(fileName);
        }

        public static XFile FromBytes(byte[] fileBytes)
        {
            if (fileBytes == null)
            {
                throw new ArgumentNullException(nameof(fileBytes));
            }

            if (fileBytes.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fileBytes));
            }

            return XFileReader.ReadFile(fileBytes);
        }

        public static XFile FromStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);

                bytes = ms.ToArray();
            }

            return XFile.FromBytes(bytes);
        }

        public void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            Save(fileName, XofFileFormats.Binary);
        }

        public void Save(string fileName, XofFileFormats format)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            XFileWriter.WriteFile(this, fileName, format);
        }
    }
}
