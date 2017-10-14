namespace Secs4Net {

    /// <summary>
    /// The enumeration of SECS item value format 
    /// </summary>
    public enum SecsFormat : byte
    {
        List    = 0b0000_00_00,
        Binary  = 0b0010_00_00,
        Boolean = 0b0010_01_00,
        ASCII   = 0b0100_00_00,
        JIS8    = 0b0100_01_00,
        I8      = 0b0110_00_00,
        I1      = 0b0110_01_00,
        I2      = 0b0110_10_00,
        I4      = 0b0111_00_00,
        F8      = 0b1000_00_00,
        F4      = 0b1001_00_00,
        U8      = 0b1010_00_00,
        U1      = 0b1010_01_00,
        U2      = 0b1010_10_00,
        U4      = 0b1011_00_00
    }
}