using CommunityToolkit.HighPerformance.Buffers;
using FluentAssertions;
using System;
using System.Buffers;
using System.Linq;
using Xunit;

namespace Secs4Net.UnitTests;

public class ItemUnitTest
{
    [Fact]
    public void Item_Self_Equals_Should_Be_True()
    {
        var left =
            L(
                A("A"),
                B(),
                F4(),
                I4()
            );

        var right = left;
        Assert.True(left == right);
    }

    [Fact]
    public void Item_Self_Not_Equals_Should_Be_False()
    {
        var left =
            L(
                A("A"),
                B(),
                F4(),
                I4()
            );

        var right = left;
        Assert.False(left != right);
    }

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
    public void ItemWithOwner_Can_Compare_With_ItemWithOwner()
    {
        using var left = I2(CreateMemoryOwner<short>(1, 2, 3));

        using var right = I2(CreateMemoryOwner<short>(1, 2, 3));

        left.Should().BeEquivalentTo(right);

        static MemoryOwner<T> CreateMemoryOwner<T>(T a1, T a2, T a3)
        {
            var leftOwner = MemoryOwner<T>.Allocate(3);
            leftOwner.Span[0] = a1;
            leftOwner.Span[1] = a2;
            leftOwner.Span[2] = a3;
            return leftOwner;
        }
    }

    [Fact]
    public void ItemWithOwner_Can_Compare_With_Item()
    {
        using var left = I2(CreateMemoryOwner<short>(1, 2, 3));

        using var right = I2(1, 2, 3);

        left.Should().BeEquivalentTo(right);

        static MemoryOwner<T> CreateMemoryOwner<T>(T a1, T a2, T a3)
        {
            var leftOwner = MemoryOwner<T>.Allocate(3);
            leftOwner.Span[0] = a1;
            leftOwner.Span[1] = a2;
            leftOwner.Span[2] = a3;
            return leftOwner;
        }
    }

    [Fact]
    public void Item_Can_Get_Internal_Values_With_Proper_Method()
    {
        var str = "I am a string";
        using (var stringItem = A(str))
        {
            stringItem.GetString().Should().Be(str);
            stringItem.Count.Should().Be(str.Length);
        }

        var arr = new short[] { 2314, 4214, 4221 };
        using (var arrayItem = I2(arr))
        {
            arrayItem.GetMemory<short>().ToArray().Should().BeEquivalentTo(arr);
            arrayItem.FirstValue<short>().Should().Be(arr[0]);
            arrayItem.FirstValueOrDefault<short>(21).Should().NotBe(21).And.Be(arr[0]);
        }

        using (var listItem = L(
            A(str),
            I2(arr),
            U2(),
            Boolean()))
        {
            listItem[0].Should().BeEquivalentTo(A(str));
            listItem[1].Should().BeEquivalentTo(I2(arr));

            Action getEnumerator = () =>
            {
                foreach (var item in listItem.Items)
                {

                }
            };
            getEnumerator.Should().NotThrow();

            L(listItem.Items[1..^1]).Should().BeEquivalentTo(L(I2(arr), U2()));

            Action sliceOutOfRange = () => _ = listItem.Items[2..5];
            sliceOutOfRange.Should().Throw<ArgumentOutOfRangeException>();
        }
    }

    [Fact]
    public void Item_Throw_Not_Supported_Exception_With_Format_Unmatch()
    {
        using (var stringItem = A("string"))
        {
            Action stringItemAccessIndexer = () => _ = stringItem[0];
            stringItemAccessIndexer.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage("Item", stringItem.Format));

            Action stringItemGetEnumerator = () => _ = stringItem.Items;
            stringItemGetEnumerator.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.Items), stringItem.Format));

            Action stringItemFirstValue = () => stringItem.FirstValue<byte>();
            stringItemFirstValue.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.FirstValue), stringItem.Format));

            Action stringItemFirstValueOrDefault = () => stringItem.FirstValueOrDefault<byte>();
            stringItemFirstValueOrDefault.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.FirstValueOrDefault), stringItem.Format));

            Action stringItemGetValues = () => stringItem.GetMemory<char>();
            stringItemGetValues.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.GetMemory), stringItem.Format));
        }

        using (var arrayItem = Boolean())
        {
            Action arrayItemGetString = () => arrayItem.GetString();
            arrayItemGetString.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.GetString), arrayItem.Format));

            Action arrayItemAccessIndexer = () => _ = arrayItem[^1];
            arrayItemAccessIndexer.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage("Item", arrayItem.Format));

            Action arrayItemGetEnumerator = () => _ = arrayItem.Items;
            arrayItemGetEnumerator.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.Items), arrayItem.Format));
        }

        using (var listItem = L())
        {
            Action listItemGetString = () => listItem.GetString();
            listItemGetString.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.GetString), listItem.Format));

            Action listItemFirstValue = () => listItem.FirstValue<byte>();
            listItemFirstValue.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.FirstValue), listItem.Format));

            Action listItemFirstValueOrDefault = () => listItem.FirstValueOrDefault<byte>();
            listItemFirstValueOrDefault.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.FirstValueOrDefault), listItem.Format));

            Action listItemGetValues = () => listItem.GetMemory<byte>();
            listItemGetValues.Should().Throw<NotSupportedException>()
                .WithMessage(CreateNotSupportedMessage(nameof(Item.GetMemory), listItem.Format));
        }
        static string CreateNotSupportedMessage(string memberName, SecsFormat format)
            => $"{memberName} is not supported, since the item's {nameof(Item.Format)} is {format}";
    }

    [Fact]
    public void Item_GetValues_With_Match_Typed()
    {
        var source = new int[] { 1, 2, 3, 4 };
        var itemArray = I4(source).GetMemory<int>().ToArray();

        itemArray.Should().BeEquivalentTo(source);
    }

    [Fact]
    public void Item_GetValues_With_Downsize_Typed()
    {
        var source = new int[] { 1, 2, 3, 4 };

        // down size from int to short
        var itemArray = I4(source).GetMemory<short>().ToArray();
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
        var itemArray = I4(source).GetMemory<long>().ToArray();
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
        var itemArray = I4(source).GetMemory<long>().ToArray();
        itemArray.Length.Should().Be(Buffer.ByteLength(source) / sizeof(long));

        var expectedArray = new long[itemArray.Length];
        Buffer.BlockCopy(source, 0, expectedArray, 0, itemArray.Length * sizeof(long));

        itemArray.Should().BeEquivalentTo(expectedArray);
    }

    [Fact]
    public void Item_GetValues_ValueArray_Can_Index_Great_Than_Length()
    {
        var item = I4(1, 2, 3, 4, 5);
        // up size from int to long

        Action indexGreatThanSpanLength = () =>
        {
            var itemArray = item.GetMemory<long>().Span;
            var value = itemArray[itemArray.Length + 1];
        };
        indexGreatThanSpanLength.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Item_FirstValue_Should_Throw_Error_If_Is_Empty()
    {
        var item = I4();

        Action action = () => item.FirstValue<int>();
        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Item_FirstValue_With_Downsize_Typed()
    {
        var firstSrc = short.MaxValue + 12;
        var first = I4(firstSrc).FirstValue<short>();
        first.Should().Be(unchecked((short)firstSrc));
    }

    [Fact]
    public void Item_FirstValue_With_Upsize_Typed()
    {
        var bytes = new byte[] { 128, 212, 231 };
        var first = U1(bytes).FirstValue<ushort>();
#if NET
        first.Should().Be(BitConverter.ToUInt16(bytes));
#else
        first.Should().Be(BitConverter.ToUInt16(bytes, 0));
#endif
    }

    [Fact]
    public void Item_FirstValue_With_Upsize_Typed_Should_Throw_Error_If_Total_Bytes_Less_Than_SizeOf_T()
    {
        var bytes = new byte[] { 128 };
        Action action = () => U1(bytes).FirstValue<ushort>();
        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Item_FirstValueOrDefault_Should_Not_Throw_Error_If_Is_Empty()
    {
        var item = I4();

        Action action = () => item.FirstValueOrDefault<int>();
        action.Should().NotThrow();

        Action action2 = () => item.FirstValueOrDefault<bool>();
        action2.Should().NotThrow();

        item.FirstValueOrDefault(4).Should().Be(4);
    }

    [Fact]
    public void Item_FirstValueOrDefault_With_Upsize_Typed_Should_Return_Default_Value_If_Total_Bytes_Less_Than_SizeOf_T()
    {
        var bytes = new byte[] { 128 };
        ushort defaultValue = 11;
        var first = U1(bytes).FirstValueOrDefault(defaultValue);
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
                U8(1258833613, 4513153356),
                F4(23123.21323f, 2324.221f),
                A("A string"),
                J("sdsad"),
                F8(231.00002321d, 0.2913212312d),
                L(
                    I1(122, 34),
                    I2(-1235, 23123),
                    I4(-2123513, 52451141),
                    I8(1266138966, -156114),
                    F4(23123.21323f, 2324.221f),
                    Boolean(true, false, false, true),
                    B(0x1C, 0x01, 0xFF),
                    L(
                        A("B string"),
                        J("asfdasfsfafsaf"),
                        Boolean(true, false, false, true),
                        B(0x1C, 0x01, 0xFF)),
                    F8(231.00002321d, 0.2913212312d)));

        var encodedBytes = new ReadOnlySequence<byte>(item.GetEncodedBytes());
        var item2 = DecodeFromFullBuffer(ref encodedBytes);
        encodedBytes.IsEmpty.Should().BeTrue();
        item.Should().BeEquivalentTo(item2);
    }

    [Fact]
    public void Item_With_MemoryOwner_Encode_Decode_Should_Be_Equivalent()
    {
        var item = U1(Enumerable.Repeat((byte)1, 1025));

        var encodedBytes = new ReadOnlySequence<byte>(item.GetEncodedBytes());
        using var item2 = DecodeFromFullBuffer(ref encodedBytes);
        //encodedBytes.IsEmpty.Should().BeTrue();
        item.Should().BeEquivalentTo(item2);
    }

    [Fact]
    public void List_Item_Slot_Can_Be_Changed()
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

        // be careful this operation
        // the original Item will not be managed by any List
        // so manually invoke Dispose method if you need.
        var oldItem = item[2];
        oldItem.Dispose();
        item[2] = Boolean(true);
        item[2].Should().BeEquivalentTo(Boolean(true));
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

#if NET
        changed.Should().Be(BitConverter.ToUInt16(originalBytes));
#else
        changed.Should().Be(BitConverter.ToUInt16(originalBytes, 0));
#endif
    }

    [Fact]
    public void Item_Values_Can_Be_Changed()
    {
        var original = ushort.MaxValue;
        var item = U2(original);
        var arr = item.GetMemory<byte>().Span;

        arr.Length.Should().Be(2);
        arr[0] = 123;
        arr[1] = 3;

        var changed = item.FirstValue<ushort>();
        changed.Should().NotBe(original);
#if NET
        changed.Should().Be(BitConverter.ToUInt16(arr));
#else
        changed.Should().Be(BitConverter.ToUInt16(arr.ToArray(), 0));
#endif
    }

    [Fact]
    public void Item_Encode_Decode_Should_Be_Work_With_Empty_ReadOnlySequenceSegment()
    {
        var item =
            L(
               A("AA"),
               A("q"));

        var encodedBytes = item.GetEncodedBytes();
        var first = new BufferSegment(Array.Empty<byte>());
        var last = first.Append(encodedBytes);
        var encodedSequence = new ReadOnlySequence<byte>(first, 0, last, encodedBytes.Length);

        var item2 = DecodeFromFullBuffer(ref encodedSequence);
        encodedSequence.IsEmpty.Should().BeTrue();
        item.Should().BeEquivalentTo(item2);
    }
}

sealed class BufferSegment : ReadOnlySequenceSegment<byte>
{
    public BufferSegment(Memory<byte> memory)
    {
        Memory = memory;
    }

    public BufferSegment Append(Memory<byte> memory)
    {
        var segment = new BufferSegment(memory)
        {
            RunningIndex = RunningIndex + Memory.Length
        };
        Next = segment;
        return segment;
    }
}
