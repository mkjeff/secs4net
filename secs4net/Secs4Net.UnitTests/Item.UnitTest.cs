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
                    A(""),
                    B(),
                    F4(),
                    I4()
                );

            var right =
                L(
                    A(""),
                    B(),
                    F4(),
                    I4()
                );

            left.Equals(right).Should().BeTrue();
        }

        [Fact]
        public void Item_Equals_Should_Be_False()
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
                    A(""),
                    B(),
                    F4(),
                    I4()
                );

            left.Equals(right).Should().BeTrue();
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

            Action action = () => item.GetValue<int>();
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [Fact]
        public void Item_GetValue_With_Downsize_Typed()
        {
            var firstSrc = short.MaxValue + 12;
            var first = I4(firstSrc).GetValue<short>();
            first.Should().Be(unchecked((short)firstSrc));
        }

        [Fact]
        public void Item_GetValue_With_Upsize_Typed()
        {
            var bytes = new byte[] { 128, 212, 231 };
            var first = U1(bytes).GetValue<ushort>();
            first.Should().Be(BitConverter.ToUInt16(bytes));
        }

        [Fact]
        public void Item_GetValue_With_Upsize_Typed_Should_Throw_Error_If_Total_Bytes_Less_Than_SizeOf_T()
        {
            var bytes = new byte[] { 128 };
            Action action = () => U1(bytes).GetValue<ushort>();
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [Fact]
        public void Item_GetValueOrDefault_Should_Not_Throw_Error_If_Is_Empty()
        {
            var item = I4();

            Action action = () => item.GetValueOrDefault<int>();
            action.Should().NotThrow();

            Action action2 = () => item.GetValueOrDefault<bool>();
            action2.Should().NotThrow();

            item.GetValueOrDefault(4).Should().Be(4);
        }

        [Fact]
        public void Item_GetValueOrDefault_With_Upsize_Typed_Should_Return_Default_Value_If_Total_Bytes_Less_Than_SizeOf_T()
        {
            var bytes = new byte[] { 128 };
            ushort defaultValue = 11;
            var first = U1(bytes).GetValueOrDefault(defaultValue);
            first.Should().Be(defaultValue);
        }

        [Fact]
        public void Message_Encode_Decode_Should_Be_Equivalent()
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
    }
}
