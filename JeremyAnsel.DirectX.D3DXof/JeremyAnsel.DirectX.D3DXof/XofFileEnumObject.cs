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
        public XofFile GetFile()
        {
            return new XofFile(_enumObject.GetFile());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetChildrenCount()
        {
            return _enumObject.GetChildren().ToInt32();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumData GetChild(int index)
        {
            return new XofFileEnumData(_enumObject.GetChild(new IntPtr(index)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumData GetDataObjectById(Guid id)
        {
            try
            {
                return new XofFileEnumData(_enumObject.GetDataObjectById(ref id));
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
        public XofFileEnumData GetDataObjectByName(string name)
        {
            try
            {
                return new XofFileEnumData(_enumObject.GetDataObjectByName(name));
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
