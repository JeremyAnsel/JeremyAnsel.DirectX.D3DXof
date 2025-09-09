using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XofFileEnumObject : IDisposable
    {
        internal readonly IXofFileEnumObject _enumObject;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal XofFileEnumObject(IXofFileEnumObject enumObject)
        {
            _enumObject = enumObject;
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(_enumObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFile? GetFile()
        {
            IXofFile? file = _enumObject.GetFile();

            if (file is null)
            {
                return null;
            }

            return new XofFile(file);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetChildrenCount()
        {
            return _enumObject.GetChildren().ToInt32();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumData? GetChild(int index)
        {
            IXofFileData? data = _enumObject.GetChild(new IntPtr(index));

            if (data is null)
            {
                return null;
            }

            return new XofFileEnumData(data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumData? GetDataObjectById(Guid id)
        {
            try
            {
                IXofFileData? data = _enumObject.GetDataObjectById(ref id);

                if (data is null)
                {
                    return null;
                }

                return new XofFileEnumData(data);
            }
            catch (Exception ex)
            {
                // D3DXFERR_NOTFOUND
                if (ex.HResult == unchecked((int)0x88760387))
                {
                    return null;
                }

                throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumData? GetDataObjectByName(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            try
            {
                IXofFileData? data = _enumObject.GetDataObjectByName(name!);

                if (data is null)
                {
                    return null;
                }

                return new XofFileEnumData(data);
            }
            catch (Exception ex)
            {
                // D3DXFERR_NOTFOUND
                if (ex.HResult == unchecked((int)0x88760387))
                {
                    return null;
                }

                throw;
            }
        }
    }
}
