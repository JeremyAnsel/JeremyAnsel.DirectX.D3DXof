using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    internal static class XofFileDefaultTemplates
    {
        public static readonly string DefaultTemplates = @"xof 0303txt 0032 
template Header
{
<3D82AB43-62DA-11cf-AB39-0020AF71E433>
WORD major;
WORD minor;
DWORD flags;
}
template Animation
{
<3D82AB4F-62DA-11cf-AB39-0020AF71E433>
[...]
}
template FloatKeys
{
<10DD46A9-775B-11cf-8F52-0040333594A3>
DWORD nValues;
array FLOAT values[nValues];
}
template TimedFloatKeys 
{ 
<F406B180-7B3B-11cf-8F52-0040333594A3>
DWORD time; 
FloatKeys tfkeys; 
}
template AnimationKey
{
<10DD46A8-775B-11CF-8F52-0040333594A3>
DWORD keyType;
DWORD nKeys;
array TimedFloatKeys keys[nKeys];
}
template AnimationOptions
{
<E2BF56C0-840F-11cf-8F52-0040333594A3>
DWORD openclosed;
DWORD positionquality;
}
template AnimationSet
{
<3D82AB50-62DA-11cf-AB39-0020AF71E433>
[ Animation <3D82AB4F-62DA-11cf-AB39-0020AF71E433> ]
} 
template AnimTicksPerSecond
{
<9E415A43-7BA6-4a73-8743-B73D47E88476>
DWORD AnimTicksPerSecond;
}
template Boolean
{
<537da6a0-ca37-11d0-941c-0080c80cfa7b>
DWORD truefalse;
}
template Boolean2d
{
<4885AE63-78E8-11cf-8F52-0040333594A3>
Boolean u;
Boolean v;
}
template ColorRGB
{
<D3E16E81-7835-11cf-8F52-0040333594A3>
FLOAT red;
FLOAT green;
FLOAT blue;
}
template ColorRGBA
{
<35FF44E0-6C7C-11cf-8F52-0040333594A3>
FLOAT red;
FLOAT green;
FLOAT blue;
FLOAT alpha;
}
template CompressedAnimationSet
{
<7F9B00B3-F125-4890-876E-1C42BF697C4D>
DWORD CompressedBlockSize;
FLOAT TicksPerSec;
DWORD PlaybackType;
DWORD BufferLength;
array DWORD CompressedData[BufferLength];
}
template Coords2d
{
<F6F23F44-7686-11cf-8F52-0040333594A3>
FLOAT u;
FLOAT v;
}
template VertexElement 
{ 
<F752461C-1E23-48f6-B9F8-8350850F336F> 
DWORD Type; 
DWORD Method; 
DWORD Usage; 
DWORD UsageIndex; 
}
template DeclData
{
<BF22E553-292C-4781-9FEA-62BD554BDD93>
DWORD nElements;
array VertexElement Elements[nElements];
DWORD nDWords;
array DWORD data[nDWords];
}
template EffectDWord
{
<622C0ED0-956E-4da9-908A-2AF94F3CE716>
DWORD Value;       
}
template EffectFloats
{
<F1CFE2B3-0DE3-4e28-AFA1-155A750A282D>
DWORD nFloats;
array FLOAT Floats[nFloats];
}
template EffectInstance
{
<E331F7E4-0559-4cc2-8E99-1CEC1657928F>
STRING EffectFilename;
[...]
}
template EffectParamDWord
{
<E13963BC-AE51-4c5d-B00F-CFA3A9D97CE5>
STRING ParamName;
DWORD Value;
}
template EffectParamFloats
{
<3014B9A0-62F5-478c-9B86-E4AC9F4E418B>
STRING ParamName;
DWORD nFloats;
array FLOAT Floats[nFloats];
}
template EffectParamString
{
<1DBC4C88-94C1-46ee-9076-2C28818C9481>
STRING ParamName;
STRING Value;
}
template EffectString
{
<D55B097E-BDB6-4c52-B03D-6051C89D0E42>
STRING Value;
}
template FaceAdjacency
{
<A64C844A-E282-4756-8B80-250CDE04398C>
DWORD nIndices;
array DWORD indices[nIndices];
}
template Frame
{
<3D82AB46-62DA-11CF-AB39-0020AF71E433>
[...]			
}
template Matrix4x4
{
<F6F23F45-7686-11cf-8F52-0040333594A3>
array FLOAT matrix[16];
}
template FrameTransformMatrix
{
<F6F23F41-7686-11cf-8F52-0040333594A3>
Matrix4x4 frameMatrix;
}
template FrameMeshName
{
<c2a50aed-0ee9-4d97-8732-c14a2d8a7825>
DWORD RenderPass;
STRING FileName;
}
template FrameCamera
{
<f96e7de6-40ce-4847-b7e9-5875232e5201>
FLOAT RotationScaler;
FLOAT MoveScaler;
}
template FVFData
{
<B6E70A0E-8EF9-4e83-94AD-ECC8B0C04897>
DWORD dwFVF;
DWORD nDWords;
array DWORD data[nDWords];
}
template Guid
{
<a42790e0-7810-11cf-8f52-0040333594a3>
DWORD data1;
WORD data2;
WORD data3;
array UCHAR data4[8];
}
template Material
{
<3D82AB4D-62DA-11CF-AB39-0020AF71E433>
ColorRGBA faceColor;
FLOAT power;
ColorRGB specularColor;
ColorRGB emissiveColor;
[...]
}
template MaterialWrap
{
<4885ae60-78e8-11cf-8f52-0040333594a3>
Boolean u;
Boolean v;
}
template Vector 
{ 
<3D82AB5E-62DA-11cf-AB39-0020AF71E433> 
FLOAT x; 
FLOAT y; 
FLOAT z; 
}
template MeshFace
{
<3D82AB5F-62DA-11cf-AB39-0020AF71E433>
DWORD nFaceVertexIndices;
array DWORD faceVertexIndices[nFaceVertexIndices];
}
template Mesh
{
<3D82AB44-62DA-11CF-AB39-0020AF71E433>
DWORD nVertices;
array Vector vertices[nVertices];
DWORD nFaces;
array MeshFace faces[nFaces];
[...]
}
template MeshFaceWraps
{
<ED1EC5C0-C0A8-11D0-941C-0080C80CFA7B>
DWORD nFaceWrapValues;
array Boolean2d faceWrapValues[nFaceWrapValues];
}
template MeshMaterialList
{
<F6F23F42-7686-11CF-8F52-0040333594A3>
DWORD nMaterials;
DWORD nFaceIndexes;
array DWORD faceIndexes[nFaceIndexes];
[Material <3D82AB4D-62DA-11CF-AB39-0020AF71E433>]
}
template MeshNormals
{
<F6F23F43-7686-11cf-8F52-0040333594A3>
DWORD nNormals;
array Vector normals[nNormals];
DWORD nFaceNormals;
array MeshFace faceNormals[nFaceNormals];
}
template MeshTextureCoords
{
<F6F23F40-7686-11cf-8F52-0040333594A3>
DWORD nTextureCoords;
array Coords2d textureCoords[nTextureCoords] ;
}
template IndexedColor
{
<1630B820-7842-11cf-8F52-0040333594A3>
DWORD index;
ColorRGBA indexColor;
}
template MeshVertexColors
{
<1630B821-7842-11cf-8F52-0040333594A3>
DWORD nVertexColors;
array IndexedColor vertexColors[nVertexColors];
}
template Patch
{
<A3EB5D44-FC22-429D-9AFB-3221CB9719A6>
DWORD nControlIndices;
array DWORD controlIndices[nControlIndices];
}
template PatchMesh
{
<D02C95CC-EDBA-4305-9B5D-1820D7704BBF>
DWORD nVertices;
array Vector vertices[nVertices];
DWORD nPatches;
array Patch patches[nPatches];
[...]
}
template PatchMesh9
{
<B9EC94E1-B9A6-4251-BA18-94893F02C0EA>
DWORD Type;
DWORD Degree;
DWORD Basis;
DWORD nVertices;
array Vector vertices[nVertices];
DWORD nPatches;
array Patch patches[nPatches];
[...]
}
template PMAttributeRange
{
<917E0427-C61E-4a14-9C64-AFE65F9E9844>
DWORD iFaceOffset;
DWORD nFacesMin;
DWORD nFacesMax;
DWORD iVertexOffset;
DWORD nVerticesMin;
DWORD nVerticesMax;
}
template PMVSplitRecord 
{ 
<574CCC14-F0B3-4333-822D-93E8A8A08E4C> 
DWORD iFaceCLW; 
DWORD iVlrOffset; 
DWORD iCode; 
}
template PMInfo 
{ 
<B6C3E656-EC8B-4b92-9B62-681659522947> 
DWORD nAttributes; 
array PMAttributeRange attributeRanges[nAttributes]; 
DWORD nMaxValence; 
DWORD nMinLogicalVertices; 
DWORD nMaxLogicalVertices; 
DWORD nVSplits; 
array PMVSplitRecord splitRecords[nVSplits]; 
DWORD nAttributeMispredicts; 
array DWORD attributeMispredicts[nAttributeMispredicts]; 
}
template SkinWeights
{ 
<6F0D123B-BAD2-4167-A0D0-80224F25FABB> 
STRING transformNodeName; 
DWORD nWeights; 
array DWORD vertexIndices[nWeights]; 
array FLOAT weights[nWeights]; 
Matrix4x4 matrixOffset; 
}
template TextureFilename 
{ 
<A42790E1-7810-11cf-8F52-0040333594A3> 
STRING filename; 
}
template VertexDuplicationIndices 
{ 
<B8D65549-D7C9-4995-89CF-53A9A8B031E3> 
DWORD nIndices; 
DWORD nOriginalVertices; 
array DWORD indices[nIndices]; 
}
template XSkinMeshHeader 
{ 
<3CF169CE-FF7C-44ab-93C0-F78F62D172E2>  
WORD nMaxSkinWeightsPerVertex; 
WORD nMaxSkinWeightsPerFace; 
WORD nBones; 
}
".Replace("\r\n", "\n");

        public static readonly Guid HeaderId = new Guid("3D82AB43-62DA-11cf-AB39-0020AF71E433");

        public static readonly Guid AnimationId = new Guid("3D82AB4F-62DA-11cf-AB39-0020AF71E433");

        public static readonly Guid FloatKeysId = new Guid("10DD46A9-775B-11cf-8F52-0040333594A3");

        public static readonly Guid TimedFloatKeysId = new Guid("F406B180-7B3B-11cf-8F52-0040333594A3");

        public static readonly Guid AnimationKeyId = new Guid("10DD46A8-775B-11CF-8F52-0040333594A3");

        public static readonly Guid AnimationOptionsId = new Guid("E2BF56C0-840F-11cf-8F52-0040333594A3");

        public static readonly Guid AnimationSetId = new Guid("3D82AB50-62DA-11cf-AB39-0020AF71E433");

        public static readonly Guid AnimTicksPerSecondId = new Guid("9E415A43-7BA6-4a73-8743-B73D47E88476");

        public static readonly Guid BooleanId = new Guid("537da6a0-ca37-11d0-941c-0080c80cfa7b");

        public static readonly Guid Boolean2dId = new Guid("4885AE63-78E8-11cf-8F52-0040333594A3");

        public static readonly Guid ColorRGBId = new Guid("D3E16E81-7835-11cf-8F52-0040333594A3");

        public static readonly Guid ColorRGBAId = new Guid("35FF44E0-6C7C-11cf-8F52-0040333594A3");

        public static readonly Guid CompressedAnimationSetId = new Guid("7F9B00B3-F125-4890-876E-1C42BF697C4D");

        public static readonly Guid Coords2dId = new Guid("F6F23F44-7686-11cf-8F52-0040333594A3");

        public static readonly Guid VertexElementId = new Guid("F752461C-1E23-48f6-B9F8-8350850F336F");

        public static readonly Guid DeclDataId = new Guid("BF22E553-292C-4781-9FEA-62BD554BDD93");

        public static readonly Guid EffectDWordId = new Guid("622C0ED0-956E-4da9-908A-2AF94F3CE716");

        public static readonly Guid EffectFloatsId = new Guid("F1CFE2B3-0DE3-4e28-AFA1-155A750A282D");

        public static readonly Guid EffectInstanceId = new Guid("E331F7E4-0559-4cc2-8E99-1CEC1657928F");

        public static readonly Guid EffectParamDWordId = new Guid("E13963BC-AE51-4c5d-B00F-CFA3A9D97CE5");

        public static readonly Guid EffectParamFloatsId = new Guid("3014B9A0-62F5-478c-9B86-E4AC9F4E418B");

        public static readonly Guid EffectParamStringId = new Guid("1DBC4C88-94C1-46ee-9076-2C28818C9481");

        public static readonly Guid EffectStringId = new Guid("D55B097E-BDB6-4c52-B03D-6051C89D0E42");

        public static readonly Guid FaceAdjacencyId = new Guid("A64C844A-E282-4756-8B80-250CDE04398C");

        public static readonly Guid FrameId = new Guid("3D82AB46-62DA-11CF-AB39-0020AF71E433");

        public static readonly Guid Matrix4x4Id = new Guid("F6F23F45-7686-11cf-8F52-0040333594A3");

        public static readonly Guid FrameTransformMatrixId = new Guid("F6F23F41-7686-11cf-8F52-0040333594A3");

        public static readonly Guid FrameMeshNameId = new Guid("c2a50aed-0ee9-4d97-8732-c14a2d8a7825");

        public static readonly Guid FrameCameraId = new Guid("f96e7de6-40ce-4847-b7e9-5875232e5201");

        public static readonly Guid FVFDataId = new Guid("B6E70A0E-8EF9-4e83-94AD-ECC8B0C04897");

        public static readonly Guid GuidId = new Guid("a42790e0-7810-11cf-8f52-0040333594a3");

        public static readonly Guid MaterialId = new Guid("3D82AB4D-62DA-11CF-AB39-0020AF71E433");

        public static readonly Guid MaterialWrapId = new Guid("4885ae60-78e8-11cf-8f52-0040333594a3");

        public static readonly Guid VectorId = new Guid("3D82AB5E-62DA-11cf-AB39-0020AF71E433");

        public static readonly Guid MeshFaceId = new Guid("3D82AB5F-62DA-11cf-AB39-0020AF71E433");

        public static readonly Guid MeshId = new Guid("3D82AB44-62DA-11CF-AB39-0020AF71E433");

        public static readonly Guid MeshFaceWrapsId = new Guid("ED1EC5C0-C0A8-11D0-941C-0080C80CFA7B");

        public static readonly Guid MeshMaterialListId = new Guid("F6F23F42-7686-11CF-8F52-0040333594A3");

        public static readonly Guid MeshNormalsId = new Guid("F6F23F43-7686-11cf-8F52-0040333594A3");

        public static readonly Guid MeshTextureCoordsId = new Guid("F6F23F40-7686-11cf-8F52-0040333594A3");

        public static readonly Guid IndexedColorId = new Guid("1630B820-7842-11cf-8F52-0040333594A3");

        public static readonly Guid MeshVertexColorsId = new Guid("1630B821-7842-11cf-8F52-0040333594A3");

        public static readonly Guid PatchId = new Guid("A3EB5D44-FC22-429D-9AFB-3221CB9719A6");

        public static readonly Guid PatchMeshId = new Guid("D02C95CC-EDBA-4305-9B5D-1820D7704BBF");

        public static readonly Guid PatchMesh9Id = new Guid("B9EC94E1-B9A6-4251-BA18-94893F02C0EA");

        public static readonly Guid PMAttributeRangeId = new Guid("917E0427-C61E-4a14-9C64-AFE65F9E9844");

        public static readonly Guid PMVSplitRecordId = new Guid("574CCC14-F0B3-4333-822D-93E8A8A08E4C");

        public static readonly Guid PMInfoId = new Guid("B6C3E656-EC8B-4b92-9B62-681659522947");

        public static readonly Guid SkinWeightsId = new Guid("6F0D123B-BAD2-4167-A0D0-80224F25FABB");

        public static readonly Guid TextureFilenameId = new Guid("A42790E1-7810-11cf-8F52-0040333594A3");

        public static readonly Guid VertexDuplicationIndicesId = new Guid("B8D65549-D7C9-4995-89CF-53A9A8B031E3");

        public static readonly Guid XSkinMeshHeaderId = new Guid("3CF169CE-FF7C-44ab-93C0-F78F62D172E2");
    }
}
