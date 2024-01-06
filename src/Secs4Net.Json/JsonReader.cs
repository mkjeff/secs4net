using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static Secs4Net.Item;

namespace Secs4Net.Json;

public static class JsonReader
{
    public static Item ToItem(this JsonElement jsonObject)
    {
        var json = jsonObject.EnumerateObject().First();
#if NET
        var format = Enum.Parse<SecsFormat>(json.Name);
#else
        Enum.TryParse<SecsFormat>(json.Name, out var format);
#endif
        var value = json.Value;
        return format switch
        {
            SecsFormat.List => CreateList(value),
            SecsFormat.ASCII => A(value.GetString()),
            SecsFormat.JIS8 => J(value.GetString()),
            SecsFormat.Binary => CreateBinary(value),
            SecsFormat.Boolean => CreateBoolean(value),
            SecsFormat.F4 => CreateF4(value),
            SecsFormat.F8 => CreateF8(value),
            SecsFormat.I1 => CreateI1(value),
            SecsFormat.I2 => CreateI2(value),
            SecsFormat.I4 => CreateI4(value),
            SecsFormat.I8 => CreateI8(value),
            SecsFormat.U1 => CreateU1(value),
            SecsFormat.U2 => CreateU2(value),
            SecsFormat.U4 => CreateU4(value),
            SecsFormat.U8 => CreateU8(value),
            _ => ThrowHelper(format),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateList(JsonElement jsonArray)
        {
            var items = new Item[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                items[i++] = a.ToItem();
            }
            return L(items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateBinary(JsonElement jsonArray)
        {
            var values = new byte[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetByte();
            }
            return B(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateBoolean(JsonElement jsonArray)
        {
            var values = new bool[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetBoolean();
            }
            return Boolean(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateF4(JsonElement jsonArray)
        {
            var values = new float[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetSingle();
            }
            return F4(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateF8(JsonElement jsonArray)
        {
            var values = new double[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetDouble();
            }
            return F8(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateI1(JsonElement jsonArray)
        {
            var values = new sbyte[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetSByte();
            }
            return I1(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateI2(JsonElement jsonArray)
        {
            var values = new short[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetInt16();
            }
            return I2(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateI4(JsonElement jsonArray)
        {
            var values = new int[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetInt32();
            }
            return I4(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateI8(JsonElement jsonArray)
        {
            var values = new long[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetInt64();
            }
            return I8(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateU1(JsonElement jsonArray)
        {
            var values = new byte[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetByte();
            }
            return U1(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateU2(JsonElement jsonArray)
        {
            var values = new ushort[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetUInt16();
            }
            return U2(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateU4(JsonElement jsonArray)
        {
            var values = new uint[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetUInt32();
            }
            return U4(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Item CreateU8(JsonElement jsonArray)
        {
            var values = new ulong[jsonArray.GetArrayLength()];
            int i = 0;
            foreach (var a in jsonArray.EnumerateArray())
            {
                values[i++] = a.GetUInt64();
            }
            return U8(values);
        }

        static Item ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException($"Unknown item format: {format}");
    }

    public static IEnumerable<SecsMessage> ToSecsMessages(this Stream stream)
    {
        var options = new JsonSerializerOptions
        {
            Converters = {
                new ItemJsonConverter()
            }
        };

        return JsonSerializer.Deserialize<IEnumerable<SecsMessage>>(stream, options)!;
    }
}
