namespace Secs4Net {

    /// <summary>
    /// The enumeration of SECS item value format 
    /// </summary>
    public enum SecsFormat : byte
    {
        List    = 0b_0000_00_00,
        Binary  = 0b_0010_00_00,
        Boolean = 0b_0010_01_00,
        ASCII   = 0b_0100_00_00,
        JIS8    = 0b_0100_01_00,
        I8      = 0b_0110_00_00,
        I1      = 0b_0110_01_00,
        I2      = 0b_0110_10_00,
        I4      = 0b_0111_00_00,
        F8      = 0b_1000_00_00,
        F4      = 0b_1001_00_00,
        U8      = 0b_1010_00_00,
        U1      = 0b_1010_01_00,
        U2      = 0b_1010_10_00,
        U4      = 0b_1011_00_00
    }
}