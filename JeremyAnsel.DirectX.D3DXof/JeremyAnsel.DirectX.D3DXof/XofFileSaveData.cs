using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XofFileSaveData : IDisposable
    {
        internal readonly IXofFileSaveData _saveData;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal XofFileSaveData(IXofFileSaveData saveData)
        {
            _saveData = saveData;
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(_saveData);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileSaveObject GetSaveObject()
        {
            return new XofFileSaveObject(_saveData.GetSave());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetName()
        {
            IntPtr sizePtr = Marshal.AllocHGlobal(IntPtr.Size);

            try
            {
                _saveData.GetName(IntPtr.Zero, sizePtr);

                int size = Marshal.ReadInt32(sizePtr);

                if (size <= 1)
                {
                    return string.Empty;
                }

                IntPtr nameBuffer = Marshal.AllocHGlobal(size);

                try
                {
                    _saveData.GetName(nameBuffer, sizePtr);

                    return Marshal.PtrToStringAnsi(nameBuffer, size - 1);
                }
                finally
                {
                    Marshal.FreeHGlobal(nameBuffer);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(sizePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Guid? GetId()
        {
            Guid id = _saveData.GetId();

            if (id == Guid.Empty)
            {
                return null;
            }

            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Guid GetTemplateType()
        {
            return _saveData.GetTemplateType();
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

                IXofFileSaveData dataObject = _saveData.AddDataObject(
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
        public void AddDataReference(string name, Guid? id)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            GCHandle idHandle = default;

            try
            {
                if (id.HasValue)
                {
                    idHandle = GCHandle.Alloc(id.Value, GCHandleType.Pinned);
                }

                _saveData.AddDataReference(name, id.HasValue ? idHandle.AddrOfPinnedObject() : IntPtr.Zero);
            }
            finally
            {
                if (idHandle.IsAllocated)
                {
                    idHandle.Free();
                }
            }
        }

        public void AddDataReference(XofFileEnumData child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            AddDataReference(child.GetName(), child.GetId());
        }
    }
}
