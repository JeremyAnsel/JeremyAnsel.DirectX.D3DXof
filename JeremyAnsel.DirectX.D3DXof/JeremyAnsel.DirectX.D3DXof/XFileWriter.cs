using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    internal static class XFileWriter
    {
        public static void WriteFile(XFile file, string fileName, XofFileFormats format)
        {
            using (var saveObject = XofFile.OpenSaveObject(fileName, format))
            {
                WriteFile(file, saveObject);

                saveObject.Save();
            }
        }

        private static void WriteFile(XFile file, XofFileSaveObject saveObject)
        {
            if (file.FileVersion != null)
            {
                WriteHeader(file, saveObject);
            }

            foreach (var mesh in file.Meshes)
            {
                WriteMesh(mesh, saveObject);
            }

            foreach (var material in file.Materials)
            {
                WriteMaterial(material, saveObject);
            }

            foreach (var frame in file.Frames)
            {
                WriteFrame(frame, saveObject);
            }

            foreach (var animationSet in file.AnimationSets)
            {
                WriteAnimationSet(animationSet, saveObject);
            }

            if (file.AnimTicksPerSecond != 0)
            {
                WriteAnimTicksPerSecond(file.AnimTicksPerSecond, saveObject);
            }
        }

        private static void WriteHeader(XFile file, XofFileSaveObject saveObject)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write((short)file.FileVersion.Major);
                writer.Write((short)file.FileVersion.Minor);
                writer.Write(file.FileFlags);

                data = ms.ToArray();
            }

            using (saveObject.AddData(XofFileDefaultTemplates.HeaderId, null, null, data))
            {
            }
        }

        private static void WriteAnimTicksPerSecond(int animTicksPerSecond, XofFileSaveObject saveObject)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(animTicksPerSecond);

                data = ms.ToArray();
            }

            using (saveObject.AddData(XofFileDefaultTemplates.AnimTicksPerSecondId, null, null, data))
            {
            }
        }

        private static void WriteMesh(XMesh mesh, XofFileSaveObject saveObject)
        {
            byte[] data = BuildMeshData(mesh);

            using (var child = saveObject.AddData(XofFileDefaultTemplates.MeshId, mesh.Name, null, data))
            {
                WriteMeshChildren(mesh, child);
            }
        }

        private static void WriteMesh(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data = BuildMeshData(mesh);

            using (var child = saveData.AddData(XofFileDefaultTemplates.MeshId, mesh.Name, null, data))
            {
                WriteMeshChildren(mesh, child);
            }
        }

        private static byte[] BuildMeshData(XMesh mesh)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.Vertices.Count);

                foreach (var vector in mesh.Vertices)
                {
                    WriteVector(vector, writer);
                }

                writer.Write(mesh.FacesIndices.Count);

                foreach (var face in mesh.FacesIndices)
                {
                    writer.Write(face.Count);

                    for (int i = 0; i < face.Count; i++)
                    {
                        writer.Write(face[i]);
                    }
                }

                data = ms.ToArray();
            }

            return data;
        }

        private static void WriteMeshChildren(XMesh mesh, XofFileSaveData child)
        {
            WriteMeshNormals(mesh, child);
            WriteMeshTextureCoords(mesh, child);
            WriteMeshMaterialList(mesh, child);
            WriteVertexDuplicationIndices(mesh, child);
            WriteFVFData(mesh, child);
            WriteMeshVertexColors(mesh, child);
            WriteDeclData(mesh, child);
            WriteXSkinMeshHeader(mesh, child);

            foreach (var skin in mesh.SkinWeights)
            {
                WriteSkinWeights(skin, child);
            }
        }

        private static void WriteMeshNormals(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.Normals.Count);

                foreach (var normal in mesh.Normals)
                {
                    WriteVector(normal, writer);
                }

                writer.Write(mesh.FacesNormalsIndices.Count);

                foreach (var indices in mesh.FacesNormalsIndices)
                {
                    writer.Write(indices.Count);

                    for (int i = 0; i < indices.Count; i++)
                    {
                        writer.Write(indices[i]);
                    }
                }

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.MeshNormalsId, null, null, data))
            {
            }
        }

        private static void WriteMeshTextureCoords(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.TextureCoords.Count);

                foreach (var coords in mesh.TextureCoords)
                {
                    WriteCoords2d(coords, writer);
                }

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.MeshTextureCoordsId, null, null, data))
            {
            }
        }

        private static void WriteMeshMaterialList(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.Materials.Count);
                writer.Write(mesh.MaterialsFacesIndices.Count);

                for (int i = 0; i < mesh.MaterialsFacesIndices.Count; i++)
                {
                    writer.Write(mesh.MaterialsFacesIndices[i]);
                }

                data = ms.ToArray();
            }

            using (var child = saveData.AddData(XofFileDefaultTemplates.MeshMaterialListId, null, null, data))
            {
                foreach (var material in mesh.Materials)
                {
                    WriteMaterial(material, child);
                }
            }
        }

        private static void WriteVertexDuplicationIndices(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.VertexDuplicationIndices?.Length ?? 0);
                writer.Write(mesh.OriginalVerticesCount);

                if (mesh.VertexDuplicationIndices != null)
                {
                    for (int i = 0; i < mesh.VertexDuplicationIndices.Length; i++)
                    {
                        writer.Write(mesh.VertexDuplicationIndices[i]);
                    }
                }

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.VertexDuplicationIndicesId, null, null, data))
            {
            }
        }

        private static void WriteFVFData(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.FVF);

                writer.Write(mesh.FVFData.Count);

                for (int i = 0; i < mesh.FVFData.Count; i++)
                {
                    writer.Write(mesh.FVFData[i]);
                }

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.FVFDataId, null, null, data))
            {
            }
        }

        private static void WriteMeshVertexColors(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.VertexColors.Count);

                foreach (var indexedColor in mesh.VertexColors)
                {
                    WriteIndexedColor(indexedColor, writer);
                }

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.MeshVertexColorsId, null, null, data))
            {
            }
        }

        private static void WriteDeclData(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.VertexElements.Count);

                foreach (var element in mesh.VertexElements)
                {
                    WriteVertexElement(element, writer);
                }

                writer.Write(mesh.VertexElementsData?.Length ?? 0);

                if (mesh.VertexElementsData != null)
                {
                    for (int i = 0; i < mesh.VertexElementsData.Length; i++)
                    {
                        writer.Write(mesh.VertexElementsData[i]);
                    }
                }

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.DeclDataId, null, null, data))
            {
            }
        }

        private static void WriteXSkinMeshHeader(XMesh mesh, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(mesh.MaxSkinWeightsPerVertex);
                writer.Write(mesh.MaxSkinWeightsPerFace);
                writer.Write(mesh.BonesCount);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.XSkinMeshHeaderId, null, null, data))
            {
            }
        }

        private static void WriteMaterial(XMaterial material, XofFileSaveObject saveObject)
        {
            byte[] data = BuildMaterialData(material);

            using (var child = saveObject.AddData(XofFileDefaultTemplates.MaterialId, material.Name, null, data))
            {
                WriteMaterialChildren(material, child);
            }
        }

        private static void WriteMaterial(XMaterial material, XofFileSaveData saveData)
        {
            if (material.IsReference)
            {
                saveData.AddDataReference(material.Name, null);
            }
            else
            {
                byte[] data = BuildMaterialData(material);

                using (var child = saveData.AddData(XofFileDefaultTemplates.MaterialId, material.Name, null, data))
                {
                    WriteMaterialChildren(material, child);
                }
            }
        }

        private static byte[] BuildMaterialData(XMaterial material)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                WriteColorRgba(material.FaceColor, writer);
                writer.Write(material.Power);
                WriteColorRgb(material.SpecularColor, writer);
                WriteColorRgb(material.EmissiveColor, writer);

                data = ms.ToArray();
            }

            return data;
        }

        private static void WriteMaterialChildren(XMaterial material, XofFileSaveData child)
        {
            if (!string.IsNullOrEmpty(material.Filename))
            {
                WriteTextureFilename(material.Filename, child);
            }

            if (material.EffectInstance != null)
            {
                WriteEffectInstance(material.EffectInstance, child);
            }
        }

        private static void WriteTextureFilename(string filename, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                WriteString(filename, writer);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.TextureFilenameId, null, null, data))
            {
            }
        }

        private static void WriteEffectInstance(XEffectInstance effect, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                WriteString(effect.EffectFilename, writer);

                data = ms.ToArray();
            }

            using (var child = saveData.AddData(XofFileDefaultTemplates.EffectInstanceId, effect.Name, null, data))
            {
                foreach (var param in effect.IntegerParameters)
                {
                    WriteEffectParamDWord(param, child);
                }

                foreach (var param in effect.FloatParameters)
                {
                    WriteEffectParamFloats(param, child);
                }

                foreach (var param in effect.StringParameters)
                {
                    WriteEffectParamString(param, child);
                }
            }
        }

        private static void WriteEffectParamDWord(Tuple<string, int> param, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                WriteString(param.Item1, writer);
                writer.Write(param.Item2);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.EffectParamDWordId, null, null, data))
            {
            }
        }

        private static void WriteEffectParamFloats(Tuple<string, float[]> param, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                WriteString(param.Item1, writer);

                writer.Write(param.Item2.Length);

                for (int i = 0; i < param.Item2.Length; i++)
                {
                    writer.Write(param.Item2[i]);
                }

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.EffectParamFloatsId, null, null, data))
            {
            }
        }

        private static void WriteEffectParamString(Tuple<string, string> param, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                WriteString(param.Item1, writer);
                WriteString(param.Item2, writer);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.EffectParamStringId, null, null, data))
            {
            }
        }

        private static void WriteFrame(XFrame frame, XofFileSaveObject saveObject)
        {
            using (var child = saveObject.AddData(XofFileDefaultTemplates.FrameId, frame.Name, null, null))
            {
                WriteFrameChildren(frame, child);
            }
        }

        private static void WriteFrame(XFrame frame, XofFileSaveData saveData)
        {
            using (var child = saveData.AddData(XofFileDefaultTemplates.FrameId, frame.Name, null, null))
            {
                WriteFrameChildren(frame, child);
            }
        }

        private static void WriteFrameChildren(XFrame frame, XofFileSaveData child)
        {
            if (frame.TransformMatrix != null)
            {
                WriteFrameTransformMatrix(frame, child);
            }

            foreach (var subFrame in frame.Frames)
            {
                WriteFrame(subFrame, child);
            }

            foreach (var mesh in frame.Meshes)
            {
                WriteMesh(mesh, child);
            }

            foreach (var frameMeshName in frame.MeshesNames)
            {
                WriteFrameMeshName(frameMeshName, child);
            }

            if (frame.CameraRotationScaler != 1.0f || frame.CameraMoveScaler != 1.0f)
            {
                WriteFrameCamera(frame, child);
            }
        }

        private static void WriteFrameTransformMatrix(XFrame frame, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                WriteMatrix4x4(frame.TransformMatrix, writer);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.FrameTransformMatrixId, null, null, data))
            {
            }
        }

        private static void WriteFrameMeshName(KeyValuePair<int, string> frameMeshName, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(frameMeshName.Key);
                WriteString(frameMeshName.Value, writer);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.FrameMeshNameId, null, null, data))
            {
            }
        }

        private static void WriteFrameCamera(XFrame frame, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(frame.CameraRotationScaler);
                writer.Write(frame.CameraMoveScaler);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.FrameMeshNameId, null, null, data))
            {
            }
        }

        private static void WriteAnimationSet(XAnimationSet animationSet, XofFileSaveObject saveObject)
        {
            using (var child = saveObject.AddData(XofFileDefaultTemplates.AnimationSetId, animationSet.Name, null, null))
            {
                foreach (var animation in animationSet.Animations)
                {
                    WriteAnimation(animation, child);
                }
            }
        }

        private static void WriteAnimation(XAnimation animation, XofFileSaveData saveData)
        {
            using (var child = saveData.AddData(XofFileDefaultTemplates.AnimationId, animation.Name, null, null))
            {
                if (!string.IsNullOrEmpty(animation.FrameReference))
                {
                    // TODO: XofFileDefaultTemplates.FrameId
                    child.AddDataReference(animation.FrameReference, null);
                }

                foreach (var animationKey in animation.Keys)
                {
                    WriteAnimationKey(animationKey, child);
                }

                WriteAnimationOptions(animation, child);
            }
        }

        private static void WriteAnimationOptions(XAnimation animation, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(animation.OpenClosedOption);
                writer.Write(animation.PositionQualityOption);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.AnimationOptionsId, null, null, data))
            {
            }
        }

        private static void WriteAnimationKey(XAnimationKey animationKey, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write((int)animationKey.KeyType);
                writer.Write(animationKey.Keys.Count);

                foreach (var key in animationKey.Keys)
                {
                    writer.Write(key.Item1);
                    writer.Write(key.Item2.Length);

                    for (int i = 0; i < key.Item2.Length; i++)
                    {
                        writer.Write(key.Item2[i]);
                    }
                }

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.AnimationKeyId, animationKey.Name, null, data))
            {
            }
        }

        private static void WriteSkinWeights(XSkinWeights skin, XofFileSaveData saveData)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                WriteString(skin.TransformNodeName, writer);

                writer.Write(skin.VertexIndices.Count);

                for (int i = 0; i < skin.VertexIndices.Count; i++)
                {
                    writer.Write(skin.VertexIndices[i]);
                }

                for (int i = 0; i < skin.Weights.Count; i++)
                {
                    writer.Write(skin.Weights[i]);
                }

                WriteMatrix4x4(skin.MatrixOffset, writer);

                data = ms.ToArray();
            }

            using (saveData.AddData(XofFileDefaultTemplates.SkinWeightsId, skin.Name, null, data))
            {
            }
        }

        private static void WriteVertexElement(XVertexElement element, BinaryWriter writer)
        {
            writer.Write((int)element.DataType);
            writer.Write((int)element.Method);
            writer.Write((int)element.Usage);
            writer.Write(element.UsageIndex);
        }

        private static void WriteString(string str, BinaryWriter writer)
        {
            writer.Write(Encoding.ASCII.GetBytes(str));
            writer.Write((byte)0);
        }

        private static void WriteVector(XVector vector, BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }

        private static void WriteMatrix4x4(XMatrix4x4 matrix, BinaryWriter writer)
        {
            for (int i = 0; i < 16; i++)
            {
                writer.Write(matrix.Matrix[i]);
            }
        }

        private static void WriteCoords2d(XCoords2d coords, BinaryWriter writer)
        {
            writer.Write(coords.U);
            writer.Write(coords.V);
        }

        private static void WriteIndexedColor(Tuple<int, XColorRgba> indexedColor, BinaryWriter writer)
        {
            writer.Write(indexedColor.Item1);
            WriteColorRgba(indexedColor.Item2, writer);
        }

        private static void WriteColorRgba(XColorRgba color, BinaryWriter writer)
        {
            writer.Write(color.Red);
            writer.Write(color.Green);
            writer.Write(color.Blue);
            writer.Write(color.Alpha);
        }

        private static void WriteColorRgb(XColorRgb color, BinaryWriter writer)
        {
            writer.Write(color.Red);
            writer.Write(color.Green);
            writer.Write(color.Blue);
        }
    }
}
