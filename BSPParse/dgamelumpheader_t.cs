using System.Runtime.InteropServices;

namespace RRFull.BSPParse
{
    [StructLayout(LayoutKind.Sequential)]
    public struct dgamelumpheader_t
    {
        public int m_LumpCount;
    }
}
