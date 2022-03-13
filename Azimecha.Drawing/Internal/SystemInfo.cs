using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.Internal {
    public static class SystemInfo {
        public static CoreHardwareInfo CoreHardware {
            get {
                CoreHardwareInfo infHW;
                GetSystemInfo(out infHW);
                return infHW;
            }
        }

        public static bool HasCPUFeature(CPUFeature feature) => IsProcessorFeaturePresent(feature);

        [DllImport("kernel32")]
        private static extern void GetSystemInfo(out CoreHardwareInfo infSystem);

        [DllImport("kernel32")]
        private static extern bool IsProcessorFeaturePresent(CPUFeature feature);

    }

    public enum CPUFeature {
        FDIVBug = 0,
        NoFPU = 1,
        CompareExchange = 2,
        MMX = 3,
        SSE1 = 6,
        SSE2 = 10,
        SSE3 = 13,
        PAE = 9,
        NX = 12
    }

    public enum CPUArchitecture : ushort {
        IA32 = 0,
        ARM = 5,
        IA64 = 6,
        AMD64 = 9,
        ARM64 = 12
    }

    public enum CPUType : uint {
        Intel80386 = 386,
        Intel80486 = 486,
        Pentium = 586,
        IA64 = 2200,
        AMD64 = 8664
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CoreHardwareInfo {
        public CPUArchitecture ProcessorArchitecture;
        private ushort _reserved;
        public uint PageSize;
        public IntPtr MinimumAppMemAddress;
        public IntPtr MaximumAppMemAddress;
        public uint ActiveProcessorsMask;
        public uint NumberOfProcessors;
        public CPUType ProcessorType;
        public uint AllocationGranularity;
        public ushort ProcessorLevel;
        public ushort ProcessorRevision;
    }
}
