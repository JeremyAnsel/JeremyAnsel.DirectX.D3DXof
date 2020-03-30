using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XofFileEnumData : IDisposable
    {
        internal readonly IXofFileData _data;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal XofFileEnumData(IXofFileData data)
        {
            _data = data;
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(_data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumObject GetEnumObject()
        {
            return new XofFileEnumObject(_data.GetEnum());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetName()
        {
            IntPtr sizePtr = Marshal.AllocHGlobal(IntPtr.Size);

            try
            {
                _data.GetName(IntPtr.Zero, sizePtr);

                int size = Marshal.ReadInt32(sizePtr);

                if (size <= 1)
                {
                    return string.Empty;
                }

                IntPtr nameBuffer = Marshal.AllocHGlobal(size);

                try
                {
                    _data.GetName(nameBuffer, sizePtr);

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
            Guid id = _data.GetId();

            if (id == Guid.Empty)
            {
                return null;
            }

            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetData()
        {
            IntPtr sizePtr = Marshal.AllocHGlobal(IntPtr.Size);
            IntPtr dataPtr = Marshal.AllocHGlobal(IntPtr.Size);

            try
            {
                _data.Lock(sizePtr, dataPtr);
                bool error;
                byte[] bytes;

                try
                {
                    int size = Marshal.ReadInt32(sizePtr);
                    IntPtr data = Marshal.ReadIntPtr(dataPtr);

                    if (size == 0)
                    {
#if NET45
                        bytes = new byte[0];
#else
                        bytes = Array.Empty<byte>();
#endif
                    }
                    else
                    {
                        bytes = new byte[size];
                        Marshal.Copy(data, bytes, 0, size);
                    }
                }
                finally
                {
                    error = _data.Unlock();
                }

                if (error)
                {
                    throw new InvalidOperationException();
                }

                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(sizePtr);
                Marshal.FreeHGlobal(dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Guid GetTemplateType()
        {
            return _data.GetTemplateType();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsReference()
        {
            return _data.IsReference();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetChildrenCount()
        {
            return _data.GetChildren().ToInt32();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumData GetChild(int index)
        {
            return new XofFileEnumData(_data.GetChild(new IntPtr(index)));
        }
    }
}
