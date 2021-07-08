using System;
using System.Runtime.InteropServices;


//[StructLayout(LayoutKind.Explicit, Pack = 1)]
//public struct C_SunGlowOverly
//{
//    [FieldOffset(0x04)]
//    public SharpDX.Vector3 m_vecOrigin;

//    [FieldOffset(0x10)]
//    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
//    public bool m_bDirectional;

//    [FieldOffset(0x14)]
//    public SharpDX.Vector3 m_vecDirection;

//    [FieldOffset(0x24)]
//    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
//    public bool m_bSkyObstructionScale;

//    [FieldOffset(0x30)]
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
//    public CGlowSprite[] m_glowSprites;

//    [FieldOffset(0x90)]
//    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
//    public byte m_nSprites;

//}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct CEntInfo
{
    #region FIELDS
    public IntPtr m_pEntity;
    public int m_SerialNumber;
    public IntPtr m_pPrevious;
    public IntPtr m_pNext;
    #endregion
}
//public struct CGlowSprite
//{
//    public SharpDX.Vector3 RGB;
//    public float m_flHorzSize;
//    public float m_flVertSize;
//    public IntPtr m_pMaterialSystem;
//}

//[StructLayout(LayoutKind.Explicit)]
//public unsafe struct CInput
//{
//    [FieldOffset(0)] public int pvftable;
//    [FieldOffset(12)] public byte m_fTrackIRAvailable;
//    [FieldOffset(13)] public byte m_fMouseInitialized;
//    [FieldOffset(14)] public byte m_fMouseActive;
//    [FieldOffset(15)] public byte m_fJoystickAdvancedInit;
//    [FieldOffset(60)] public void* m_pKeys;
//    [FieldOffset(172)] public byte m_fCameraInterceptingMouse;
//    [FieldOffset(173)] public byte m_fCameraInThirdPerson;
//    [FieldOffset(174)] public byte m_fCameraMovingWithMouse;
//    [FieldOffset(175)] public byte m_fCameraDistanceMove;
//    [FieldOffset(176)] public SharpDX.Vector3 m_vecCameraOffset;
//    [FieldOffset(188)] public int m_nCameraOldX;
//    [FieldOffset(192)] public int m_nCameraOldY;
//    [FieldOffset(196)] public int m_nCameraX;
//    [FieldOffset(200)] public int m_nCameraY;
//    [FieldOffset(204)] public byte m_CameraIsOrthographic;
//    [FieldOffset(236)] public float m_flLastForwardMove;
//    [FieldOffset(240)] public int m_nClearInputState;
//    [FieldOffset(244)] public CUserCmd m_pCommands;
//    [FieldOffset(248)] public VerifiedUserCMD m_pVerifiedCommands;
//}
struct _CColor
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;
}
//public struct VerifiedUserCMD
//{
//    public CUserCmd m_cmd;
//    public UInt32 m_crc;
//};

//[StructLayout(LayoutKind.Explicit)]
//public unsafe struct CUserCmd
//{
//    [FieldOffset(0x00)] public int pvftable;
//    [FieldOffset(0x04)] public int m_iCmdNumber;
//    [FieldOffset(0x08)] public int m_iTickCount;
//    [FieldOffset(0x0C)] public SharpDX.Vector3 m_vecViewAngles;
//    [FieldOffset(0x24)] public float m_flForwardmove;
//    [FieldOffset(0x28)] public float m_flSidemove;
//    [FieldOffset(0x2C)] public float m_flUpmove;
//    [FieldOffset(0x30)] public int m_iButtons;
//    [FieldOffset(0x34)] public char m_bImpulse;
//    [FieldOffset(0x38)] public int m_iWeaponSelect;
//    [FieldOffset(0x3C)] public int m_iWeaponSubtype;
//    [FieldOffset(0x40)] public int m_iRandomSeed;
//    [FieldOffset(0x44)] public short m_siMouseDx;
//    [FieldOffset(0x46)] public short m_siMouseDy;
//    [FieldOffset(0x48)] public byte m_bHasBeenPredicted;
//    [FieldOffset(0x4C)] public fixed byte pad[0x18];
//}

[StructLayout(LayoutKind.Explicit)]
public struct ClientClass_t
{
    [FieldOffset(0x08)]
    public IntPtr m_pNetworkName;
    [FieldOffset(0x0C)]
    public IntPtr m_pRecvTable;
    [FieldOffset(0x10)]
    public IntPtr m_pNext;
    [FieldOffset(0x14)]
    public ushort m_ClassID;
}
[StructLayout(LayoutKind.Explicit)]
public struct RecvTable
{
    [FieldOffset(0x00)]
    public IntPtr m_pProps;
    [FieldOffset(0x04)]
    public int m_nProps;
    [FieldOffset(0x08)]
    public IntPtr m_pDecoder;
    [FieldOffset(0x0C)]
    public IntPtr m_pNetTableName;
}
public enum ePropType : int
{
    Int = 0,
    Float,
    Vector,
    VectorXY,
    String,
    Array,
    DataTable,
    Int64,
    NUMSendPropTypes
};
[StructLayout(LayoutKind.Explicit)]
public struct RecvProp
{
    [FieldOffset(0x0)]
    public int m_pVarName;
    [FieldOffset(0x4)]
    public ePropType m_RecvType;
    [FieldOffset(0x8)]
    public int m_Flags;
    [FieldOffset(0xC)]
    public int m_StringBufferSize;
    [FieldOffset(0x10)]
    public bool m_bInsideArray;
    [FieldOffset(0x14)]
    public int m_pExtraData;
    [FieldOffset(0x18)]
    public int m_pArrayProp;
    [FieldOffset(0x1C)]
    public int m_ArrayLengthProxy;
    [FieldOffset(0x20)]
    public int m_ProxyFn;
    [FieldOffset(0x24)]
    public int m_DataTableProxyFn;
    [FieldOffset(0x28)]
    public int m_pDataTable;
    [FieldOffset(0x2C)]
    public int m_Offset;
    [FieldOffset(0x30)]
    public int m_ElementStride;
    [FieldOffset(0x34)]
    public int m_nElements;
    [FieldOffset(0x38)]
    public int m_pParentArrayPropName;
}
[StructLayout(LayoutKind.Explicit)]
struct GlowManager
{
    #region FIELDS
    [FieldOffset(0x00)]
    public IntPtr m_pGlowArray;
    [FieldOffset(0x04)]
    public int m_iCapacity;
    [FieldOffset(0x0C)]
    public int m_iNumObjects;
    #endregion
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct mweapon_indexes_t
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    public int[] _pointers;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GlobalVarsBase
{
    public float m_flRealTime;
    public int m_iFrameCount;
    public float m_flAbsoluteFrameTime;
    public float m_flAbsoluteFrameStartTime;
    public float m_flCurTime;
    public float m_flFrameTime;
    public int m_iMaxClients;
    public int m_iTickCount;
    public float m_flIntervalPerTick;
    public float m_flInterpolationAmount;
    public int m_iSimTicksThisFrame;
    public int m_iNetworkProtocol;
};
[StructLayout(LayoutKind.Explicit)]
public struct GlobalVars_t //Line 266 in csgo_playeranimstate
{
    [FieldOffset(0x0)]
    public float RealTime;

    [FieldOffset(0x04)]
    public int FrameCount;

    [FieldOffset(0x08)]
    public float AbsoluteFrametime;

    [FieldOffset(0x0C)]
    public float AbsoluteFrameStartTimestddev;

    [FieldOffset(0x10)]
    public float Curtime;

    [FieldOffset(0x14)]
    public float Frametime;

    [FieldOffset(0x18)]
    public int MaxClients;

    [FieldOffset(0x1C)]
    public int TickCount;

    [FieldOffset(0x20)]
    public float Interval_Per_Tick;

    [FieldOffset(0x24)]
    public float Interpolation_Amount;

    [FieldOffset(0x28)]
    public int SimTicksThisFrame;

    [FieldOffset(0x2c)]
    public int Network_Protocol;

    [FieldOffset(0x30)]
    public IntPtr pSaveData;

    [FieldOffset(0x34)]
    public bool m_bClient;

    [FieldOffset(0x38)]
    public bool m_bRemoteClient;

    [FieldOffset(0x3C)]
    public int nTimestampNetworkingBase;

    [FieldOffset(0x40)]
    public int nTimestampRandomizeWindow;
}
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct CGlobalVarsBase
{
    [FieldOffset(0x00)]
    public float realtime;

    [FieldOffset(0x04)]
    public int framecount;

    [FieldOffset(0x08)]
    public float absoluteframetime;

    [FieldOffset(0x10)]
    public float curtime;

    [FieldOffset(0x17)]
    public float frametime;

    [FieldOffset(0x18)]
    public int maxclients;

    [FieldOffset(0x1C)]
    public int tickcount;

    [FieldOffset(0x20)]
    public float interval_per_tick;

    [FieldOffset(0x24)]
    public float interpolation_amount;

    [FieldOffset(0x28)]
    public int simTicksThisFrame;

    [FieldOffset(0x2C)]
    public int network_protocol;

    //  [FieldOffset(0x24)]
    //0x30	void* pSaveData;
    //[FieldOffset(0x34)]
    //public bool m_bClient;
    //[FieldOffset(0x38)]
    //public int nTimestampNetworkingBase;
    //[FieldOffset(0x3C)]
    //public int nTimestampRandomizeWindow;
}


[StructLayout(LayoutKind.Explicit)]
struct m_dwGlowObject
{
    [FieldOffset(0x04)]
    public IntPtr dwEntity; //0x0000

    [FieldOffset(0x08)]
    public float R; //0x0004

    [FieldOffset(0x0C)]
    public float G; //0x0008

    [FieldOffset(0x10)]
    public float B; //0x000C

    [FieldOffset(0x14)]
    public float A; //0x0010

    [FieldOffset(0x1C)]
    public float m_flGlowAlphaFunctionOfMaxVelocity; //0x0010
    [FieldOffset(0x20)]
    public float m_flGlowAlphaMax; //0x0010
    //[FieldOffset(0x20)]
    ///public float m_flGlowPulseOverdrive; //0x0010

    //[FieldOffset(0x14)]
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    //public byte[] unkA; //0x0014 -> 0x20 JUNK

    [FieldOffset(0x28)]
    public bool bRenderWhenOccluded; //0x0024

    [FieldOffset(0x29)]
    public bool bRenderWhenUnoccluded; //0x0025

    [FieldOffset(0x2A)]
    public bool bFullBloom; //0x0026


    [FieldOffset(0x30)]
    public byte m_nRenderStyle; //0x002C
}

//[StructLayout(LayoutKind.Explicit)]
//struct m_dwGlowObject
//{
//    [FieldOffset(0x04)]
//    public IntPtr dwEntity; //0x0000

//    [FieldOffset(0x08)]
//    public float R; //0x0004

//    [FieldOffset(0x0C)]
//    public float G; //0x0008

//    [FieldOffset(0x10)]
//    public float B; //0x000C

//    [FieldOffset(0x14)]
//    public float A; //0x0010

//    [FieldOffset(0x1C)]
//    public float m_flGlowAlphaFunctionOfMaxVelocity; //0x0010
//    [FieldOffset(0x20)]
//    public float m_flGlowAlphaMax; //0x0010
//    //[FieldOffset(0x20)]
//    ///public float m_flGlowPulseOverdrive; //0x0010

//    //[FieldOffset(0x14)]
//    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
//    //public byte[] unkA; //0x0014 -> 0x20 JUNK

//    [FieldOffset(0x28)]
//    public bool bRenderWhenOccluded; //0x0024

//    [FieldOffset(0x29)]
//    public bool bRenderWhenUnoccluded; //0x0025

//    [FieldOffset(0x2A)]
//    public bool bFullBloom; //0x0026


//    [FieldOffset(0x30)]
//    public byte m_nRenderStyle; //0x002C
//}


[StructLayout(LayoutKind.Explicit)]
struct m_dwGlowObject_NOENT
{
    [FieldOffset(0x04)]
    public float R; //0x0004

    [FieldOffset(0x08)]
    public float G; //0x0008

    [FieldOffset(0x0C)]
    public float B; //0x000C

    [FieldOffset(0x10)]
    public float A; //0x0010

    [FieldOffset(0x18)]
    public float m_flGlowAlphaFunctionOfMaxVelocity; //0x0010
    [FieldOffset(0x1C)]
    public float m_flGlowAlphaMax; //0x0010
    [FieldOffset(0x20)]
    public float m_flGlowPulseOverdrive; //0x0010

    [FieldOffset(0x24)]
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool bRenderWhenOccluded; //0x0024

    [FieldOffset(0x25)]
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool bRenderWhenUnoccluded; //0x0025

    [FieldOffset(0x26)]
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool bFullBloom; //0x0026

    //[FieldOffset(0x27)]
    //public byte gayBytes;

    //[FieldOffset(0x28)]
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //public byte[] unkB; //0x0027 -> 2B 5 

    [FieldOffset(0x2C)]
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool bInnerGlow; //0x002C

    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    //[FieldOffset(0x30)]
    //public byte[] unkC;
}
[StructLayout(LayoutKind.Explicit)]
public struct GlowObjectDefinition_t
{
    [FieldOffset(0x00)]
    public IntPtr dwEntity; // 0x0

    [FieldOffset(0x04)]
    public float fR;    // 0x4

    [FieldOffset(0x08)]
    public float fG;  // 0x8

    [FieldOffset(0x08)]
    public float fB; // 0xC

    [FieldOffset(0x10)]
    public float fAlpha; // 0x10

    [FieldOffset(0x14)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] cJunk; // 0x14

    [FieldOffset(0x24)]
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool bRenderWhenOccluded; // 0x24

    [FieldOffset(0x25)]
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool bRenderWhenUnoccluded; // 0x25

    [FieldOffset(0x26)]
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool bFullBloomRender; // 0x26

    [FieldOffset(0x27)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] junk2; // 0x27

    [FieldOffset(0x2C)]
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool m_bInnerGlow; // 0x2C
}


[StructLayout(LayoutKind.Explicit)]
public struct glowObjectDefinition_t
{
    [FieldOffset(0x00)]
    public IntPtr pEntity;
    [FieldOffset(0x04)]
    public float r;
    [FieldOffset(0x08)]
    public float g;
    [FieldOffset(0x08)]
    public float b;
    [FieldOffset(0x10)]
    public float a;

    //16 bytes junk

    [FieldOffset(0x14)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] junk0;

    [FieldOffset(0x24)]
    public byte m_bRenderWhenOccluded;
    [FieldOffset(0x25)]
    public byte m_bRenderWhenUnoccluded;
    [FieldOffset(0x26)]
    public byte m_bFullBloom;

    //12 bytes junk
    [FieldOffset(0x27)]
    public byte junk2;

    [FieldOffset(0x28)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] junk1;
}

//[StructLayout(LayoutKind.Explicit)]
//public struct CENVTONEMAPPER
//{
//    [FieldOffset(0x09D8)]
//    public bool m_bUseCustomAutoExposureMin;
//    [FieldOffset(0x09D9)]
//    public bool m_bUseCustomAutoExposureMax;
//    [FieldOffset(0x09DA)]
//    public bool m_bUseCustomBloomScale;
//    [FieldOffset(0x09DC)]
//    public float m_flCustomAutoExposureMin;
//    [FieldOffset(0x09E0)]
//    public float m_flCustomAutoExposureMax;
//    [FieldOffset(0x09E4)]
//    public float m_flCustomBloomScale;
//    [FieldOffset(0x09E8)]
//    public float m_flCustomBloomScaleMinimum;
//    [FieldOffset(0x09EC)]
//    public float m_flBloomExponent;
//    [FieldOffset(0x09F0)]
//    public float m_flBloomSaturation;
//    [FieldOffset(0x09F4)]
//    public float m_flTonemapPercentTarget;
//    [FieldOffset(0x09F8)]
//    public float m_flTonemapPercentBrightPixels;
//    [FieldOffset(0x09FC)]
//    public float m_flTonemapMinAvgLum;
//    [FieldOffset(0x0A00)]
//    public float m_flTonemapRate;
//}
[StructLayout(LayoutKind.Sequential)]
public struct view_matrix_t
{
    #region CONSTANTS
    public const int NUM_ROWS = 4;
    public const int NUM_COLS = 4;
    public const int NUM_ELEMENTS = NUM_ROWS * NUM_COLS;
    #endregion

    #region VARIABLES
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_ELEMENTS)]
    private float[] Elements;
    #endregion

    #region OPERATORS
    public float this[int i]
    {
        get
        {
            if (i < 0 || i >= NUM_ELEMENTS)
                throw new IndexOutOfRangeException("Invalid matrix-index");

            return Elements[i];
        }
    }
    public float this[int row, int column]
    {
        get
        {
            if (row < 0 || row >= NUM_ROWS || column < 0 || column >= NUM_COLS)
                throw new IndexOutOfRangeException("Invalid matrix-indices");

            return Elements[row * NUM_COLS + column];
        }
    }
    #endregion
}

//[StructLayout(LayoutKind.Explicit)]
//struct CPostProcessController
//{
//    [FieldOffset(0x9D8)]
//    public float fadetime; //0x0000
//    [FieldOffset(0x9DC)]
//    public float localcontrast; //0x0004
//    [FieldOffset(0x9D0)]
//    public float localcontrastedstrength; //0x0008
//    [FieldOffset(0x9E4)]
//    public float vignettestart; //0x000C
//    [FieldOffset(0x9E8)]
//    public float vignetteend; //0x0010
//    [FieldOffset(0x9EC)]
//    public float vignetteblurstrength; //0x0014
//    [FieldOffset(0x9F0)]
//    public float fadetoblackstrength; //0x0018
//    [FieldOffset(0x9F4)]
//    public float depthblurfocalstrength; //0x001C
//    [FieldOffset(0x9F8)]
//    public float depthblurfocaldistance; //0x0020
//    [FieldOffset(0x9FC)]
//    public float depthblurstrength; //0x0024
//    [FieldOffset(0xA00)]
//    public float screenblurstrength; //0x0028
//    [FieldOffset(0xA04)]
//    public float filmgrainstrength; //0x002C
//}

[StructLayout(LayoutKind.Explicit)]
public struct localsound
{
    [FieldOffset(0x3158)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    public SharpDX.Vector3[] Indexes;
    [FieldOffset(0x31B8)]
    public int soundScapeIndex;
    [FieldOffset(0x31BC)]
    public int localBits;
    [FieldOffset(0x31C0)]
    public int entIndex; //?
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct player_info_s
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public char[] __pad0;
    public int m_nXuidLow;
    public int m_nXuidHigh;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public char[] m_szPlayerName;
    public int m_nUserID;
    //33 or 20
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public char[] m_szSteamID;
    public uint m_nSteam3ID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public char[] m_szFriendsName;
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool m_bIsFakePlayer;
    [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
    public bool m_bIsHLTV;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public int[] m_dwCustomFiles;
    public char m_FilesDownloaded;
}

//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct ConVar_struct
//{
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
//    public char[] pad_0x0000; //0x0000
//    public IntPtr pNext; //0x0004 
//    public int bRegistered; //0x0008 
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
//    public char[] m_pszName;
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
//    public char[] m_pszHelpString; //0x0010 
//    public int nFlags; //0x0014 
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
//    public char[] pad_0x0018; //0x0018
//    public IntPtr pParent; //0x001C 
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
//    public char[] pszDefaultValue; //0x0020 
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
//    public char[] strString; //0x0024 
//    public int StringLength; //0x0028 
//    public float fValue; //0x002C 
//    public int nValue; //0x0030 
//    public int bHasMin; //0x0034 
//    public float fMinVal; //0x0038 
//    public int bHasMax; //0x003C 
//    public float fMaxVal; //0x0040 
//    public IntPtr fnChangeCallback; //0x0044
//}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct mstudiobbox_t
{
    public int bone;
    public int group;
    public struct_Vector bbmin;
    public struct_Vector bbmax;
    public int szhitboxnameindex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public char[] m_iPad01;
    public float m_flRadius;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public int[] m_iPad02;
};

[StructLayout(LayoutKind.Explicit)]
public struct struct_Vector
{
    [FieldOffset(0x0)]
    public float x;
    [FieldOffset(0x4)]
    public float y;
    [FieldOffset(0x8)]
    public float z;

    public override string ToString()
    {
        return string.Format("{0} {1} {2}", x, y, z);
    }
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct float_4
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] second;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct matrix3x4
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float_4[] first;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VectorBone
{
    public float X;
    public float Y;
    public float Z;
}

[StructLayout(LayoutKind.Explicit)]
public struct BoneVec
{
    [FieldOffset(0x0C)]
    public float X;
    [FieldOffset(0x1C)]
    public float Y;
    [FieldOffset(0x2C)]
    public float Z;
}
