using FluentAssertions;
using System;
using Xunit;
using static Secs4Net.Item;

namespace Secs4Net.UnitTests
{
    public class ItemUnitTest
    {
        [Fact]
        public void Item_Equals_Should_Be_True()
        {
            var left =
                L(
                    A("A"),
                    B(),
                    F4(),
                    I4()
                );

            var right =
                L(
                    A("A"),
                    B(),
                    F4(),
                    I4()
                );

            left.Should().BeEquivalentTo(right);
        }

        [Fact]
        public void Item_Equals_Should_Be_False()
        {
            var left =
                L(
                    A("A"),
                    B(),
                    L(
                        B(12, 11)),
                    F4(),
                    I4()
                );

            var right =
                L(
                    A("A"),
                    B(),
                    L(
                        B(12, 10)), // diff
                    F8(), // diff
                    I4()
                );

            left.Should().NotBeEquivalentTo(right);
        }

        [Fact]
        public void Item_GetValues_With_Match_Typed()
        {
            var source = new int[] { 1, 2, 3, 4 };
            var itemArray = I4(source).GetValues<int>().ToArray();

            itemArray.Should().BeEquivalentTo(source);
        }

        [Fact]
        public void Item_GetValues_With_Downsize_Typed()
        {
            var source = new int[] { 1, 2, 3, 4 };

            // down size from int to short
            var itemArray = I4(source).GetValues<short>().ToArray();
            itemArray.Length.Should().Be(Buffer.ByteLength(source) / sizeof(short));

            var expectedArray = new short[itemArray.Length];
            Buffer.BlockCopy(source, 0, expectedArray, 0, itemArray.Length * sizeof(short));

            itemArray.Should().BeEquivalentTo(expectedArray);
        }

        [Fact]
        public void Item_GetValues_With_Upsize_Typed()
        {
            var source = new int[] { 1, 2, 3, 4 };

            // up size from int to long
            var itemArray = I4(source).GetValues<long>().ToArray();
            itemArray.Length.Should().Be(Buffer.ByteLength(source) / sizeof(long));

            var expectedArray = new long[itemArray.Length];
            Buffer.BlockCopy(source, 0, expectedArray, 0, itemArray.Length * sizeof(long));

            itemArray.Should().BeEquivalentTo(expectedArray);
        }

        [Fact]
        public void Item_GetValues_With_Upsize_Typed_But_Unallignment()
        {
            var source = new int[] { 1, 2, 3, 4, 5 };

            // up size from int to long
            var itemArray = I4(source).GetValues<long>().ToArray();
            itemArray.Length.Should().Be(Buffer.ByteLength(source) / sizeof(long));

            var expectedArray = new long[itemArray.Length];
            Buffer.BlockCopy(source, 0, expectedArray, 0, itemArray.Length * sizeof(long));

            itemArray.Should().BeEquivalentTo(expectedArray);
        }

        [Fact]
        public void Item_Get_GetValue_Should_Throw_Error_If_Is_Empty()
        {
            var item = I4();

            Action action = () => item.FirstValue<int>();
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [Fact]
        public void Item_GetValue_With_Downsize_Typed()
        {
            var firstSrc = short.MaxValue + 12;
            var first = I4(firstSrc).FirstValue<short>();
            first.Should().Be(unchecked((short)firstSrc));
        }

        [Fact]
        public void Item_GetValue_With_Upsize_Typed()
        {
            var bytes = new byte[] { 128, 212, 231 };
            var first = U1(bytes).FirstValue<ushort>();
            first.Should().Be(BitConverter.ToUInt16(bytes));
        }

        [Fact]
        public void Item_GetValue_With_Upsize_Typed_Should_Throw_Error_If_Total_Bytes_Less_Than_SizeOf_T()
        {
            var bytes = new byte[] { 128 };
            Action action = () => U1(bytes).FirstValue<ushort>();
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [Fact]
        public void Item_GetValueOrDefault_Should_Not_Throw_Error_If_Is_Empty()
        {
            var item = I4();

            Action action = () => item.GetFirstValueOrDefault<int>();
            action.Should().NotThrow();

            Action action2 = () => item.GetFirstValueOrDefault<bool>();
            action2.Should().NotThrow();

            item.GetFirstValueOrDefault(4).Should().Be(4);
        }

        [Fact]
        public void Item_GetValueOrDefault_With_Upsize_Typed_Should_Return_Default_Value_If_Total_Bytes_Less_Than_SizeOf_T()
        {
            var bytes = new byte[] { 128 };
            ushort defaultValue = 11;
            var first = U1(bytes).GetFirstValueOrDefault(defaultValue);
            first.Should().Be(defaultValue);
        }

        [Fact]
        public void Item_Encode_Decode_Should_Be_Equivalent()
        {
            var item =
                L(
                    L(),
                    U1(122, 34),
                    U2(34531, 23123),
                    U4(2123513, 52451141),
                    F4(23123.21323f, 2324.221f),
                    A("A string"),
                    J("sdsad"),
                    F8(231.00002321d, 0.2913212312d),
                    L(
                        U1(122, 34),
                        U2(34531, 23123),
                        U4(2123513, 52451141),
                        F4(23123.21323f, 2324.221f),
                        Boolean(true, false, false, true),
                        B(0x1C, 0x01, 0xFF),
                        L(
                            A("A string"),
                            J("sdsad"),
                            Boolean(true, false, false, true),
                            B(0x1C, 0x01, 0xFF)),
                        F8(231.00002321d, 0.2913212312d)));

            var item2 = Item.Decode(item.GetEncodedBytes());
            item.Should().BeEquivalentTo(item2); 
        }

        [Fact]
        public void Item_List_Can_Be_Changed()
        {
            var item =
                L(
                    U1(122, 34),
                    U2(34531, 23123),
                    U4(2123513, 52451141),
                    F4(23123.21323f, 2324.221f),
                    Boolean(true, false, false, true),
                    B(0x1C, 0x01, 0xFF),
                    F8(231.00002321d, 0.2913212312d));

            item[2] = Boolean(true);
            item[2].Should().BeEquivalentTo(item[2]);
        }

        [Fact]
        public void Item_FirstValue_Can_Be_Changed()
        {
            ushort original = 2134;
            var item = U2(original);
            item.FirstValue<byte>() = 12; // change first byte

            var changed = item.FirstValue<ushort>();
            Assert.NotEqual(original, changed);

            var originalBytes = BitConverter.GetBytes(original);
            originalBytes[0] = 12; // change first byte

            changed.Should().Be(BitConverter.ToUInt16(originalBytes));
        }

        [Fact]
        public void Item_Values_Can_Be_Changed()
        {
            var original = ushort.MaxValue;
            var item = U2(original);
            var arr = item.GetValues<byte>();
            arr[0] = 123;
            arr[1] = 3;

            var changed = item.FirstValue<ushort>();
            changed.Should().NotBe(original);
            changed.Should().Be(BitConverter.ToUInt16(arr.AsSpan()));
        }
    }
}
