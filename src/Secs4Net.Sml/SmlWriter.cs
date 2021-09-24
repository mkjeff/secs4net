using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Newtonsoft.Json;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Secs4Net.Sml;

public static class SmlWriter
{
    public static int SmlIndent = 4;

    public static string ToSml(this SecsMessage msg)
    {
        using var sw = new StringWriter();
        msg.WriteSmlTo(sw);
        return sw.ToString();
    }

    public static void WriteSmlTo(this SecsMessage msg, TextWriter writer, int indent = 4)
    {
        if (msg.Name is not null)
        {
            writer.Write(msg.Name);
            writer.Write(':');
        }
        writer.Write("'S");
        writer.Write(msg.S);
        writer.Write('F');
        writer.Write(msg.F);
        writer.Write('\'');
        if (msg.ReplyExpected)
        {
            writer.Write('W');
        }
        writer.WriteLine();

        if (msg.SecsItem is not null)
        {
            writer.Write(msg.SecsItem, indent);
        }

        writer.WriteLine('.');
    }

    public static async Task WriteSmlToAsync(this SecsMessage msg, TextWriter writer, int indent = 4)
    {
        if (msg.Name is not null)
        {
            writer.Write(msg.Name);
            writer.Write(':');
        }
        writer.Write("'S");
        writer.Write(msg.S);
        writer.Write('F');
        writer.Write(msg.F);
        writer.Write('\'');
        if (msg.ReplyExpected)
        {
            writer.Write('W');
        }
        writer.WriteLine();

        if (msg.SecsItem is not null)
        {
            await writer.WriteAsync(msg.SecsItem, indent).ConfigureAwait(false);
        }

        await writer.WriteLineAsync('.').ConfigureAwait(false);
    }

    public static void Write(this TextWriter writer, Item item, int indent = 4)
    {
        var indentStr = new string(' ', indent);
        writer.Write(indentStr);
        writer.Write('<');
        writer.Write(item.Format.ToSml());
        writer.Write(" [");
        writer.Write(item.Count);
        writer.Write("] ");
        switch (item.Format)
        {
            case SecsFormat.List:
                writer.WriteLine();
                foreach (var a in item.Items)
                {
                    writer.Write(a, indent + SmlIndent);
                }
                writer.Write(indentStr);
                break;
            case SecsFormat.ASCII:
            case SecsFormat.JIS8:
                writer.Write('\'');
                writer.Write(item.GetString());
                writer.Write('\'');
                break;
            case SecsFormat.Binary:
                WriteHexArray(writer, item.GetMemory<byte>());
                break;
            case SecsFormat.F4:
                writer.WriteArray(item.GetMemory<float>());
                break;
            case SecsFormat.F8:
                writer.WriteArray(item.GetMemory<double>());
                break;
            case SecsFormat.I1:
                writer.WriteArray(item.GetMemory<sbyte>());
                break;
            case SecsFormat.I2:
                writer.WriteArray(item.GetMemory<short>());
                break;
            case SecsFormat.I4:
                writer.WriteArray(item.GetMemory<int>());
                break;
            case SecsFormat.I8:
                writer.WriteArray(item.GetMemory<long>());
                break;
            case SecsFormat.U1:
                writer.WriteArray(item.GetMemory<byte>());
                break;
            case SecsFormat.U2:
                writer.WriteArray(item.GetMemory<ushort>());
                break;
            case SecsFormat.U4:
                writer.WriteArray(item.GetMemory<uint>());
                break;
            case SecsFormat.U8:
                writer.WriteArray(item.GetMemory<ulong>());
                break;
            case SecsFormat.Boolean:
                writer.WriteArray(item.GetMemory<bool>());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(item.Format), item.Format, "invalid SecsFormat value");
        }
        writer.WriteLine('>');
    }

    public static async Task WriteAsync(this TextWriter writer, Item item, int indent = 4)
    {
        var indentStr = new string(' ', indent);
        await writer.WriteAsync(indentStr).ConfigureAwait(false);
        await writer.WriteAsync($"<{item.Format.ToSml()} [{item.Count}] ").ConfigureAwait(false);
        switch (item.Format)
        {
            case SecsFormat.List:
                await WriteListAsnc(writer, item, indent, indentStr).ConfigureAwait(false);
                break;
            case SecsFormat.ASCII:
            case SecsFormat.JIS8:
                writer.Write('\'');
                writer.Write(item.GetString());
                writer.Write('\'');
                break;
            case SecsFormat.Binary:
                writer.WriteHexArray(item.GetMemory<byte>());
                break;
            case SecsFormat.F4:
                writer.WriteArray(item.GetMemory<float>());
                break;
            case SecsFormat.F8:
                writer.WriteArray(item.GetMemory<double>());
                break;
            case SecsFormat.I1:
                writer.WriteArray(item.GetMemory<sbyte>());
                break;
            case SecsFormat.I2:
                writer.WriteArray(item.GetMemory<short>());
                break;
            case SecsFormat.I4:
                writer.WriteArray(item.GetMemory<int>());
                break;
            case SecsFormat.I8:
                writer.WriteArray(item.GetMemory<long>());
                break;
            case SecsFormat.U1:
                writer.WriteArray(item.GetMemory<byte>());
                break;
            case SecsFormat.U2:
                writer.WriteArray(item.GetMemory<ushort>());
                break;
            case SecsFormat.U4:
                writer.WriteArray(item.GetMemory<uint>());
                break;
            case SecsFormat.U8:
                writer.WriteArray(item.GetMemory<ulong>());
                break;
            case SecsFormat.Boolean:
                writer.WriteArray(item.GetMemory<bool>());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(item.Format), item.Format, "invalid SecsFormat value");
        }

        await writer.WriteLineAsync('>').ConfigureAwait(false);

        static async Task WriteListAsnc(TextWriter writer, Item item, int indent, string indentStr)
        {
            await writer.WriteLineAsync().ConfigureAwait(false);
            foreach (var a in item.Items)
            {
                await WriteAsync(writer, a, indent + SmlIndent).ConfigureAwait(false);
            }

            await writer.WriteAsync(indentStr).ConfigureAwait(false);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteArray<T>(this TextWriter writer, Memory<T> memory)
#if NET6_0
            where T : unmanaged, ISpanFormattable
#else
            where T: unmanaged
#endif
    {
        if (memory.IsEmpty)
        {
            return;
        }

        var array = memory.Span;
        ref var rStart = ref array.DangerousGetReferenceAt(0);
        ref var rEnd = ref array.DangerousGetReferenceAt(array.Length - 1);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            WriteValue(writer, rStart);
            writer.Write(' ');
            rStart = ref Unsafe.Add(ref rStart, 1);
        }
        WriteValue(writer, rStart);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void WriteValue(TextWriter writer, T value)
#if NET6_0
                => writer.WriteSpanFormattableValue(value);
#else
                => writer.Write(value.ToString());
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteArray(this TextWriter writer, Memory<float> memory)
    {
        if (memory.IsEmpty)
        {
            return;
        }

        var array = memory.Span;
        ref var rStart = ref array.DangerousGetReferenceAt(0);
        ref var rEnd = ref array.DangerousGetReferenceAt(array.Length - 1);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            WriteValue(writer, rStart);
            writer.Write(' ');
            rStart = ref Unsafe.Add(ref rStart, 1);
        }
        WriteValue(writer, rStart);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void WriteValue(TextWriter writer, float value)
#if NET6_0
                => writer.WriteSpanFormattableValue(value);
#else
                => writer.Write(value.ToString("G9", CultureInfo.InvariantCulture));
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteArray(this TextWriter writer, Memory<bool> memory)
    {
        if (memory.IsEmpty)
        {
            return;
        }

        var array = memory.Span;
        ref var rStart = ref array.DangerousGetReferenceAt(0);
        ref var rEnd = ref array.DangerousGetReferenceAt(array.Length - 1);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            writer.Write(rStart.ToString());
            writer.Write(' ');
            rStart = ref Unsafe.Add(ref rStart, 1);
        }
        writer.Write(rStart.ToString());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteHexArray(this TextWriter writer, ReadOnlyMemory<byte> memory)
    {
        if (memory.IsEmpty)
        {
            return;
        }

        var array = memory.Span;
        ref var rStart = ref array.DangerousGetReferenceAt(0);
        ref var rEnd = ref array.DangerousGetReferenceAt(array.Length - 1);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            AppendHexChars(writer, rStart);
            writer.Write(' ');
            rStart = ref Unsafe.Add(ref rStart, 1);
        }
        AppendHexChars(writer, rStart);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AppendHexChars(TextWriter sb, byte num)
        {
            sb.Write("0x");
            var hex1 = Math.DivRem(num, 0x10, out var hex0);
            sb.Write(GetHexChar(hex1));
            sb.Write(GetHexChar(hex0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char GetHexChar(int i) => (i < 10) ? (char)(i + 0x30) : (char)(i - 10 + 0x41);
    }

#if NET6_0
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteSpanFormattableValue<T>(this TextWriter writer, T value) where T : unmanaged, ISpanFormattable
    {
        using var spanOwner = SpanOwner<char>.Allocate(128);
        if (value.TryFormat(spanOwner.Span, out var writtenCount, default, CultureInfo.InvariantCulture))
        {
            writer.Write(spanOwner.Span.Slice(0, writtenCount));
        }
        else
        {
            writer.Write(value.ToString());
        }
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string ToSml(this SecsFormat format)
    {
        return format switch
        {
            SecsFormat.List => "L",
            SecsFormat.Binary => "B",
            SecsFormat.Boolean => "Boolean",
            SecsFormat.ASCII => "A",
            SecsFormat.JIS8 => "J",
            SecsFormat.I8 => "I8",
            SecsFormat.I1 => "I1",
            SecsFormat.I2 => "I2",
            SecsFormat.I4 => "I4",
            SecsFormat.F8 => "F8",
            SecsFormat.F4 => "F4",
            SecsFormat.U8 => "U8",
            SecsFormat.U1 => "U1",
            SecsFormat.U2 => "U2",
            SecsFormat.U4 => "U4",
            _ => ThrowHelper(format),
        };

        [DoesNotReturn]
        static string ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException(nameof(format), (int)format, "Invalid enum value");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Any Class</typeparam>
    /// <param name="t">Class Instance</param>
    /// <returns>Sml String</returns>
    public static string ToSml<T>(this T t) where T : class
    {
        string result = string.Empty;
        if (t.GetType().IsPrimitive)  //IsPrimitiv判断是否基本类型
        {
            result += $"<{TypeToFormat(t)}[1] {t}>\r\n";
            return result;
        }
        else if (t is string)
        {
            //如果有中文则要用Unicode编码
            //var bytes = Encoding.UTF8.GetBytes(t as string);  //Unicode
            //result += $"<{TypeToFormat(bytes)}[{bytes.Length}] {string.Join(" ", bytes)}>\r\n";
            //如果无中文则用ASC编码
            result += $"<{TypeToFormat(t)}[{(t as string).Length}] {t}>\r\n";
            return result;
        }
        else
        {
            if (t is Array)  //数组分情况处理
            {
                Array array = t as Array;
                object obj = array.GetValue(0);
                if (obj.GetType().IsPrimitive || t is string)
                {
                    List<object> objList = new List<object>();
                    foreach (var item in array)
                    {
                        objList.Add(item);
                    }
                    result += $"<{TypeToFormat(obj)}[{array.Length}] {string.Join(" ", objList)}>\r\n";
                    return result;
                }
                else
                {
                    result += $"<L[{array.Length}]\r\n";
                    foreach (var item in array)
                    {
                        var result1 = ToSml(item);
                        result += result1;
                    }
                    result += $">\r\n";
                }
            }
            else  //不是基本类型不是string不是数组，则是struct或class，struct是System.ValueType，class是引用类型
            {
                FieldInfo[] fInfo = t.GetType().GetFields();
                result = $"<L[{fInfo.Length}]\r\n";
                foreach (FieldInfo item in fInfo)
                {
                    object obj = item?.GetValue(t);
                    if (obj != null)
                    {
                        //string typeName = Regex.Split(item.ToString(), " ")[0];  //获取类型名称
                        //string varName = Regex.Split(item.ToString(), " ")[1];  //获取变量名称
                        var result1 = ToSml(obj);
                        result += result1;
                    }
                }
                result += $">\r\n";
            }
        }
        return result;
    }

    public static T ToType<T>(this SecsMessage msg) where T : class, new()
    {
        try
        {
            if (msg != null && msg.SecsItem != null)
            {
                string sml = msg.ToSml();
                string[] smlList = sml.Split(new string[] { "<", ">" }, StringSplitOptions.None);

                string json = JsonConvert.SerializeObject(new T());
                List<string> jsonListFinal = new List<string>();
                string[] jsonList = json.Split(new string[] { ":" }, StringSplitOptions.None);  //为了保留分隔符，分多次Split
                for (int i = 0; i < jsonList.Length; i++)
                {
                    if (i != jsonList.Length - 1)
                    {
                        jsonList[i] += ":";
                    }
                    string[] jsonList1 = jsonList[i].Split(new string[] { ",\"" }, StringSplitOptions.None);
                    for (int j = 0; j < jsonList1.Length; j++)
                    {
                        if (j != 0)
                        {
                            jsonList1[j] = ",\"" + jsonList1[j];
                        }
                        string[] jsonList2 = jsonList1[j].Split(new string[] { ",{" }, StringSplitOptions.None);
                        for (int k = 0; k < jsonList2.Length; k++)
                        {
                            if (k != jsonList2.Length - 1)
                            {
                                jsonList2[k] += ",{";
                            }
                        }
                        jsonListFinal.AddRange(jsonList2);
                    }
                }

                //先拿到json，然后拿到sml，遍历jsonList，然后挑选sml中对应的数据填到json里，最后再把json转成T
                int row = 0;
                for (int i = 0; i < jsonListFinal.Count; i++)
                {
                    if (!jsonListFinal[i].Contains(":"))
                    {
                        int index = -1;
                        string format = string.Empty;
                        bool bValid = false;
                        while (!bValid)
                        {
                            row++;
                            bValid = GetValid();
                        }
                        bool GetValid()
                        {
                            index = smlList[row].IndexOf("[");
                            if (index < 0)
                            {
                                return false;
                            }
                            format = smlList[row].Substring(0, index - 1);
                            if (!bPrimitive(format))
                            {
                                return false;
                            }
                            return true;
                        }

                        int index1 = smlList[row].IndexOf("]");
                        int count = int.Parse(smlList[row].Substring(index + 1, index1 - index - 1));
                        if (count == 1)
                        {
                            jsonListFinal[i] = jsonListFinal[i];
                        }
                        else
                        {
                            if (format == "B")
                            {
                                string temp = smlList[row].Substring(index1 + 2);
                                byte[] bytes = temp.Split(new string[] { " " }, StringSplitOptions.None).Select(item => Convert.ToByte(item, 16)).ToArray();
                                string str = Encoding.UTF8.GetString(bytes);
                                int index2 = jsonListFinal[i].IndexOf("\"");
                                int index3 = jsonListFinal[i].IndexOf("\"", index2 + 1);
                                string str2 = jsonListFinal[i].Substring(0, index2 + 1);
                                string str3 = jsonListFinal[i].Substring(index3);
                                jsonListFinal[i] = str2 + str + str3;
                            }
                            else if(format == "A")
                            {
                                string temp = smlList[row].Substring(index1 + 2);
                                jsonListFinal[i] = temp;
                            }
                            else
                            {
                                string temp = smlList[row].Substring(index1 + 2);
                                temp = $"{temp.Replace(" ", ",")}";
                                int index2 = jsonListFinal[i].IndexOf("[");
                                int index3 = jsonListFinal[i].IndexOf("]");
                                string str2 = jsonListFinal[i].Substring(0, index2 + 1);
                                string str3 = jsonListFinal[i].Substring(index3);
                                jsonListFinal[i] = str2 + temp + str3;
                            }
                        }
                    }
                }
                string json1 = string.Empty;
                for (int i = 0; i < jsonListFinal.Count; i++)
                {
                    json1 += jsonListFinal[i];
                }

                //这句话先new一个T，然后往T里填合适的数据，类型不对也不会报错
                T t1 = JsonConvert.DeserializeObject<T>(json1);  //从SecsMessage中解析出来的值：{"温度":999,"功率":[123,456]}
                
                return t1;
            }
            else
            {
                return default(T);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static string TypeToFormat(object obj)
    {
        if (obj is Array)  //二进制数组
            return "B";
        else if (obj is string)  //ASC字符
            return "A";
        else if (obj is bool)
            return "Boolean";
        else if (obj is long)
            return "I8";
        else if (obj is int)
            return "I4";
        else if (obj is short)
            return "I2";
        else if (obj is sbyte)
            return "I1";
        else if (obj is ulong)
            return "U8";
        else if (obj is uint)
            return "U4";
        else if (obj is ushort)
            return "U2";
        else if (obj is byte)
            return "U1";
        else if (obj is double)
            return "F8";
        else if (obj is float)
            return "F4";
        return "";
    }
    public static bool bPrimitive(string str)
    {
        var array = new string[] { "B", "A", "Boolean", "I8", "I4", "I2", "I1", "U8", "U4", "U2", "U1", "F8", "F4" };
        return array.Contains(str);
    }
}
