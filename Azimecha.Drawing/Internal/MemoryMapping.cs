using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.Internal {
    public static class MemoryMapping {
        public static IDataBuffer MapFileReadOnly(string strFilePath, bool bCopyOnWrite) {
            MemoryWritability writability = bCopyOnWrite ? MemoryWritability.CopyOnWrite : MemoryWritability.ReadOnly;
#if NETFX_40
            return new DotnetMemoryMappedFile(strFilePath, writability);
#else
            return new WindowsMemoryMappedFile(strFilePath, writability);
#endif
        }
    }

    internal enum MemoryWritability {
        ReadOnly,
        CopyOnWrite,
        ReadWrite
    }

#if NETFX_40
    internal class DotnetMemoryMappedFile : IDataBuffer {
        private System.IO.MemoryMappedFiles.MemoryMappedFile _file;
        private System.IO.MemoryMappedFiles.MemoryMappedViewAccessor _view;

        public DotnetMemoryMappedFile(string strFilePath, MemoryWritability writability) {
            System.IO.MemoryMappedFiles.MemoryMappedFileAccess access;

            switch (writability) {
                case MemoryWritability.ReadOnly:
                    access = System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read;
                    break;

                case MemoryWritability.CopyOnWrite:
                    access = System.IO.MemoryMappedFiles.MemoryMappedFileAccess.CopyOnWrite;
                    break;

                case MemoryWritability.ReadWrite:
                    access = System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite;
                    break;

                default:
                    throw new ArgumentException($"Invalid memory writability option {writability}");
            }

            _file = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(strFilePath, System.IO.FileMode.Open,
                null, 0, access);

            _view = _file.CreateViewAccessor();
        }

        public IntPtr DataPointer => _view.SafeMemoryMappedViewHandle.DangerousGetHandle();
        public long DataSize => _view.Capacity;

        public void Dispose() {
            _view?.Dispose();
            _file?.Dispose();
        }
    }
#endif

#if !NETFX_40
    internal class WindowsMemoryMappedFile : IDataBuffer {
        private SafeWindowsMappedMemoryHandle _hView = new SafeWindowsMappedMemoryHandle();
        private long _nSize;

        public WindowsMemoryMappedFile(string strFilePath, MemoryWritability access) {
            FileAccess accessFile;
            MemoryProtection prot;
            MemoryAccess accessMem;

            switch (access) {
                case MemoryWritability.ReadOnly:
                    accessFile = FileAccess.GenericRead;
                    prot = MemoryProtection.ReadOnly;
                    accessMem = MemoryAccess.Read;
                    break;

                case MemoryWritability.CopyOnWrite:
                    accessFile = FileAccess.GenericRead;
                    prot = MemoryProtection.ReadAndWriteCopy;
                    accessMem = MemoryAccess.Read | MemoryAccess.WriteCopy;
                    break;

                case MemoryWritability.ReadWrite:
                    accessFile = FileAccess.GenericWrite | FileAccess.GenericWrite;
                    prot = MemoryProtection.ReadAndWrite;
                    accessMem = MemoryAccess.Read | MemoryAccess.WriteCopy;
                    break;

                default:
                    throw new ArgumentException($"Invalid memory writability option {access}");
            }

            using (SafeWindowsHandle hFile = new SafeWindowsHandle()) {
                hFile.TakeObject(CreateFile(strFilePath, accessFile, FileSharing.Read, IntPtr.Zero, CreationDisposition.OpenExisting, 0, IntPtr.Zero), true);
                if (!hFile.IsHandleValid)
                    throw new Win32Exception();

                uint nFileSizeLow = GetFileSize(hFile.Handle, out uint nFileSizeHigh);
                if ((nFileSizeLow == 0xFFFF_FFFF) && (Marshal.GetLastWin32Error() != 0))
                    throw new Win32Exception();

                ulong nFileSize = ((ulong)nFileSizeHigh << 32) | nFileSizeLow;

                if ((nFileSize > long.MaxValue) || (nFileSize > Utils.IntPtrMaxValue))
                    throw new FormatException($"File \"{strFilePath}\" is too large to use as a data buffer");

                _nSize = (long)nFileSize;

                using (SafeWindowsHandle hSection = new SafeWindowsHandle()) {
                    hSection.TakeObject(CreateFileMapping(hFile.Handle, IntPtr.Zero, prot, 0, 0, null), true);
                    if (!hSection.IsHandleValid)
                        throw new Win32Exception();

                    _hView.TakeObject(MapViewOfFile(hSection.Handle, accessMem, 0, 0, IntPtr.Zero), true);
                    if (!_hView.IsHandleValid)
                        throw new Win32Exception();
                }
            }
        }

        public IntPtr DataPointer => _hView.Handle;
        public long DataSize => _nSize;

        public void Dispose() {
            _hView.Dispose();
        }

        [Flags]
        private enum FileAccess : uint {
            GenericRead = 0x8000_0000,
            GenericWrite = 0x4000_0000,
            GenericExecute = 0x2000_0000,
            GenericAll = 0x1000_0000
        }

        [Flags]
        private enum FileSharing : uint {
            None = 0, Read = 1, Write = 2, Delete = 4
        }

        private enum CreationDisposition : uint {
            CreateNew = 1,
            CreateAlways = 2,
            OpenExisting = 3,
            OpenAlways = 4,
            TruncateExisting = 5
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr CreateFile(string strName, FileAccess access, FileSharing nShareMode, IntPtr pSecAttrib, CreationDisposition nCreationDisp, 
            uint flags, IntPtr hTemplate);

        private enum MemoryProtection : uint {
            ReadOnly = 0x02,
            ReadAndWrite = 0x04,
            ReadAndWriteCopy = 0x08,
            ReadAndExecute = 0x20,
            ReadWriteExecute = 0x40,
            ReadExecuteAndWriteCopy = 0x80,
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr pSecAttrib, MemoryProtection prot, uint nMaxSizeHigh, uint nMaxSizeLow,
            string strName);

        [Flags]
        private enum MemoryAccess : uint {
            Read = 4,
            WriteReal = 2,
            WriteCopy = 1
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr MapViewOfFile(IntPtr hSection, MemoryAccess access, uint nOffsetHigh, uint nOffsetLow, IntPtr nSize);

        [DllImport("kernel32", SetLastError = true)]
        private static extern uint GetFileSize(IntPtr hFile, out uint nFileSizeHigh);
    }

    internal class SafeWindowsHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            CloseHandle(hObject);
        }

        [DllImport("kernel32")]
        private static extern bool CloseHandle(IntPtr hObject);
    }

    internal class SafeWindowsMappedMemoryHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            UnmapViewOfFile(hObject);
        }

        [DllImport("kernel32")]
        private static extern bool UnmapViewOfFile(IntPtr pBaseAddr);
    }
#endif
}
