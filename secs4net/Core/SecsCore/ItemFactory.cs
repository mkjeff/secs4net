using System.Collections.Generic;

namespace Secs4Net
{
    public static class Item
    {
        public static SecsItem L() => ListFormat.Empty;
        public static SecsItem L(SecsItem item0) => ListFormat.Create(item0);
        public static SecsItem L(SecsItem item0, SecsItem item1) => ListFormat.Create(item0, item1);
        public static SecsItem L(SecsItem item0, SecsItem item1, SecsItem item2) => ListFormat.Create(item0, item1, item2);
        public static SecsItem L(SecsItem item0, SecsItem item1, SecsItem item2, SecsItem item3) => ListFormat.Create(item0, item1, item2, item3);
        public static SecsItem L(SecsItem item0, SecsItem item1, SecsItem item2, SecsItem item3, SecsItem item4) => ListFormat.Create(item0, item1, item2, item3, item4);
        public static SecsItem L(SecsItem item0, SecsItem item1, SecsItem item2, SecsItem item3, SecsItem item4, SecsItem item5) => ListFormat.Create(item0, item1, item2, item3, item4, item5);
        public static SecsItem L(SecsItem item0, SecsItem item1, SecsItem item2, SecsItem item3, SecsItem item4, SecsItem item5, SecsItem item6) => ListFormat.Create(item0, item1, item2, item3, item4, item5, item6);
        public static SecsItem L(SecsItem item0, SecsItem item1, SecsItem item2, SecsItem item3, SecsItem item4, SecsItem item5, SecsItem item6, SecsItem item7) => ListFormat.Create(item0, item1, item2, item3, item4, item5, item6, item7);
        public static SecsItem L(SecsItem item0, SecsItem item1, SecsItem item2, SecsItem item3, SecsItem item4, SecsItem item5, SecsItem item6, SecsItem item7, SecsItem item8) => ListFormat.Create(item0, item1, item2, item3, item4, item5, item6, item7, item8);
        public static SecsItem L(SecsItem item0, SecsItem item1, SecsItem item2, SecsItem item3, SecsItem item4, SecsItem item5, SecsItem item6, SecsItem item7, SecsItem item8, SecsItem item9) => ListFormat.Create(item0, item1, item2, item3, item4, item5, item6, item7, item8, item9);
        public static SecsItem L(IEnumerable<SecsItem> items) => ListFormat.Create(items);
        public static SecsItem L(params SecsItem[] secsItems) => ListFormat.Create(secsItems);

        public static SecsItem B() => BinaryFormat.Empty;
        public static SecsItem B(byte value0) => BinaryFormat.Create(value0);
        public static SecsItem B(byte value0, byte value1) => BinaryFormat.Create(value0, value1);
        public static SecsItem B(byte value0, byte value1, byte value2) => BinaryFormat.Create(value0, value1, value2);
        public static SecsItem B(byte value0, byte value1, byte value2, byte value3) => BinaryFormat.Create(value0, value1, value2, value3);
        public static SecsItem B(byte value0, byte value1, byte value2, byte value3, byte value4) => BinaryFormat.Create(value0, value1, value2, value3, value4);
        public static SecsItem B(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5) => BinaryFormat.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem B(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6) => BinaryFormat.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem B(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7) => BinaryFormat.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem B(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7, byte value8) => BinaryFormat.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem B(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7, byte value8, byte value9) => BinaryFormat.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem B(IEnumerable<byte> value) => BinaryFormat.Create(value);
        public static SecsItem B(params byte[] value) => BinaryFormat.Create(value);

        public static SecsItem U1() => U1Format.Empty;
        public static SecsItem U1(byte value0) => U1Format.Create(value0);
        public static SecsItem U1(byte value0, byte value1) => U1Format.Create(value0, value1);
        public static SecsItem U1(byte value0, byte value1, byte value2) => U1Format.Create(value0, value1, value2);
        public static SecsItem U1(byte value0, byte value1, byte value2, byte value3) => U1Format.Create(value0, value1, value2, value3);
        public static SecsItem U1(byte value0, byte value1, byte value2, byte value3, byte value4) => U1Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem U1(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5) => U1Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem U1(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6) => U1Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem U1(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7) => U1Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem U1(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7, byte value8) => U1Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem U1(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7, byte value8, byte value9) => U1Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem U1(IEnumerable<byte> value) => U1Format.Create(value);
        public static SecsItem U1(params byte[] value) => U1Format.Create(value);

        public static SecsItem U2() => U2Format.Empty;
        public static SecsItem U2(ushort value0) => U2Format.Create(value0);
        public static SecsItem U2(ushort value0, ushort value1) => U2Format.Create(value0, value1);
        public static SecsItem U2(ushort value0, ushort value1, ushort value2) => U2Format.Create(value0, value1, value2);
        public static SecsItem U2(ushort value0, ushort value1, ushort value2, ushort value3) => U2Format.Create(value0, value1, value2, value3);
        public static SecsItem U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4) => U2Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4, ushort value5) => U2Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4, ushort value5, ushort value6) => U2Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4, ushort value5, ushort value6, ushort value7) => U2Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4, ushort value5, ushort value6, ushort value7, ushort value8) => U2Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4, ushort value5, ushort value6, ushort value7, ushort value8, ushort value9) => U2Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem U2(IEnumerable<ushort> value) => U2Format.Create(value);
        public static SecsItem U2(params ushort[] value) => U2Format.Create(value);
        
        public static SecsItem U4() => U4Format.Empty;
        public static SecsItem U4(uint value0) => U4Format.Create(value0);
        public static SecsItem U4(uint value0, uint value1) => U4Format.Create(value0, value1);
        public static SecsItem U4(uint value0, uint value1, uint value2) => U4Format.Create(value0, value1, value2);
        public static SecsItem U4(uint value0, uint value1, uint value2, uint value3) => U4Format.Create(value0, value1, value2, value3);
        public static SecsItem U4(uint value0, uint value1, uint value2, uint value3, uint value4) => U4Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem U4(uint value0, uint value1, uint value2, uint value3, uint value4, uint value5) => U4Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem U4(uint value0, uint value1, uint value2, uint value3, uint value4, uint value5, uint value6) => U4Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem U4(uint value0, uint value1, uint value2, uint value3, uint value4, uint value5, uint value6, uint value7) => U4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem U4(uint value0, uint value1, uint value2, uint value3, uint value4, uint value5, uint value6, uint value7, uint value8) => U4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem U4(uint value0, uint value1, uint value2, uint value3, uint value4, uint value5, uint value6, uint value7, uint value8, uint value9) => U4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem U4(IEnumerable<uint> value) => U4Format.Create(value);
        public static SecsItem U4(params uint[] value) => U4Format.Create(value);
     
        public static SecsItem U8() => U8Format.Empty;
        public static SecsItem U8(ulong value0) => U8Format.Create(value0);
        public static SecsItem U8(ulong value0, ulong value1) => U8Format.Create(value0, value1);
        public static SecsItem U8(ulong value0, ulong value1, ulong value2) => U8Format.Create(value0, value1, value2);
        public static SecsItem U8(ulong value0, ulong value1, ulong value2, ulong value3) => U8Format.Create(value0, value1, value2, value3);
        public static SecsItem U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4) => U8Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4, ulong value5) => U8Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4, ulong value5, ulong value6) => U8Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4, ulong value5, ulong value6, ulong value7) => U8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4, ulong value5, ulong value6, ulong value7, ulong value8) => U8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4, ulong value5, ulong value6, ulong value7, ulong value8, ulong value9) => U8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem U8(IEnumerable<ulong> value) => U8Format.Create(value);
        public static SecsItem U8(params ulong[] value) => U8Format.Create(value);

        public static SecsItem I1() => I1Format.Empty;
        public static SecsItem I1(sbyte value0) => I1Format.Create(value0);
        public static SecsItem I1(sbyte value0, sbyte value1) => I1Format.Create(value0, value1);
        public static SecsItem I1(sbyte value0, sbyte value1, sbyte value2) => I1Format.Create(value0, value1, value2);
        public static SecsItem I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3) => I1Format.Create(value0, value1, value2, value3);
        public static SecsItem I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4) => I1Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5) => I1Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5, sbyte value6) => I1Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5, sbyte value6, sbyte value7) => I1Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5, sbyte value6, sbyte value7, sbyte value8) => I1Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5, sbyte value6, sbyte value7, sbyte value8, sbyte value9) => I1Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem I1(IEnumerable<sbyte> value) => I1Format.Create(value);
        public static SecsItem I1(params sbyte[] value) => I1Format.Create(value);
        
        public static SecsItem I2() => I2Format.Empty;
        public static SecsItem I2(short value0) => I2Format.Create(value0);
        public static SecsItem I2(short value0, short value1) => I2Format.Create(value0, value1);
        public static SecsItem I2(short value0, short value1, short value2) => I2Format.Create(value0, value1, value2);
        public static SecsItem I2(short value0, short value1, short value2, short value3) => I2Format.Create(value0, value1, value2, value3);
        public static SecsItem I2(short value0, short value1, short value2, short value3, short value4) => I2Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem I2(short value0, short value1, short value2, short value3, short value4, short value5) => I2Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem I2(short value0, short value1, short value2, short value3, short value4, short value5, short value6) => I2Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem I2(short value0, short value1, short value2, short value3, short value4, short value5, short value6, short value7) => I2Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem I2(short value0, short value1, short value2, short value3, short value4, short value5, short value6, short value7, short value8) => I2Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem I2(short value0, short value1, short value2, short value3, short value4, short value5, short value6, short value7, short value8, short value9) => I2Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem I2(IEnumerable<short> value) => I2Format.Create(value);
        public static SecsItem I2(params short[] value) => I2Format.Create(value);
    
        public static SecsItem I4() => I4Format.Empty;
        public static SecsItem I4(int value0) => I4Format.Create(value0);
        public static SecsItem I4(int value0, int value1) => I4Format.Create(value0, value1);
        public static SecsItem I4(int value0, int value1, int value2) => I4Format.Create(value0, value1, value2);
        public static SecsItem I4(int value0, int value1, int value2, int value3) => I4Format.Create(value0, value1, value2, value3);
        public static SecsItem I4(int value0, int value1, int value2, int value3, int value4) => I4Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem I4(int value0, int value1, int value2, int value3, int value4, int value5) => I4Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem I4(int value0, int value1, int value2, int value3, int value4, int value5, int value6) => I4Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem I4(int value0, int value1, int value2, int value3, int value4, int value5, int value6, int value7) => I4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem I4(int value0, int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8) => I4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem I4(int value0, int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8, int value9) => I4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem I4(IEnumerable<int> value) => I4Format.Create(value);
        public static SecsItem I4(params int[] value) => I4Format.Create(value);
     
        public static SecsItem I8() => I8Format.Empty;
        public static SecsItem I8(long value0) => I8Format.Create(value0);
        public static SecsItem I8(long value0, long value1) => I8Format.Create(value0, value1);
        public static SecsItem I8(long value0, long value1, long value2) => I8Format.Create(value0, value1, value2);
        public static SecsItem I8(long value0, long value1, long value2, long value3) => I8Format.Create(value0, value1, value2, value3);
        public static SecsItem I8(long value0, long value1, long value2, long value3, long value4) => I8Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem I8(long value0, long value1, long value2, long value3, long value4, long value5) => I8Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem I8(long value0, long value1, long value2, long value3, long value4, long value5, long value6) => I8Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem I8(long value0, long value1, long value2, long value3, long value4, long value5, long value6, long value7) => I8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem I8(long value0, long value1, long value2, long value3, long value4, long value5, long value6, long value7, long value8) => I8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem I8(long value0, long value1, long value2, long value3, long value4, long value5, long value6, long value7, long value8, long value9) => I8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem I8(IEnumerable<long> value) => I8Format.Create(value);
        public static SecsItem I8(params long[] value) => I8Format.Create(value);
    
        public static SecsItem F4() => F4Format.Empty;
        public static SecsItem F4(float value0) => F4Format.Create(value0);
        public static SecsItem F4(float value0, float value1) => F4Format.Create(value0, value1);
        public static SecsItem F4(float value0, float value1, float value2) => F4Format.Create(value0, value1, value2);
        public static SecsItem F4(float value0, float value1, float value2, float value3) => F4Format.Create(value0, value1, value2, value3);
        public static SecsItem F4(float value0, float value1, float value2, float value3, float value4) => F4Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem F4(float value0, float value1, float value2, float value3, float value4, float value5) => F4Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem F4(float value0, float value1, float value2, float value3, float value4, float value5, float value6) => F4Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem F4(float value0, float value1, float value2, float value3, float value4, float value5, float value6, float value7) => F4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem F4(float value0, float value1, float value2, float value3, float value4, float value5, float value6, float value7, float value8) => F4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem F4(float value0, float value1, float value2, float value3, float value4, float value5, float value6, float value7, float value8, float value9) => F4Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem F4(IEnumerable<float> value) => F4Format.Create(value);
        public static SecsItem F4(params float[] value) => F4Format.Create(value);
   
        public static SecsItem F8() => F8Format.Empty;
        public static SecsItem F8(double value0) => F8Format.Create(value0);
        public static SecsItem F8(double value0, double value1) => F8Format.Create(value0, value1);
        public static SecsItem F8(double value0, double value1, double value2) => F8Format.Create(value0, value1, value2);
        public static SecsItem F8(double value0, double value1, double value2, double value3) => F8Format.Create(value0, value1, value2, value3);
        public static SecsItem F8(double value0, double value1, double value2, double value3, double value4) => F8Format.Create(value0, value1, value2, value3, value4);
        public static SecsItem F8(double value0, double value1, double value2, double value3, double value4, double value5) => F8Format.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem F8(double value0, double value1, double value2, double value3, double value4, double value5, double value6) => F8Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem F8(double value0, double value1, double value2, double value3, double value4, double value5, double value6, double value7) => F8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem F8(double value0, double value1, double value2, double value3, double value4, double value5, double value6, double value7, double value8) => F8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem F8(double value0, double value1, double value2, double value3, double value4, double value5, double value6, double value7, double value8, double value9) => F8Format.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem F8(IEnumerable<double> value) => F8Format.Create(value);
        public static SecsItem F8(params double[] value) => F8Format.Create(value);
     
        public static SecsItem Boolean() => BooleanFormat.Empty;
        public static SecsItem Boolean(bool value0) => BooleanFormat.Create(value0);
        public static SecsItem Boolean(bool value0, bool value1) => BooleanFormat.Create(value0, value1);
        public static SecsItem Boolean(bool value0, bool value1, bool value2) => BooleanFormat.Create(value0, value1, value2);
        public static SecsItem Boolean(bool value0, bool value1, bool value2, bool value3) => BooleanFormat.Create(value0, value1, value2, value3);
        public static SecsItem Boolean(bool value0, bool value1, bool value2, bool value3, bool value4) => BooleanFormat.Create(value0, value1, value2, value3, value4);
        public static SecsItem Boolean(bool value0, bool value1, bool value2, bool value3, bool value4, bool value5) => BooleanFormat.Create(value0, value1, value2, value3, value4, value5);
        public static SecsItem Boolean(bool value0, bool value1, bool value2, bool value3, bool value4, bool value5, bool value6) => BooleanFormat.Create(value0, value1, value2, value3, value4, value5, value6);
        public static SecsItem Boolean(bool value0, bool value1, bool value2, bool value3, bool value4, bool value5, bool value6, bool value7) => BooleanFormat.Create(value0, value1, value2, value3, value4, value5, value6, value7);
        public static SecsItem Boolean(bool value0, bool value1, bool value2, bool value3, bool value4, bool value5, bool value6, bool value7, bool value8) => BooleanFormat.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8);
        public static SecsItem Boolean(bool value0, bool value1, bool value2, bool value3, bool value4, bool value5, bool value6, bool value7, bool value8, bool value9) => BooleanFormat.Create(value0, value1, value2, value3, value4, value5, value6, value7, value8, value9);
        public static SecsItem Boolean(IEnumerable<bool> value) => BooleanFormat.Create(value);
        public static SecsItem Boolean(params bool[] value) => BooleanFormat.Create(value);
     
        public static SecsItem A() => ASCIIFormat.Empty;
        public static SecsItem A(string value) => ASCIIFormat.Create(value);

        public static SecsItem J() => JIS8Format.Empty;
        public static SecsItem J(string value) => JIS8Format.Create(value);
    }
}
