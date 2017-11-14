﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection.Emit;

namespace NvApiWrapper
{
    public enum NvStatus
    {
        OK = 0,
        ERROR = -1,
        LIBRARY_NOT_FOUND = -2,
        NO_IMPLEMENTATION = -3,
        API_NOT_INTIALIZED = -4,
        INVALID_ARGUMENT = -5,
        NVIDIA_DEVICE_NOT_FOUND = -6,
        END_ENUMERATION = -7,
        INVALID_HANDLE = -8,
        INCOMPATIBLE_STRUCT_VERSION = -9,
        HANDLE_INVALIDATED = -10,
        OPENGL_CONTEXT_NOT_CURRENT = -11,
        NO_GL_EXPERT = -12,
        INSTRUMENTATION_DISABLED = -13,
        EXPECTED_LOGICAL_GPU_HANDLE = -100,
        EXPECTED_PHYSICAL_GPU_HANDLE = -101,
        EXPECTED_DISPLAY_HANDLE = -102,
        INVALID_COMBINATION = -103,
        NOT_SUPPORTED = -104,
        PORTID_NOT_FOUND = -105,
        EXPECTED_UNATTACHED_DISPLAY_HANDLE = -106,
        INVALID_PERF_LEVEL = -107,
        DEVICE_BUSY = -108,
        NV_PERSIST_FILE_NOT_FOUND = -109,
        PERSIST_DATA_NOT_FOUND = -110,
        EXPECTED_TV_DISPLAY = -111,
        EXPECTED_TV_DISPLAY_ON_DCONNECTOR = -112,
        NO_ACTIVE_SLI_TOPOLOGY = -113,
        SLI_RENDERING_MODE_NOTALLOWED = -114,
        EXPECTED_DIGITAL_FLAT_PANEL = -115,
        ARGUMENT_EXCEED_MAX_SIZE = -116,
        DEVICE_SWITCHING_NOT_ALLOWED = -117,
        TESTING_CLOCKS_NOT_SUPPORTED = -118,
        UNKNOWN_UNDERSCAN_CONFIG = -119,
        TIMEOUT_RECONFIGURING_GPU_TOPO = -120,
        DATA_NOT_FOUND = -121,
        EXPECTED_ANALOG_DISPLAY = -122,
        NO_VIDLINK = -123,
        REQUIRES_REBOOT = -124,
        INVALID_HYBRID_MODE = -125,
        MIXED_TARGET_TYPES = -126,
        SYSWOW64_NOT_SUPPORTED = -127,
        IMPLICIT_SET_GPU_TOPOLOGY_CHANGE_NOT_ALLOWED = -128,
        REQUEST_USER_TO_CLOSE_NON_MIGRATABLE_APPS = -129,
        OUT_OF_MEMORY = -130,
        WAS_STILL_DRAWING = -131,
        FILE_NOT_FOUND = -132,
        TOO_MANY_UNIQUE_STATE_OBJECTS = -133,
        INVALID_CALL = -134,
        D3D10_1_LIBRARY_NOT_FOUND = -135,
        FUNCTION_NOT_FOUND = -136
    }

    public enum NvThermalController
    {
        NONE = 0,
        GPU_INTERNAL,
        ADM1032,
        MAX6649,
        MAX1617,
        LM99,
        LM89,
        LM64,
        ADT7473,
        SBMAX6649,
        VBIOSEVT,
        OS,
        UNKNOWN = -1,
    }

    public enum NvThermalTarget
    {
        NONE = 0,
        GPU = 1,
        MEMORY = 2,
        POWER_SUPPLY = 4,
        BOARD = 8,
        ALL = 15,
        UNKNOWN = -1
    };

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvSensor
    {
        public NvThermalController Controller;
        public uint DefaultMinTemp;
        public uint DefaultMaxTemp;
        public uint CurrentTemp;
        public NvThermalTarget Target;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvGPUThermalSettings
    {
        public uint Version;
        public uint Count;
        [MarshalAs(UnmanagedType.ByValArray,
          SizeConst = NVAPI.MAX_THERMAL_SENSORS_PER_GPU)]
        public NvSensor[] Sensors;

        public int CurrentTemp => (int) (Sensors[0].CurrentTemp);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NvDisplayHandle
    {
        private readonly IntPtr ptr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NvPhysicalGpuHandle
    {
        private readonly IntPtr ptr;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvClocks
    {
        public uint Version;
        public uint ClockType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NVAPI.MAX_CLOCKS_PER_GPU)]
        public NvClock[] Clocks;

        public int CoreClock => (int)(Clocks[0].Frequency/1000);
        public int MemoryClock => (int)(Clocks[4].Frequency / 1000);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvPState
    {
        public bool Present;
        public int Percentage;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvClock
    {
        public uint Present;
        public uint Frequency;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvPStates
    {
        public uint Version;
        public uint Flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NVAPI.MAX_PSTATES_PER_GPU)]
        public NvPState[] PStates;

        public int GpuUsage => (int)(PStates[0].Percentage );
        public int GpuPower => (int)(PStates[1].Percentage );
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvUsages
    {
        public uint Version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NVAPI.MAX_USAGES_PER_GPU)]
        public uint[] Usage;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvCooler
    {
        public int Type;
        public int Controller;
        public int DefaultMin;
        public int DefaultMax;
        public int CurrentMin;
        public int CurrentMax;
        public int CurrentLevel;
        public int DefaultPolicy;
        public int CurrentPolicy;
        public int Target;
        public int ControlType;
        public int Active;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvGPUCoolerSettings
    {
        public uint Version;
        public uint Count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NVAPI.MAX_COOLER_PER_GPU)]
        public NvCooler[] Cooler;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvMemoryInfo
    {
        public uint Version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =
          NVAPI.MAX_MEMORY_VALUES_PER_GPU)]
        public uint[] Values;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvDisplayDriverVersion
    {
        public uint Version;
        public uint DriverVersion;
        public uint BldChangeListNum;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NVAPI.SHORT_STRING_MAX)]
        public string BuildBranch;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NVAPI.SHORT_STRING_MAX)]
        public string Adapter;
    }

    public class NVAPI
    {

        public const int MAX_PHYSICAL_GPUS = 64;
        public const int SHORT_STRING_MAX = 64;

        public const int MAX_THERMAL_SENSORS_PER_GPU = 3;
        public const int MAX_CLOCKS_PER_GPU = 32;
        public const int MAX_PSTATES_PER_GPU = 8;
        public const int MAX_USAGES_PER_GPU = 33;
        public const int MAX_COOLER_PER_GPU = 20;
        public const int MAX_MEMORY_VALUES_PER_GPU = 5;

        public static readonly uint GPU_THERMAL_SETTINGS_VER = (uint)
          Marshal.SizeOf(typeof(NvGPUThermalSettings)) | 0x10000;
        public static readonly uint GPU_CLOCKS_VER = (uint) Marshal.SizeOf(typeof(NvClocks)) | 0x20000;
        public static readonly uint GPU_PSTATES_VER = (uint)
          Marshal.SizeOf(typeof(NvPStates)) | 0x10000;
        public static readonly uint GPU_USAGES_VER = (uint)
          Marshal.SizeOf(typeof(NvUsages)) | 0x10000;
        public static readonly uint GPU_COOLER_SETTINGS_VER = (uint)
          Marshal.SizeOf(typeof(NvGPUCoolerSettings)) | 0x20000;
        public static readonly uint GPU_MEMORY_INFO_VER = (uint)
          Marshal.SizeOf(typeof(NvMemoryInfo)) | 0x20000;
        public static readonly uint DISPLAY_DRIVER_VERSION_VER = (uint)
          Marshal.SizeOf(typeof(NvDisplayDriverVersion)) | 0x10000;

        private delegate NvStatus NvAPI_InitializeDelegate();
        private delegate NvStatus NvAPI_GPU_GetFullNameDelegate(
          NvPhysicalGpuHandle gpuHandle, StringBuilder name);

        public delegate NvStatus NvAPI_GPU_GetThermalSettingsDelegate(
          NvPhysicalGpuHandle gpuHandle, int sensorIndex,
          ref NvGPUThermalSettings nvGPUThermalSettings);
        public delegate NvStatus NvAPI_EnumNvidiaDisplayHandleDelegate(int thisEnum,
          ref NvDisplayHandle displayHandle);
        public delegate NvStatus NvAPI_GetPhysicalGPUsFromDisplayDelegate(
          NvDisplayHandle displayHandle, [Out] NvPhysicalGpuHandle[] gpuHandles,
          out uint gpuCount);
        public delegate NvStatus NvAPI_EnumPhysicalGPUsDelegate(
          [Out] NvPhysicalGpuHandle[] gpuHandles, out int gpuCount);
        public delegate NvStatus NvAPI_GPU_GetTachReadingDelegate(
          NvPhysicalGpuHandle gpuHandle, out int value);
        public delegate NvStatus NvAPI_GPU_GetAllClocksDelegate(NvPhysicalGpuHandle gpuHandle, ref NvClocks nvClocks);
        public delegate NvStatus NvAPI_GPU_GetPStatesDelegate(
          NvPhysicalGpuHandle gpuHandle, ref NvPStates nvPStates);
        public delegate NvStatus NvAPI_GPU_GetUsagesDelegate(
          NvPhysicalGpuHandle gpuHandle, ref NvUsages nvUsages);
        public delegate NvStatus NvAPI_GPU_GetCoolerSettingsDelegate(
          NvPhysicalGpuHandle gpuHandle, int coolerIndex,
          ref NvGPUCoolerSettings nvGPUCoolerSettings);
        public delegate NvStatus NvAPI_GPU_GetMemoryInfoDelegate(
          NvDisplayHandle displayHandle, ref NvMemoryInfo nvMemoryInfo);
        public delegate NvStatus NvAPI_GetDisplayDriverVersionDelegate(
          NvDisplayHandle displayHandle, [In, Out] ref NvDisplayDriverVersion
          nvDisplayDriverVersion);
        public delegate NvStatus NvAPI_GetInterfaceVersionStringDelegate(
          StringBuilder version);

        private static readonly bool available;
        private static readonly NvAPI_InitializeDelegate NvAPI_Initialize;
        private static readonly NvAPI_GPU_GetFullNameDelegate
          _NvAPI_GPU_GetFullName;
        private static readonly NvAPI_GetInterfaceVersionStringDelegate
          _NvAPI_GetInterfaceVersionString;

        public static readonly NvAPI_GPU_GetThermalSettingsDelegate
          NvAPI_GPU_GetThermalSettings;
        public static readonly NvAPI_EnumNvidiaDisplayHandleDelegate
          NvAPI_EnumNvidiaDisplayHandle;
        public static readonly NvAPI_GetPhysicalGPUsFromDisplayDelegate
          NvAPI_GetPhysicalGPUsFromDisplay;
        public static readonly NvAPI_EnumPhysicalGPUsDelegate
          NvAPI_EnumPhysicalGPUs;
        public static readonly NvAPI_GPU_GetTachReadingDelegate NvAPI_GPU_GetTachReading;
        public static readonly NvAPI_GPU_GetAllClocksDelegate NvAPI_GPU_GetAllClocks;
        public static readonly NvAPI_GPU_GetPStatesDelegate
          NvAPI_GPU_GetPStates;
        public static readonly NvAPI_GPU_GetUsagesDelegate
          NvAPI_GPU_GetUsages;
        public static readonly NvAPI_GPU_GetCoolerSettingsDelegate
          NvAPI_GPU_GetCoolerSettings;
        public static readonly NvAPI_GPU_GetMemoryInfoDelegate
          NvAPI_GPU_GetMemoryInfo;
        public static readonly NvAPI_GetDisplayDriverVersionDelegate
          NvAPI_GetDisplayDriverVersion;

        private NVAPI() { }

        public static NvStatus NvAPI_GPU_GetFullName(NvPhysicalGpuHandle gpuHandle,
          out string name)
        {
            StringBuilder builder = new StringBuilder(SHORT_STRING_MAX);
            NvStatus status;
            if (_NvAPI_GPU_GetFullName != null)
                status = _NvAPI_GPU_GetFullName(gpuHandle, builder);
            else
                status = NvStatus.FUNCTION_NOT_FOUND;
            name = builder.ToString();
            return status;
        }

        public static NvStatus NvAPI_GetInterfaceVersionString(out string version)
        {
            StringBuilder builder = new StringBuilder(SHORT_STRING_MAX);
            NvStatus status;
            if (_NvAPI_GetInterfaceVersionString != null)
                status = _NvAPI_GetInterfaceVersionString(builder);
            else
                status = NvStatus.FUNCTION_NOT_FOUND;
            version = builder.ToString();
            return status;
        }

       
        private static void GetDelegate<T>(uint id, out T newDelegate)
          where T : class
        {
            IntPtr ptr = nvapi_QueryInterface(id);
            if (ptr != IntPtr.Zero)
            {
                newDelegate =
                  Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;
            }
            else
            {
                newDelegate = null;
            }
        }

        [DllImport("nvapi64.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr nvapi_QueryInterface(uint id);

        static NVAPI()
        {
            try
            {
                GetDelegate((uint)FunctionId.NvAPI_Initialize, out NvAPI_Initialize);
            }
            catch (DllNotFoundException) { return; }
            //catch (EntryPointNotFoundException) { return; }
            catch (Exception e) { throw e; }
            //catch (ArgumentNullException) { return; }

            if (NvAPI_Initialize() == NvStatus.OK)
            {
                GetDelegate((uint)FunctionId.NvAPI_GPU_GetThermalSettings, out NvAPI_GPU_GetThermalSettings);
                GetDelegate((uint)FunctionId.NvAPI_GPU_GetFullName, out _NvAPI_GPU_GetFullName);
                GetDelegate((uint)FunctionId.NvAPI_EnumNvidiaDisplayHandle, out NvAPI_EnumNvidiaDisplayHandle);
                GetDelegate((uint)FunctionId.NvAPI_GetPhysicalGPUsFromDisplay, out NvAPI_GetPhysicalGPUsFromDisplay);
                GetDelegate((uint)FunctionId.NvAPI_EnumPhysicalGPUs, out NvAPI_EnumPhysicalGPUs);
                GetDelegate((uint)FunctionId.NvAPI_GPU_GetTachReading, out NvAPI_GPU_GetTachReading);
                GetDelegate((uint)FunctionId.NvAPI_GPU_GetAllClockFrequencies, out NvAPI_GPU_GetAllClocks);
                GetDelegate((uint)FunctionId.NvAPI_GPU_GetDynamicPStatesInfoEx, out NvAPI_GPU_GetPStates);
               // GetDelegate(0x189A1FDF, out NvAPI_GPU_GetUsages);
               // GetDelegate(0xDA141340, out NvAPI_GPU_GetCoolerSettings);
                GetDelegate((uint)FunctionId.NvAPI_GPU_GetMemoryInfo, out NvAPI_GPU_GetMemoryInfo);
                GetDelegate((uint)FunctionId.NvAPI_GetDisplayDriverVersion, out NvAPI_GetDisplayDriverVersion);
                GetDelegate((uint)FunctionId.NvAPI_GetInterfaceVersionString, out _NvAPI_GetInterfaceVersionString);

                available = true;
            }
        }

        public static bool IsAvailable
        {
            get { return available; }
        }

        internal enum FunctionId : uint
        {
            NvAPI_Initialize = 0x150E828,
            NvAPI_GetErrorMessage = 0x6C2D048C,
            NvAPI_GetInterfaceVersionString = 0x1053FA5,
            NvAPI_GPU_GetEDID = 0x37D32E69,
            NvAPI_SetView = 0x957D7B6,
            NvAPI_SetViewEx = 0x6B89E68,
            NvAPI_GetDisplayDriverVersion = 0xF951A4D1,
            NvAPI_SYS_GetDriverAndBranchVersion = 0x2926AAAD,
            NvAPI_GPU_GetMemoryInfo = 0x7F9B368,
            NvAPI_OGL_ExpertModeSet = 0x3805EF7A,
            NvAPI_OGL_ExpertModeGet = 0x22ED9516,
            NvAPI_OGL_ExpertModeDefaultsSet = 0xB47A657E,
            NvAPI_OGL_ExpertModeDefaultsGet = 0xAE921F12,
            NvAPI_EnumPhysicalGPUs = 0xE5AC921F,
            NvAPI_EnumTCCPhysicalGPUs = 0xD9930B07,
            NvAPI_EnumLogicalGPUs = 0x48B3EA59,
            NvAPI_GetPhysicalGPUsFromDisplay = 0x34EF9506,
            NvAPI_GetPhysicalGPUFromUnAttachedDisplay = 0x5018ED61,
            NvAPI_GetLogicalGPUFromDisplay = 0xEE1370CF,
            NvAPI_GetLogicalGPUFromPhysicalGPU = 0xADD604D1,
            NvAPI_GetPhysicalGPUsFromLogicalGPU = 0xAEA3FA32,
            NvAPI_GPU_GetShaderSubPipeCount = 0xBE17923,
            NvAPI_GPU_GetGpuCoreCount = 0xC7026A87,
            NvAPI_GPU_GetAllOutputs = 0x7D554F8E,
            NvAPI_GPU_GetConnectedOutputs = 0x1730BFC9,
            NvAPI_GPU_GetConnectedSLIOutputs = 0x680DE09,
            NvAPI_GPU_GetConnectedDisplayIds = 0x78DBA2,
            NvAPI_GPU_GetAllDisplayIds = 0x785210A2,
            NvAPI_GPU_GetConnectedOutputsWithLidState = 0xCF8CAF39,
            NvAPI_GPU_GetConnectedSLIOutputsWithLidState = 0x96043CC7,
            NvAPI_GPU_GetSystemType = 0xBAAABFCC,
            NvAPI_GPU_GetActiveOutputs = 0xE3E89B6F,
            NvAPI_GPU_SetEDID = 0xE83D6456,
            NvAPI_GPU_GetOutputType = 0x40A505E4,
            NvAPI_GPU_ValidateOutputCombination = 0x34C9C2D4,
            NvAPI_GPU_GetFullName = 0xCEEE8E9F,
            NvAPI_GPU_GetPCIIdentifiers = 0x2DDFB66E,
            NvAPI_GPU_GetGPUType = 0xC33BAEB1,
            NvAPI_GPU_GetBusType = 0x1BB18724,
            NvAPI_GPU_GetBusId = 0x1BE0B8E5,
            NvAPI_GPU_GetBusSlotId = 0x2A0A350F,
            NvAPI_GPU_GetIRQ = 0xE4715417,
            NvAPI_GPU_GetVbiosRevision = 0xACC3DA0A,
            NvAPI_GPU_GetVbiosOEMRevision = 0x2D43FB31,
            NvAPI_GPU_GetVbiosVersionString = 0xA561FD7D,
            NvAPI_GPU_GetAGPAperture = 0x6E042794,
            NvAPI_GPU_GetCurrentAGPRate = 0xC74925A0,
            NvAPI_GPU_GetCurrentPCIEDownstreamWidth = 0xD048C3B1,
            NvAPI_GPU_GetPhysicalFrameBufferSize = 0x46FBEB03,
            NvAPI_GPU_GetVirtualFrameBufferSize = 0x5A04B644,
            NvAPI_GPU_GetQuadroStatus = 0xE332FA47,
            NvAPI_GPU_GetBoardInfo = 0x22D54523,
            NvAPI_GPU_GetAllClockFrequencies = 0xDCB616C3,
            NvAPI_GPU_GetPStatesInfoEx = 0x843C0256,
            NvAPI_GPU_GetPStates20 = 0x6FF81213,
            NvAPI_GPU_GetCurrentPstate = 0x927DA4F6,
            NvAPI_GPU_GetDynamicPStatesInfoEx = 0x60DED2ED,
            NvAPI_GPU_GetThermalSettings = 0xE3640A56,
            NvAPI_I2CRead = 0x2FDE12C5,
            NvAPI_I2CWrite = 0xE812EB07,
            NvAPI_GPU_WorkstationFeatureSetup = 0x6C1F3FE4,
            NvAPI_GPU_WorkstationFeatureQuery = 0x4537DF,
            NvAPI_GPU_GetHDCPSupportStatus = 0xF089EEF5,
            NvAPI_GPU_GetTachReading = 0x5F608315,
            NvAPI_GPU_GetECCStatusInfo = 0xCA1DDAF3,
            NvAPI_GPU_GetECCErrorInfo = 0xC71F85A6,
            NvAPI_GPU_ResetECCErrorInfo = 0xC02EEC20,
            NvAPI_GPU_GetECCConfigurationInfo = 0x77A796F3,
            NvAPI_GPU_SetECCConfiguration = 0x1CF639D9,
            NvAPI_GPU_SetScanoutIntensity = 0xA57457A4,
            NvAPI_GPU_GetScanoutIntensityState = 0xE81CE836,
            NvAPI_GPU_SetScanoutWarping = 0xB34BAB4F,
            NvAPI_GPU_GetScanoutWarpingState = 0x6F5435AF,
            NvAPI_GPU_SetScanoutCompositionParameter = 0xF898247D,
            NvAPI_GPU_GetScanoutCompositionParameter = 0x58FE51E6,
            NvAPI_GPU_GetScanoutConfiguration = 0x6A9F5B63,
            NvAPI_GPU_GetScanoutConfigurationEx = 0xE2E1E6F0,
            NvAPI_GPU_GetPerfDecreaseInfo = 0x7F7F4600,
            NvAPI_GPU_QueryIlluminationSupport = 0xA629DA31,
            NvAPI_GPU_GetIllumination = 0x9A1B9365,
            NvAPI_GPU_SetIllumination = 0x254A187,
            NvAPI_EnumNvidiaDisplayHandle = 0x9ABDD40D,
            NvAPI_EnumNvidiaUnAttachedDisplayHandle = 0x20DE9260,
            NvAPI_CreateDisplayFromUnAttachedDisplay = 0x63F9799E,
            NvAPI_GetAssociatedNvidiaDisplayHandle = 0x35C29134,
            NvAPI_DISP_GetAssociatedUnAttachedNvidiaDisplayHandle = 0xA70503B2,
            NvAPI_GetAssociatedNvidiaDisplayName = 0x22A78B05,
            NvAPI_GetUnAttachedAssociatedDisplayName = 0x4888D790,
            NvAPI_EnableHWCursor = 0x2863148D,
            NvAPI_DisableHWCursor = 0xAB163097,
            NvAPI_GetVBlankCounter = 0x67B5DB55,
            NvAPI_SetRefreshRateOverride = 0x3092AC32,
            NvAPI_GetAssociatedDisplayOutputId = 0xD995937E,
            NvAPI_GetDisplayPortInfo = 0xC64FF367,
            NvAPI_SetDisplayPort = 0xFA13E65A,
            NvAPI_GetHDMISupportInfo = 0x6AE16EC3,
            NvAPI_Disp_InfoFrameControl = 0x6067AF3F,
            NvAPI_Disp_ColorControl = 0x92F9D80D,
            NvAPI_Disp_GetHdrCapabilities = 0x84F2A8DF,
            NvAPI_Disp_HdrColorControl = 0x351DA224,
            NvAPI_DISP_GetTiming = 0x175167E9,
            NvAPI_DISP_GetMonitorCapabilities = 0x3B05C7E1,
            NvAPI_DISP_GetMonitorColorCapabilities = 0x6AE4CFB5,
            NvAPI_DISP_EnumCustomDisplay = 0xA2072D59,
            NvAPI_DISP_TryCustomDisplay = 0x1F7DB630,
            NvAPI_DISP_DeleteCustomDisplay = 0x552E5B9B,
            NvAPI_DISP_SaveCustomDisplay = 0x49882876,
            NvAPI_DISP_RevertCustomDisplayTrial = 0xCBBD40F0,
            NvAPI_GetView = 0xD6B99D89,
            NvAPI_GetViewEx = 0xDBBC0AF4,
            NvAPI_GetSupportedViews = 0x66FB7FC0,
            NvAPI_DISP_GetDisplayIdByDisplayName = 0xAE457190,
            NvAPI_DISP_GetGDIPrimaryDisplayId = 0x1E9D8A31,
            NvAPI_DISP_GetDisplayConfig = 0x11ABCCF8,
            NvAPI_DISP_SetDisplayConfig = 0x5D8CF8DE,
            NvAPI_Mosaic_GetSupportedTopoInfo = 0xFDB63C81,
            NvAPI_Mosaic_GetTopoGroup = 0xCB89381D,
            NvAPI_Mosaic_GetOverlapLimits = 0x989685F0,
            NvAPI_Mosaic_SetCurrentTopo = 0x9B542831,
            NvAPI_Mosaic_GetCurrentTopo = 0xEC32944E,
            NvAPI_Mosaic_EnableCurrentTopo = 0x5F1AA66C,
            NvAPI_Mosaic_GetDisplayViewportsByResolution = 0xDC6DC8D3,
            NvAPI_Mosaic_SetDisplayGrids = 0x4D959A89,
            NvAPI_Mosaic_ValidateDisplayGrids = 0xCF43903D,
            NvAPI_Mosaic_EnumDisplayModes = 0x78DB97D7,
            NvAPI_Mosaic_EnumDisplayGrids = 0xDF2887AF,
            NvAPI_GetSupportedMosaicTopologies = 0x410B5C25,
            NvAPI_GetCurrentMosaicTopology = 0xF60852BD,
            NvAPI_SetCurrentMosaicTopology = 0xD54B8989,
            NvAPI_EnableCurrentMosaicTopology = 0x74073CC9,
            NvAPI_GSync_EnumSyncDevices = 0xD9639601,
            NvAPI_GSync_QueryCapabilities = 0x44A3F1D1,
            NvAPI_GSync_GetTopology = 0x4562BC38,
            NvAPI_GSync_SetSyncStateSettings = 0x60ACDFDD,
            NvAPI_GSync_GetControlParameters = 0x16DE1C6A,
            NvAPI_GSync_SetControlParameters = 0x8BBFF88B,
            NvAPI_GSync_AdjustSyncDelay = 0x2D11FF51,
            NvAPI_GSync_GetSyncStatus = 0xF1F5B434,
            NvAPI_GSync_GetStatusParameters = 0x70D404EC,
            NvAPI_D3D_GetCurrentSLIState = 0x4B708B54,
            NvAPI_D3D9_RegisterResource = 0xA064BDFC,
            NvAPI_D3D9_UnregisterResource = 0xBB2B17AA,
            NvAPI_D3D9_AliasSurfaceAsTexture = 0xE5CEAE41,
            NvAPI_D3D9_StretchRectEx = 0x22DE03AA,
            NvAPI_D3D9_ClearRT = 0x332D3942,
            NvAPI_D3D_GetObjectHandleForResource = 0xFCEAC864,
            NvAPI_D3D_SetResourceHint = 0x6C0ED98C,
            NvAPI_D3D_BeginResourceRendering = 0x91123D6A,
            NvAPI_D3D_EndResourceRendering = 0x37E7191C,
            NvAPI_D3D9_GetSurfaceHandle = 0xF2DD3F2,
            NvAPI_D3D9_VideoSetStereoInfo = 0xB852F4DB,
            NvAPI_D3D11_CreateDevice = 0x6A16D3A0,
            NvAPI_D3D11_CreateDeviceAndSwapChain = 0xBB939EE5,
            NvAPI_D3D11_SetDepthBoundsTest = 0x7AAF7A04,
            NvAPI_D3D11_IsNvShaderExtnOpCodeSupported = 0x5F68DA40,
            NvAPI_D3D11_SetNvShaderExtnSlot = 0x8E90BB9F,
            NvAPI_D3D11_BeginUAVOverlapEx = 0xBA08208A,
            NvAPI_D3D11_BeginUAVOverlap = 0x65B93CA8,
            NvAPI_D3D11_EndUAVOverlap = 0x2216A357,
            NvAPI_D3D_SetFPSIndicatorState = 0xA776E8DB,
            NvAPI_D3D11_CreateRasterizerState = 0xDB8D28AF,
            NvAPI_D3D_ConfigureANSEL = 0x341C6C7F,
            NvAPI_D3D11_AliasMSAATexture2DAsNonMSAA = 0xF1C54FC9,
            NvAPI_D3D11_CreateGeometryShaderEx_2 = 0x99ED5C1C,
            NvAPI_D3D11_CreateVertexShaderEx = 0xBEAA0B2,
            NvAPI_D3D11_CreateHullShaderEx = 0xB53CAB00,
            NvAPI_D3D11_CreateDomainShaderEx = 0xA0D7180D,
            NvAPI_D3D11_CreateFastGeometryShaderExplicit = 0x71AB7C9C,
            NvAPI_D3D11_CreateFastGeometryShader = 0x525D43BE,
            NvAPI_D3D12_CreateGraphicsPipelineState = 0x2FC28856,
            NvAPI_D3D12_CreateComputePipelineState = 0x2762DEAC,
            NvAPI_D3D12_SetDepthBoundsTestValues = 0xB9333FE9,
            NvAPI_D3D12_IsNvShaderExtnOpCodeSupported = 0x3DFACEC8,
            NvAPI_D3D_IsGSyncCapable = 0x9C1EED78,
            NvAPI_D3D_IsGSyncActive = 0xE942B0FF,
            NvAPI_D3D1x_DisableShaderDiskCache = 0xD0CBCA7D,
            NvAPI_D3D11_MultiGPU_GetCaps = 0xD2D25687,
            NvAPI_D3D11_MultiGPU_Init = 0x17BE49E,
            NvAPI_D3D11_CreateMultiGPUDevice = 0xBDB20007,
            NvAPI_D3D_QuerySinglePassStereoSupport = 0x6F5F0A6D,
            NvAPI_D3D_SetSinglePassStereoMode = 0xA39E6E6E,
            NvAPI_D3D12_QuerySinglePassStereoSupport = 0x3B03791B,
            NvAPI_D3D12_SetSinglePassStereoMode = 0x83556D87,
            NvAPI_D3D_QueryModifiedWSupport = 0xCBF9F4F5,
            NvAPI_D3D_SetModifiedWMode = 0x6EA4BF4,
            NvAPI_D3D12_QueryModifiedWSupport = 0x51235248,
            NvAPI_D3D12_SetModifiedWMode = 0xE1FDABA7,
            NvAPI_D3D_RegisterDevice = 0x8C02C4D0,
            NvAPI_D3D11_MultiDrawInstancedIndirect = 0xD4E26BBF,
            NvAPI_D3D11_MultiDrawIndexedInstancedIndirect = 0x59E890F9,
            NvAPI_D3D_ImplicitSLIControl = 0x2AEDE111,
            NvAPI_VIO_GetCapabilities = 0x1DC91303,
            NvAPI_VIO_Open = 0x44EE4841,
            NvAPI_VIO_Close = 0xD01BD237,
            NvAPI_VIO_Status = 0xE6CE4F1,
            NvAPI_VIO_SyncFormatDetect = 0x118D48A3,
            NvAPI_VIO_GetConfig = 0xD34A789B,
            NvAPI_VIO_SetConfig = 0xE4EEC07,
            NvAPI_VIO_SetCSC = 0xA1EC8D74,
            NvAPI_VIO_GetCSC = 0x7B0D72A3,
            NvAPI_VIO_SetGamma = 0x964BF452,
            NvAPI_VIO_GetGamma = 0x51D53D06,
            NvAPI_VIO_SetSyncDelay = 0x2697A8D1,
            NvAPI_VIO_GetSyncDelay = 0x462214A9,
            NvAPI_VIO_GetPCIInfo = 0xB981D935,
            NvAPI_VIO_IsRunning = 0x96BD040E,
            NvAPI_VIO_Start = 0xCDE8E1A3,
            NvAPI_VIO_Stop = 0x6BA2A5D6,
            NvAPI_VIO_IsFrameLockModeCompatible = 0x7BF0A94D,
            NvAPI_VIO_EnumDevices = 0xFD7C5557,
            NvAPI_VIO_QueryTopology = 0x869534E2,
            NvAPI_VIO_EnumSignalFormats = 0xEAD72FE4,
            NvAPI_VIO_EnumDataFormats = 0x221FA8E8,
            NvAPI_Stereo_CreateConfigurationProfileRegistryKey = 0xBE7692EC,
            NvAPI_Stereo_DeleteConfigurationProfileRegistryKey = 0xF117B834,
            NvAPI_Stereo_SetConfigurationProfileValue = 0x24409F48,
            NvAPI_Stereo_DeleteConfigurationProfileValue = 0x49BCEECF,
            NvAPI_Stereo_Enable = 0x239C4545,
            NvAPI_Stereo_Disable = 0x2EC50C2B,
            NvAPI_Stereo_IsEnabled = 0x348FF8E1,
            NvAPI_Stereo_GetStereoSupport = 0x296C434D,
            NvAPI_Stereo_CreateHandleFromIUnknown = 0xAC7E37F4,
            NvAPI_Stereo_DestroyHandle = 0x3A153134,
            NvAPI_Stereo_Activate = 0xF6A1AD68,
            NvAPI_Stereo_Deactivate = 0x2D68DE96,
            NvAPI_Stereo_IsActivated = 0x1FB0BC30,
            NvAPI_Stereo_GetSeparation = 0x451F2134,
            NvAPI_Stereo_SetSeparation = 0x5C069FA3,
            NvAPI_Stereo_DecreaseSeparation = 0xDA044458,
            NvAPI_Stereo_IncreaseSeparation = 0xC9A8ECEC,
            NvAPI_Stereo_GetConvergence = 0x4AB00934,
            NvAPI_Stereo_SetConvergence = 0x3DD6B54B,
            NvAPI_Stereo_DecreaseConvergence = 0x4C87E317,
            NvAPI_Stereo_IncreaseConvergence = 0xA17DAABE,
            NvAPI_Stereo_GetFrustumAdjustMode = 0xE6839B43,
            NvAPI_Stereo_SetFrustumAdjustMode = 0x7BE27FA2,
            NvAPI_Stereo_CaptureJpegImage = 0x932CB140,
            NvAPI_Stereo_InitActivation = 0xC7177702,
            NvAPI_Stereo_Trigger_Activation = 0xD6C6CD2,
            NvAPI_Stereo_CapturePngImage = 0x8B7E99B5,
            NvAPI_Stereo_ReverseStereoBlitControl = 0x3CD58F89,
            NvAPI_Stereo_SetNotificationMessage = 0x6B9B409E,
            NvAPI_Stereo_SetActiveEye = 0x96EEA9F8,
            NvAPI_Stereo_SetDriverMode = 0x5E8F0BEC,
            NvAPI_Stereo_GetEyeSeparation = 0xCE653127,
            NvAPI_Stereo_IsWindowedModeSupported = 0x40C8ED5E,
            NvAPI_Stereo_SetSurfaceCreationMode = 0xF5DCFCBA,
            NvAPI_Stereo_GetSurfaceCreationMode = 0x36F1C736,
            NvAPI_Stereo_Debug_WasLastDrawStereoized = 0xED4416C5,
            NvAPI_Stereo_SetDefaultProfile = 0x44F0ECD1,
            NvAPI_Stereo_GetDefaultProfile = 0x624E21C2,
            NvAPI_D3D1x_CreateSwapChain = 0x1BC21B66,
            NvAPI_D3D9_CreateSwapChain = 0x1A131E09,
            NvAPI_DRS_CreateSession = 0x694D52E,
            NvAPI_DRS_DestroySession = 0xDAD9CFF8,
            NvAPI_DRS_LoadSettings = 0x375DBD6B,
            NvAPI_DRS_SaveSettings = 0xFCBC7E14,
            NvAPI_DRS_LoadSettingsFromFile = 0xD3EDE889,
            NvAPI_DRS_SaveSettingsToFile = 0x2BE25DF8,
            NvAPI_DRS_CreateProfile = 0xCC176068,
            NvAPI_DRS_DeleteProfile = 0x17093206,
            NvAPI_DRS_SetCurrentGlobalProfile = 0x1C89C5DF,
            NvAPI_DRS_GetCurrentGlobalProfile = 0x617BFF9F,
            NvAPI_DRS_GetProfileInfo = 0x61CD6FD6,
            NvAPI_DRS_SetProfileInfo = 0x16ABD3A9,
            NvAPI_DRS_FindProfileByName = 0x7E4A9A0B,
            NvAPI_DRS_EnumProfiles = 0xBC371EE0,
            NvAPI_DRS_GetNumProfiles = 0x1DAE4FBC,
            NvAPI_DRS_CreateApplication = 0x4347A9DE,
            NvAPI_DRS_DeleteApplicationEx = 0xC5EA85A1,
            NvAPI_DRS_DeleteApplication = 0x2C694BC6,
            NvAPI_DRS_GetApplicationInfo = 0xED1F8C69,
            NvAPI_DRS_EnumApplications = 0x7FA2173A,
            NvAPI_DRS_FindApplicationByName = 0xEEE566B2,
            NvAPI_DRS_SetSetting = 0x577DD202,
            NvAPI_DRS_GetSetting = 0x73BF8338,
            NvAPI_DRS_EnumSettings = 0xAE3039DA,
            NvAPI_DRS_EnumAvailableSettingIds = 0xF020614A,
            NvAPI_DRS_EnumAvailableSettingValues = 0x2EC39F90,
            NvAPI_DRS_GetSettingIdFromName = 0xCB7309CD,
            NvAPI_DRS_GetSettingNameFromId = 0xD61CBE6E,
            NvAPI_DRS_DeleteProfileSetting = 0xE4A26362,
            NvAPI_DRS_RestoreAllDefaults = 0x5927B094,
            NvAPI_DRS_RestoreProfileDefault = 0xFA5F6134,
            NvAPI_DRS_RestoreProfileDefaultSetting = 0x53F0381E,
            NvAPI_DRS_GetBaseProfile = 0xDA8466A0,
            NvAPI_SYS_GetChipSetInfo = 0x53DABBCA,
            NvAPI_SYS_GetLidAndDockInfo = 0xCDA14D8A,
            NvAPI_SYS_GetDisplayIdFromGpuAndOutputId = 0x8F2BAB4,
            NvAPI_SYS_GetGpuAndOutputIdFromDisplayId = 0x112BA1A5,
            NvAPI_SYS_GetPhysicalGpuFromDisplayId = 0x9EA74659,
            NvAPI_Unload = 0xD22BDD7E
        }


        }


}
