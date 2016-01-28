using System;
namespace Secs4Net {
    public enum SecsFormat : byte {
        List = 0,		// 000000 00
        Binary = 0x20,	// 001000 00
        Boolean = 0x24,	// 001001 00
        ASCII = 0x40,	// 010000 00
        JIS8 = 0x44,	// 010001 00
        I8 = 0x60,		// 011000 00
        I1 = 0x64,		// 011001 00
        I2 = 0x68,		// 011010 00
        I4 = 0x70,		// 011100 00
        F8 = 0x80,		// 100000 00
        F4 = 0x90,		// 100100 00
        U8 = 0xA0,		// 101000 00
        U1 = 0xA4,		// 101001 00
        U2 = 0xA8,		// 101010 00
        U4 = 0xB0,		// 101100 00
    }
}