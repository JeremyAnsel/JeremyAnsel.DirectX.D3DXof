using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XofFile : IDisposable
    {
        internal readonly IXofFile _file;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal XofFile(IXofFile file)
        {
            _file = file;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFile()
        {
            NativeMethods.D3DXFileCreate(out IXofFile file);
            _file = file;
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(_file);
        }

        public static XofFileEnumObject OpenEnumObject(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            using (var file = new XofFile())
            {
                file.RegisterDefaultTemplates();

                return file.CreateEnumObject(path);
            }
        }

        public static XofFileEnumObject OpenEnumObject(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes));
            }

            using (var file = new XofFile())
            {
                file.RegisterDefaultTemplates();

                return file.CreateEnumObject(bytes);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumObject CreateEnumObject(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            IntPtr pathPtr = Marshal.StringToHGlobalUni(path);

            try
            {
                IXofFileEnumObject enumObject = _file.CreateEnumObject(pathPtr, XofFileLoadOptions.FromWFile);
                return new XofFileEnumObject(enumObject);
            }
            finally
            {
                Marshal.FreeHGlobal(pathPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileEnumObject CreateEnumObject(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes));
            }

            XofFileLoadMemory memory = new XofFileLoadMemory
            {
                Memory = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0),
                Size = new IntPtr(bytes.Length)
            };

            GCHandle memoryHandle = GCHandle.Alloc(memory, GCHandleType.Pinned);

            try
            {
                IXofFileEnumObject enumObject = _file.CreateEnumObject(memoryHandle.AddrOfPinnedObject(), XofFileLoadOptions.FromMemory);
                return new XofFileEnumObject(enumObject);
            }
            finally
            {
                memoryHandle.Free();
            }
        }

        public static XofFileSaveObject OpenSaveObject(string path, XofFileFormats format)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            using (var file = new XofFile())
            {
                file.RegisterDefaultTemplates();

                return file.CreateSaveObject(path, format);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public XofFileSaveObject CreateSaveObject(string path, XofFileFormats format)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            IntPtr pathPtr = Marshal.StringToHGlobalUni(path);

            try
            {
                IXofFileSaveObject saveObject = _file.CreateSaveObject(pathPtr, XofFileSaveOptions.ToWFile, format);
                return new XofFileSaveObject(saveObject);
            }
            finally
            {
                Marshal.FreeHGlobal(pathPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterTemplates(string template)
        {
            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentNullException(nameof(template));
            }

            IntPtr templatePtr = Marshal.StringToHGlobalAnsi(template);
            IntPtr templateSize = new IntPtr(Encoding.UTF8.GetByteCount(template));

            try
            {
                _file.RegisterTemplates(templatePtr, templateSize);
            }
            finally
            {
                Marshal.FreeHGlobal(templatePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterTemplates(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes));
            }

            IntPtr templatePtr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
            IntPtr templateSize = new IntPtr(bytes.Length);

            _file.RegisterTemplates(templatePtr, templateSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterEnumObjectTemplates(XofFileEnumObject enumObject)
        {
            if (enumObject == null)
            {
                throw new ArgumentNullException(nameof(enumObject));
            }

            _file.RegisterEnumTemplates(enumObject._enumObject);
        }

        public void RegisterDefaultTemplates()
        {
            this.RegisterTemplates(XofFileDefaultTemplates.DefaultTemplates);
        }
    }
}
