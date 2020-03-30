using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    internal static class XFileReader
    {
        public static XFile ReadFile(string fileName)
        {
            return ReadFile(XofFile.OpenEnumObject(fileName));
        }

        public static XFile ReadFile(byte[] fileBytes)
        {
            return ReadFile(XofFile.OpenEnumObject(fileBytes));
        }

        private static XFile ReadFile(XofFileEnumObject enumObject)
        {
            var file = new XFile();

            using (enumObject)
            {
                bool headerRead = false;
                bool animTicksPerSecondRead = false;

                int childrenCount = enumObject.GetChildrenCount();

                for (int childIndex = 0; childIndex < childrenCount; childIndex++)
                {
#pragma warning disable CA2000 // Supprimer les objets avant la mise hors de portée
                    using (var child = enumObject.GetChild(childIndex))
#pragma warning restore CA2000 // Supprimer les objets avant la mise hors de portée
                    {
                        Guid type = child.GetTemplateType();

                        if (type == XofFileDefaultTemplates.HeaderId)
                        {
                            if (headerRead)
                            {
                                throw new InvalidDataException();
                            }

                            ReadHeader(file, child);
                            headerRead = true;
                        }
                        else if (type == XofFileDefaultTemplates.MeshId)
                        {
                            file.Meshes.Add(ReadMesh(child));
                        }
                        else if (type == XofFileDefaultTemplates.MaterialId)
                        {
                            file.Materials.Add(ReadMaterial(child));
                        }
                        else if (type == XofFileDefaultTemplates.FrameId)
                        {
                            file.Frames.Add(ReadFrame(child));
                        }
                        else if (type == XofFileDefaultTemplates.AnimationSetId)
                        {
                            file.AnimationSets.Add(ReadAnimationSet(child));
                        }
                        else if (type == XofFileDefaultTemplates.AnimTicksPerSecondId)
                        {
                            if (animTicksPerSecondRead)
                            {
                                throw new InvalidDataException();
                            }

                            file.AnimTicksPerSecond = ReadAnimTicksPerSecond(child);
                            animTicksPerSecondRead = true;
                        }
                        else
                        {
                            throw new InvalidDataException();
                        }
                    }
                }
            }

            return file;
        }

        private static void ReadHeader(XFile file, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                short majorVersion = reader.ReadInt16();
                short minorVersion = reader.ReadInt16();
                file.FileVersion = new Version(majorVersion, minorVersion);

                file.FileFlags = reader.ReadInt32();

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static int ReadAnimTicksPerSecond(XofFileEnumData enumData)
        {
            int animTicksPerSecond;

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                animTicksPerSecond = reader.ReadInt32();

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }

            return animTicksPerSecond;
        }

        private static XMesh ReadMesh(XofFileEnumData enumData)
        {
            var mesh = new XMesh
            {
                Name = enumData.GetName()
            };

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                int nVertices = reader.ReadInt32();
                mesh.Vertices.Capacity = nVertices;

                for (int index = 0; index < nVertices; index++)
                {
                    mesh.Vertices.Add(ReadVector(reader));
                }

                int nFaces = reader.ReadInt32();
                mesh.FacesIndices.Capacity = nFaces;

                for (int index = 0; index < nFaces; index++)
                {
                    int indicesCount = reader.ReadInt32();
                    var vertices = new List<int>(indicesCount);

                    for (int i = 0; i < indicesCount; i++)
                    {
                        vertices.Add(reader.ReadInt32());
                    }

                    mesh.FacesIndices.Add(vertices);
                }

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            bool meshNormalsRead = false;
            bool meshTextureCoordsRead = false;
            bool meshMaterialListRead = false;
            bool vertexDuplicationIndicesRead = false;
            bool fvfDataRead = false;
            bool meshVertexColorsIdRead = false;
            bool declDataRead = false;
            bool xSkinMeshHeaderRead = false;

            int childrenCount = enumData.GetChildrenCount();

            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
            {
                using (var child = enumData.GetChild(childIndex))
                {
                    Guid type = child.GetTemplateType();

                    if (type == XofFileDefaultTemplates.MeshNormalsId)
                    {
                        if (meshNormalsRead)
                        {
                            mesh.Normals.Clear();
                            mesh.FacesNormalsIndices.Clear();
                            //throw new InvalidDataException();
                        }

                        ReadMeshNormals(mesh, child);
                        meshNormalsRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.MeshTextureCoordsId)
                    {
                        if (meshTextureCoordsRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadMeshTextureCoords(mesh, child);
                        meshTextureCoordsRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.MeshMaterialListId)
                    {
                        if (meshMaterialListRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadMeshMaterialList(mesh, child);
                        meshMaterialListRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.VertexDuplicationIndicesId)
                    {
                        if (vertexDuplicationIndicesRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadVertexDuplicationIndices(mesh, child);
                        vertexDuplicationIndicesRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.FVFDataId)
                    {
                        if (fvfDataRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadFVFData(mesh, child);
                        fvfDataRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.MeshVertexColorsId)
                    {
                        if (meshVertexColorsIdRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadMeshVertexColors(mesh, child);
                        meshVertexColorsIdRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.DeclDataId)
                    {
                        if (declDataRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadDeclData(mesh, child);
                        declDataRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.XSkinMeshHeaderId)
                    {
                        if (xSkinMeshHeaderRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadXSkinMeshHeader(mesh, child);
                        xSkinMeshHeaderRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.SkinWeightsId)
                    {
                        mesh.SkinWeights.Add(ReadSkinWeights(child));
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            return mesh;
        }

        private static void ReadMeshNormals(XMesh mesh, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                int nNormals = reader.ReadInt32();
                mesh.Normals.Capacity = nNormals;

                for (int index = 0; index < nNormals; index++)
                {
                    mesh.Normals.Add(ReadVector(reader));
                }

                int nFacesNormals = reader.ReadInt32();
                mesh.FacesNormalsIndices.Capacity = nFacesNormals;

                for (int index = 0; index < nFacesNormals; index++)
                {
                    int indicesCount = reader.ReadInt32();
                    var vertices = new List<int>(indicesCount);

                    for (int i = 0; i < indicesCount; i++)
                    {
                        vertices.Add(reader.ReadInt32());
                    }

                    mesh.FacesNormalsIndices.Add(vertices);
                }

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadMeshTextureCoords(XMesh mesh, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                int nTextureCoords = reader.ReadInt32();
                mesh.TextureCoords.Capacity = nTextureCoords;

                for (int index = 0; index < nTextureCoords; index++)
                {
                    mesh.TextureCoords.Add(ReadCoords2d(reader));
                }

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadMeshMaterialList(XMesh mesh, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                int nMaterials = reader.ReadInt32();
                mesh.Materials.Capacity = nMaterials;

                int nFaceIndexes = reader.ReadInt32();
                mesh.MaterialsFacesIndices.Capacity = nFaceIndexes;

                for (int index = 0; index < nFaceIndexes; index++)
                {
                    mesh.MaterialsFacesIndices.Add(reader.ReadInt32());
                }

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            int materialsCount = mesh.Materials.Capacity;

            int childrenCount = enumData.GetChildrenCount();

            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
            {
                using (var child = enumData.GetChild(childIndex))
                {
                    Guid type = child.GetTemplateType();

                    if (type == XofFileDefaultTemplates.MaterialId)
                    {
                        mesh.Materials.Add(ReadMaterial(child));
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            if (mesh.Materials.Count != materialsCount)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadVertexDuplicationIndices(XMesh mesh, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                int nIndices = reader.ReadInt32();

                mesh.OriginalVerticesCount = reader.ReadInt32();

                var indices = new int[nIndices];

                for (int i = 0; i < nIndices; i++)
                {
                    indices[i] = reader.ReadInt32();
                }

                mesh.VertexDuplicationIndices = indices;

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadFVFData(XMesh mesh, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                mesh.FVF = reader.ReadUInt32();

                int nDWords = reader.ReadInt32();
                mesh.FVFData.Capacity = nDWords;

                for (int i = 0; i < nDWords; i++)
                {
                    mesh.FVFData.Add(reader.ReadUInt32());
                }

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadMeshVertexColors(XMesh mesh, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                int nVertexColors = reader.ReadInt32();
                mesh.VertexColors.Capacity = nVertexColors;

                for (int index = 0; index < nVertexColors; index++)
                {
                    mesh.VertexColors.Add(ReadIndexedColor(reader));
                }

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadDeclData(XMesh mesh, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                int nElements = reader.ReadInt32();
                mesh.VertexElements.Capacity = nElements;

                for (int index = 0; index < nElements; index++)
                {
                    mesh.VertexElements.Add(ReadVertexElement(reader));
                }

                int nDWords = reader.ReadInt32();
                var elementsData = new uint[nDWords];

                for (int i = 0; i < nDWords; i++)
                {
                    elementsData[i] = reader.ReadUInt32();
                }

                mesh.VertexElementsData = elementsData;

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadXSkinMeshHeader(XMesh mesh, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                mesh.MaxSkinWeightsPerVertex = reader.ReadInt16();
                mesh.MaxSkinWeightsPerFace = reader.ReadInt16();
                mesh.BonesCount = reader.ReadInt16();

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static XMaterial ReadMaterial(XofFileEnumData enumData)
        {
            var material = new XMaterial
            {
                Name = enumData.GetName()
            };

            if (enumData.IsReference())
            {
                material.IsReference = true;
                return material;
            }

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                material.FaceColor = ReadColorRgba(reader);
                material.Power = reader.ReadSingle();
                material.SpecularColor = ReadColorRgb(reader);
                material.EmissiveColor = ReadColorRgb(reader);

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            bool textureFilenameRead = false;
            bool effectInstanceRead = false;

            int childrenCount = enumData.GetChildrenCount();

            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
            {
                using (var child = enumData.GetChild(childIndex))
                {
                    Guid type = child.GetTemplateType();

                    if (type == XofFileDefaultTemplates.TextureFilenameId)
                    {
                        if (textureFilenameRead)
                        {
                            throw new InvalidDataException();
                        }

                        material.Filename = ReadTextureFilename(child);
                        textureFilenameRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.EffectInstanceId)
                    {
                        if (effectInstanceRead)
                        {
                            throw new InvalidDataException();
                        }

                        material.EffectInstance = ReadEffectInstance(child);
                        effectInstanceRead = true;
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            return material;
        }

        private static string ReadTextureFilename(XofFileEnumData enumData)
        {
            string filename;

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                filename = ReadString(reader);

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }

            return filename;
        }

        private static XEffectInstance ReadEffectInstance(XofFileEnumData enumData)
        {
            var effect = new XEffectInstance
            {
                Name = enumData.GetName()
            };

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                effect.EffectFilename = ReadString(reader);

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            int childrenCount = enumData.GetChildrenCount();

            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
            {
                using (var child = enumData.GetChild(childIndex))
                {
                    Guid type = child.GetTemplateType();

                    if (type == XofFileDefaultTemplates.EffectParamDWordId)
                    {
                        effect.IntegerParameters.Add(ReadEffectParamDWord(child));
                    }
                    else if (type == XofFileDefaultTemplates.EffectParamFloatsId)
                    {
                        effect.FloatParameters.Add(ReadEffectParamFloats(child));
                    }
                    else if (type == XofFileDefaultTemplates.EffectParamStringId)
                    {
                        effect.StringParameters.Add(ReadEffectParamString(child));
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            return effect;
        }

        private static Tuple<string, int> ReadEffectParamDWord(XofFileEnumData enumData)
        {
            string paramName;
            int paramValue;

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                paramName = ReadString(reader);

                paramValue = reader.ReadInt32();

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }

            return Tuple.Create(paramName, paramValue);
        }

        private static Tuple<string, float[]> ReadEffectParamFloats(XofFileEnumData enumData)
        {
            string paramName;
            float[] paramValues;

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                paramName = ReadString(reader);

                int valuesCount = reader.ReadInt32();
                paramValues = new float[valuesCount];

                for (int i = 0; i < valuesCount; i++)
                {
                    paramValues[i] = reader.ReadSingle();
                }

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }

            return Tuple.Create(paramName, paramValues);
        }

        private static Tuple<string, string> ReadEffectParamString(XofFileEnumData enumData)
        {
            string paramName;
            string paramValue;

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                paramName = ReadString(reader);
                paramValue = ReadString(reader);

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }

            return Tuple.Create(paramName, paramValue);
        }

        private static XFrame ReadFrame(XofFileEnumData enumData)
        {
            var frame = new XFrame
            {
                Name = enumData.GetName()
            };

            byte[] data = enumData.GetData();

            if (data.Length != 0)
            {
                throw new InvalidDataException();
            }

            bool frameTransformMatrixRead = false;
            bool frameCameraRead = false;

            int childrenCount = enumData.GetChildrenCount();

            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
            {
                using (var child = enumData.GetChild(childIndex))
                {
                    Guid type = child.GetTemplateType();

                    if (type == XofFileDefaultTemplates.FrameTransformMatrixId)
                    {
                        if (frameTransformMatrixRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadFrameTransformMatrix(frame, child);
                        frameTransformMatrixRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.FrameId)
                    {
                        frame.Frames.Add(ReadFrame(child));
                    }
                    else if (type == XofFileDefaultTemplates.MeshId)
                    {
                        frame.Meshes.Add(ReadMesh(child));
                    }
                    else if (type == XofFileDefaultTemplates.FrameMeshNameId)
                    {
                        ReadFrameMeshName(frame, child);
                    }
                    else if (type == XofFileDefaultTemplates.FrameCameraId)
                    {
                        if (frameCameraRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadFrameCamera(frame, child);
                        frameCameraRead = true;
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            return frame;
        }

        private static void ReadFrameTransformMatrix(XFrame frame, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                frame.TransformMatrix = ReadMatrix4x4(reader);

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadFrameMeshName(XFrame frame, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                int renderPass = reader.ReadInt32();
                string fileName = ReadString(reader);

                frame.MeshesNames.Add(renderPass, fileName);

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static void ReadFrameCamera(XFrame frame, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                frame.CameraRotationScaler = reader.ReadSingle();
                frame.CameraMoveScaler = reader.ReadSingle();

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static XAnimationSet ReadAnimationSet(XofFileEnumData enumData)
        {
            var animationSet = new XAnimationSet
            {
                Name = enumData.GetName()
            };

            byte[] data = enumData.GetData();

            if (data.Length != 0)
            {
                throw new InvalidDataException();
            }

            int childrenCount = enumData.GetChildrenCount();

            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
            {
                using (var child = enumData.GetChild(childIndex))
                {
                    Guid type = child.GetTemplateType();

                    if (type == XofFileDefaultTemplates.AnimationId)
                    {
                        animationSet.Animations.Add(ReadAnimation(child));
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            return animationSet;
        }

        private static XAnimation ReadAnimation(XofFileEnumData enumData)
        {
            var animaton = new XAnimation
            {
                Name = enumData.GetName()
            };

            byte[] data = enumData.GetData();

            if (data.Length != 0)
            {
                throw new InvalidDataException();
            }

            bool frameRead = false;
            bool animationOptionsRead = false;

            int childrenCount = enumData.GetChildrenCount();

            for (int childIndex = 0; childIndex < childrenCount; childIndex++)
            {
                using (var child = enumData.GetChild(childIndex))
                {
                    Guid type = child.GetTemplateType();

                    if (type == XofFileDefaultTemplates.FrameId)
                    {
                        if (frameRead)
                        {
                            throw new InvalidDataException();
                        }

                        if (!child.IsReference())
                        {
                            throw new InvalidDataException();
                        }

                        animaton.FrameReference = child.GetName();
                        frameRead = true;
                    }
                    else if (type == XofFileDefaultTemplates.AnimationKeyId)
                    {
                        animaton.Keys.Add(ReadAnimationKey(child));
                    }
                    else if (type == XofFileDefaultTemplates.AnimationOptionsId)
                    {
                        if (animationOptionsRead)
                        {
                            throw new InvalidDataException();
                        }

                        ReadAnimationOptions(animaton, child);
                        animationOptionsRead = true;
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            return animaton;
        }

        private static void ReadAnimationOptions(XAnimation animation, XofFileEnumData enumData)
        {
            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                animation.OpenClosedOption = reader.ReadInt32();
                animation.PositionQualityOption = reader.ReadInt32();

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }
        }

        private static XAnimationKey ReadAnimationKey(XofFileEnumData enumData)
        {
            var animationKey = new XAnimationKey
            {
                Name = enumData.GetName()
            };

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                animationKey.KeyType = (XAnimationKeyType)reader.ReadInt32();

                int nKeys = reader.ReadInt32();
                animationKey.Keys.Capacity = nKeys;

                for (int index = 0; index < nKeys; index++)
                {
                    int time = reader.ReadInt32();

                    int valuesCount = reader.ReadInt32();
                    var values = new float[valuesCount];

                    for (int i = 0; i < valuesCount; i++)
                    {
                        values[i] = reader.ReadSingle();
                    }

                    animationKey.Keys.Add(Tuple.Create(time, values));
                }

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }

            return animationKey;
        }

        private static XSkinWeights ReadSkinWeights(XofFileEnumData enumData)
        {
            var skin = new XSkinWeights
            {
                Name = enumData.GetName()
            };

            byte[] data = enumData.GetData();

            using (var ms = new MemoryStream(data, false))
            using (var reader = new BinaryReader(ms))
            {
                skin.TransformNodeName = ReadString(reader);

                int nWeights = reader.ReadInt32();
                skin.VertexIndices.Capacity = nWeights;
                skin.Weights.Capacity = nWeights;

                for (int i = 0; i < nWeights; i++)
                {
                    skin.VertexIndices.Add(reader.ReadInt32());
                }

                for (int i = 0; i < nWeights; i++)
                {
                    skin.Weights.Add(reader.ReadSingle());
                }

                skin.MatrixOffset = ReadMatrix4x4(reader);

                if (ms.Position != ms.Length)
                {
                    throw new InvalidDataException();
                }
            }

            if (enumData.GetChildrenCount() != 0)
            {
                throw new InvalidDataException();
            }

            return skin;
        }

        private static XVertexElement ReadVertexElement(BinaryReader reader)
        {
            var element = new XVertexElement
            {
                DataType = (XVertexElementDataType)reader.ReadInt32(),
                Method = (XVertexElementMethod)reader.ReadInt32(),
                Usage = (XVertexElementUsage)reader.ReadInt32(),
                UsageIndex = reader.ReadInt32()
            };

            return element;
        }

        private static string ReadString(BinaryReader reader)
        {
            var sb = new StringBuilder();

            byte b;
            while ((b = reader.ReadByte()) != 0)
            {
                sb.Append((char)b);
            }

            return sb.ToString();
        }

        private static XVector ReadVector(BinaryReader reader)
        {
            return new XVector
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle()
            };
        }

        private static XMatrix4x4 ReadMatrix4x4(BinaryReader reader)
        {
            var matrix = new float[16];

            for (int i = 0; i < 16; i++)
            {
                matrix[i] = reader.ReadSingle();
            }

            return new XMatrix4x4
            {
                Matrix = matrix
            };
        }

        private static XCoords2d ReadCoords2d(BinaryReader reader)
        {
            return new XCoords2d
            {
                U = reader.ReadSingle(),
                V = reader.ReadSingle()
            };
        }

        private static Tuple<int, XColorRgba> ReadIndexedColor(BinaryReader reader)
        {
            int index = reader.ReadInt32();
            XColorRgba color = ReadColorRgba(reader);
            return Tuple.Create(index, color);
        }

        private static XColorRgba ReadColorRgba(BinaryReader reader)
        {
            return new XColorRgba
            {
                Red = reader.ReadSingle(),
                Green = reader.ReadSingle(),
                Blue = reader.ReadSingle(),
                Alpha = reader.ReadSingle()
            };
        }

        private static XColorRgb ReadColorRgb(BinaryReader reader)
        {
            return new XColorRgb
            {
                Red = reader.ReadSingle(),
                Green = reader.ReadSingle(),
                Blue = reader.ReadSingle()
            };
        }
    }
}
