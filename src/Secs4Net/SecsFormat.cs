namespace Secs4Net;

public enum SecsFormat
{
    List    = 0b_0000_00,
    Binary  = 0b_0010_00,
    Boolean = 0b_0010_01,
    ASCII   = 0b_0100_00,
    JIS8    = 0b_0100_01,
    I8      = 0b_0110_00,
    I1      = 0b_0110_01,
    I2      = 0b_0110_10,
    I4      = 0b_0111_00,
    F8      = 0b_1000_00,
    F4      = 0b_1001_00,
    U8      = 0b_1010_00,
    U1      = 0b_1010_01,
    U2      = 0b_1010_10,
    U4      = 0b_1011_00,
}
