namespace Secs4Net
{

    /// <summary>
    /// The enumeration of SECS item value format 
    /// </summary>
    public enum SecsFormat : byte
    {

        /// <summary>
        /// List
        /// </summary>
        List = 0,		// 000000 00

        /// <summary>
        /// Binary(raw)
        /// </summary>
        Binary = 0x20,	// 001000 00

        /// <summary>
        /// Boolean
        /// </summary>
        Boolean = 0x24,	// 001001 00

        /// <summary>
        /// ASCII string
        /// </summary>
        ASCII = 0x40,   // 010000 00

        /// <summary>
        /// JIS8 string
        /// </summary>
        JIS8 = 0x44,	// 010001 00

        /// <summary>
        /// Signed 8 byte integer
        /// </summary>
        I8 = 0x60,		// 011000 00

        /// <summary>
        /// Signed 1 byte integer
        /// </summary>
        I1 = 0x64,		// 011001 00

        /// <summary>
        /// Signed 2 byte integer
        /// </summary>
        I2 = 0x68,		// 011010 00

        /// <summary>
        /// Signed 4 byte integer
        /// </summary>
        I4 = 0x70,		// 011100 00

        /// <summary>
        /// Double-precision floating-point
        /// </summary>
        F8 = 0x80,		// 100000 00

        /// <summary>
        /// floating-point
        /// </summary>
        F4 = 0x90,		// 100100 00

        /// <summary>
        /// unsigned 8 byte integer
        /// </summary>
        U8 = 0xA0,      // 101000 00

        /// <summary>
        /// unsigned 1 byte integer
        /// </summary>
        U1 = 0xA4,      // 101001 00

        /// <summary>
        /// unsigned 2 byte integer
        /// </summary>
        U2 = 0xA8,      // 101010 00

        /// <summary>
        /// unsigned 4 byte integer
        /// </summary>
        U4 = 0xB0,		// 101100 00
    }
}