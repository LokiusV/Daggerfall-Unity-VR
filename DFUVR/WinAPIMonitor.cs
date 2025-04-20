////this entire file was AI generated because I didn't want to go through the entirety of the WinAPI documentation just to get a single feature working.
// This approach works fine but since I haven't really taken a look at it myself I don't trust it yet, so it remains commented out until I take the time to understand it.
//using System;
//using System.Runtime.InteropServices;
//using UnityEngine;
//using System.Collections.Generic;
//namespace DFUVR
//{
//    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
//    public struct DEVMODE
//    {
//        private const int CCHDEVICENAME = 32;
//        private const int CCHFORMNAME = 32;
//        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
//        public string dmDeviceName;
//        public ushort dmSpecVersion;
//        public ushort dmDriverVersion;
//        public ushort dmSize;
//        public ushort dmDriverExtra;
//        public uint dmFields;
//        public int dmPositionX;
//        public int dmPositionY;
//        public uint dmDisplayOrientation;
//        public uint dmDisplayFixedOutput;
//        public short dmColor;
//        public short dmDuplex;
//        public short dmYResolution;
//        public short dmTTOption;
//        public short dmCollate;
//        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
//        public string dmFormName;
//        public ushort dmLogPixels;
//        public uint dmBitsPerPel;
//        public uint dmPelsWidth;
//        public uint dmPelsHeight;
//        public uint dmDisplayFlags;
//        public uint dmDisplayFrequency;
//        public uint dmICMMethod;
//        public uint dmICMIntent;
//        public uint dmMediaType;
//        public uint dmDitherType;
//        public uint dmReserved1;
//        public uint dmReserved2;
//        public uint dmPanningWidth;
//        public uint dmPanningHeight;
//    }

//    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
//    public struct DISPLAY_DEVICE
//    {
//        [MarshalAs(UnmanagedType.U4)]
//        public int cb;
//        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
//        public string DeviceName;
//        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
//        public string DeviceString;
//        [MarshalAs(UnmanagedType.U4)]
//        public int StateFlags;
//        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
//        public string DeviceID;
//        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
//        public string DeviceKey;
//    }

//    public class WindowsAPI
//    {
//        public const int ENUM_CURRENT_SETTINGS = -1;
//        public const int CDS_UPDATEREGISTRY = 0x01;
//        public const int CDS_TEST = 0x02;
//        public const int DISP_CHANGE_SUCCESSFUL = 0;

//        public const int DM_PELSWIDTH = 0x80000;
//        public const int DM_PELSHEIGHT = 0x100000;

//        [DllImport("user32.dll")]
//        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

//        [DllImport("user32.dll")]
//        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

//        [DllImport("user32.dll")]
//        public static extern int ChangeDisplaySettingsEx(string deviceName, ref DEVMODE devMode, IntPtr hwnd, int dwflags, IntPtr lParam);
//    }
//    public class DisplayManager : MonoBehaviour
//    {
//        private struct DisplayState
//        {
//            public string DeviceName;
//            public DEVMODE OriginalMode;

//            public DisplayState(string deviceName, DEVMODE mode)
//            {
//                DeviceName = deviceName;
//                OriginalMode = mode;
//            }
//        }

//        private List<DisplayState> originalSettings = new List<DisplayState>();

//        void Start()
//        {
//            SetAllDisplaysResolution(1920, 1080);
//        }

//        void OnApplicationQuit()
//        {
//            RestoreOriginalDisplaySettings();
//        }

//        void SetAllDisplaysResolution(int width, int height)
//        {
//            uint devNum = 0;
//            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
//            d.cb = Marshal.SizeOf(d);

//            while (WindowsAPI.EnumDisplayDevices(null, devNum, ref d, 0))
//            {
//                if ((d.StateFlags & 0x00000001) != 0) // DISPLAY_DEVICE_ACTIVE
//                {
//                    DEVMODE dm = new DEVMODE();
//                    dm.dmSize = (ushort)Marshal.SizeOf(dm);
//                    if (WindowsAPI.EnumDisplaySettings(d.DeviceName, WindowsAPI.ENUM_CURRENT_SETTINGS, ref dm))
//                    {
//                        // Save original setting
//                        originalSettings.Add(new DisplayState(d.DeviceName, dm));

//                        // Change resolution
//                        dm.dmPelsWidth = (uint)width;
//                        dm.dmPelsHeight = (uint)height;
//                        dm.dmFields = WindowsAPI.DM_PELSWIDTH | WindowsAPI.DM_PELSHEIGHT;

//                        int result = WindowsAPI.ChangeDisplaySettingsEx(d.DeviceName, ref dm, IntPtr.Zero, WindowsAPI.CDS_UPDATEREGISTRY, IntPtr.Zero);
//                        if (result != WindowsAPI.DISP_CHANGE_SUCCESSFUL)
//                        {
//                            Debug.LogError("Failed to change resolution for: " + d.DeviceName);
//                        }
//                    }
//                }
//                devNum++;
//                d.cb = Marshal.SizeOf(d);
//            }
//        }

//        void RestoreOriginalDisplaySettings()
//        {
//            for (int i = 0; i < originalSettings.Count; i++)
//            {
//                DisplayState ds = originalSettings[i];
//                int result = WindowsAPI.ChangeDisplaySettingsEx(ds.DeviceName, ref ds.OriginalMode, IntPtr.Zero, WindowsAPI.CDS_UPDATEREGISTRY, IntPtr.Zero);
//                if (result != WindowsAPI.DISP_CHANGE_SUCCESSFUL)
//                {
//                    Debug.LogError("Failed to restore resolution for: " + ds.DeviceName);
//                }
//            }
//        }
//    }
//}