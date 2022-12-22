using System;
using System.Runtime.InteropServices;

public static class VorpX
{
    const string dllName = @"C:\Users\SKIKK\Documents\vorpAPI.dll";

    // Import the C API functions as external functions in C#

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern VPX_RESULT vpxInit();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern VPX_RESULT vpxFree();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern VPX_BOOL vpxIsActive();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern float vpxGetHeadsetFOV();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern float vpxVertToHorFOV(float fov_deg, float aspect);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern vpxfloat3 vpxGetHeadsetRotationEuler();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern vpxfloat4 vpxGetHeadsetRotationQuaternion();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern vpxfloat3 vpxGetHeadsetPosition();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern VPX_CONTROLLER_STATE vpxGetControllerState(uint controllerNum);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern vpxfloat3 vpxGetControllerRotationEuler(uint cotrollerNum);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern vpxfloat4 vpxGetControllerRotationQuaternion(uint controllerNum);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern vpxfloat3 vpxGetControllerPosition(uint controllerNum);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vpxForceControllerRendering(VPX_BOOL val);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern vpxfloat3 vpxYawCorrection(vpxfloat3 position, float yawInDegrees);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vpxRequestEdgePeek(VPX_BOOL val);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern VPX_BOOL vpxGetEdgePeekRequested();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern VPX_BOOL vpxGetEdgePeekActual();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern vpxfloat3 vpxGetEyePosition(uint eye, float ipd);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern float vpxGetG3DStrength();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vpxGetCurrentVorpFrame();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern VPX_BOOL vpxIsVorpDoing3D();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vpxSetGameCamRotationDeltaEuler(vpxfloat3 rotation);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vpxSetStereoReduction(float reduction);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vpxInitStereo();

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vpxSetGameStereoRenderMode(VPX_GAME_STEREO_MODE stereoMode);

    [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vpxSetAlternateFrame3DEye(uint eye);

    public static float vpxDegToRad(float a)
    {
        return a * 0.0174532925f;
    }

    public static float vpxRadToDeg(float a)
    {
       return a * 57.295795131f;
    }

    // Define C# versions of the C structs
    [StructLayout(LayoutKind.Sequential)]
    public struct vpxint2
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct vpxint3
    {
        public int x;
        public int y;
        public int z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct vpxint4
    {
        public int x;
        public int y;
        public int z;
        public int w;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct vpxfloat2
    {
        public float x;
        public float y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct vpxfloat3
    {
        public float x;
        public float y;
        public float z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct vpxfloat4
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct vpxmtx4x4
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public float[] m;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VPX_CONTROLLER_STATE
    {
        public bool IsActive;      // VPX_TRUE if active, otherwise VPX_FALSE
        public float StickX;       // Thumbstick/pad x-axis [-1|1]
        public float StickY;       // Thumbstick/pad y-axis [-1|1]
        public float Trigger;      // Trigger axis [0|1]
        public float Grip;         // Grip axis [0|1], on controllers with a grip button (e.g. Vive wands) either 0.0 or 1.0
        public float Extra0;       // Extra axis (for future use)
        public float Extra1;       // Extra axis (for future use)
        public float Extra2;       // Extra axis (for future use)
        public float Extra3;       // Extra axis (for future use)
        public float Finger0;      // Finger axis: thumb (for future use)
        public float Finger1;      // Finger axis: index (for future use)
        public float Finger2;      // Finger axis: middle (for future use)
        public float Finger3;      // Finger axis: ring (for future use)
        public float Finger4;      // Finger axis: pinky (for future use)
        public uint ButtonsPressed; // Check with a flag, e.g.: if (ButtonsPressed & VPX_CONTROLLER_BUTTON_0)
        public uint ButtonsTouched; // Check with a flag, e.g.: if (ButtonsTouched & VPX_CONTROLLER_BUTTON_0)
    }

    public enum VPX_GAME_STEREO_MODE
    {
        VPX_GAME_STEREO_MODE_NONE = 0,
        VPX_GAME_STEREO_MODE_SINGLE_FRAME = 1,
        VPX_GAME_STEREO_MODE_ALTERNATE_FRAME = 2
    }

    public enum VPX_BOOL
    {
        VPX_FALSE = 0,
        VPX_TRUE = 1,
    }

    public enum VPX_RESULT
    {
        VPX_RES_OK = 0,
        VPX_RES_FAIL = 1,
        VPX_RES_INVALID_ARGUMENT = 101,
        VPX_RES_NULL_POINTER = 102,
        VPX_RES_NOT_INITIALIZED = 103,
        VPX_RES_FUNCTION_UNAVAILABLE = 104
    }

    public enum VPX_CONTROLLER_BUTTON
    {
        VPX_CONTROLLER_BUTTON_0 = 0x001,
        VPX_CONTROLLER_BUTTON_1 = 0x002,
        VPX_CONTROLLER_BUTTON_MENU = 0x004,
        VPX_CONTROLLER_BUTTON_STICK_PAD_0 = 0x008,
        VPX_CONTROLLER_BUTTON_TRIGGER = 0x010,
        VPX_CONTROLLER_BUTTON_GRIP = 0x020
    }
}

