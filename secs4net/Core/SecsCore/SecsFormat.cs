namespace Secs4Net
{

    /// <summary>
    /// The enumeration of SECS item value format 
    /// </summary>
    public enum SecsFormat : byte
    {
        List    = 0b000000_00,
        Binary  = 0b001000_00,
        Boolean = 0b001001_00,
        ASCII   = 0b010000_00,
        JIS8    = 0b010001_00,
        I8      = 0b011000_00,
        I1      = 0b011001_00,
        I2      = 0b011010_00,
        I4      = 0b011100_00,
        F8      = 0b100000_00,
        F4      = 0b100100_00,
        U8      = 0b101000_00,
        U1      = 0b101001_00,
        U2      = 0b101010_00,
        U4      = 0b101100_00
    }
}