using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XofFileSaveObject : IDisposable
    {
        internal readonly IXofFileSaveObject _saveObject;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal XofFileSaveObject(IXofFileSaveObject saveObject)
        {
            _saveObject = saveObject;
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(_saveObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFile GetFile()
        {
            return new XofFile(_saveObject.GetFile());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileSaveData AddData(Guid template, string name, Guid? id, byte[] data)
        {
            GCHandle idHandle = default;

            try
            {
                if (id.HasValue)
                {
                    idHandle = GCHandle.Alloc(id.Value, GCHandleType.Pinned);
                }

                IXofFileSaveData dataObject = _saveObject.AddDataObject(
                    ref template,
                    string.IsNullOrEmpty(name) ? null : name,
                    id.HasValue ? idHandle.AddrOfPinnedObject() : IntPtr.Zero,
                    data == null || data.Length == 0 ? IntPtr.Zero : new IntPtr(data.Length),
                    data == null || data.Length == 0 ? IntPtr.Zero : Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));

                return new XofFileSaveData(dataObject);
            }
            finally
            {
                if (idHandle.IsAllocated)
                {
                    idHandle.Free();
                }
            }
        }

        public XofFileSaveData AddData(XofFileEnumData child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            return AddData(child.GetTemplateType(), child.GetName(), child.GetId(), child.GetData());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Save()
        {
            _saveObject.Save();
        }
    }
}
