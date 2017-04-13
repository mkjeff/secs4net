namespace Secs4Net
{
    internal abstract class StringFormat<TFormat> : IFormat<string> where TFormat : IFormat<string>
    {
        public static readonly SecsItem Empty = new StringItem<TFormat>();

        private static readonly Pool<StringItem<TFormat>> StringItemPool = new Pool<StringItem<TFormat>>(Create);

        private static StringItem<TFormat> Create(Pool<StringItem<TFormat>> p) => new StringItem<TFormat>(p);

        /// <summary>
        /// Create <typeparamref name="TFormat"/> item
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static SecsItem Create(string str) =>
             string.IsNullOrEmpty(str) ? Empty : StringItemPool.Rent().SetValue(str);
    }

    internal sealed class ASCIIFormat : StringFormat<ASCIIFormat>
    {
        public const SecsFormat Format = SecsFormat.ASCII;
    }

    internal sealed class JIS8Format : StringFormat<JIS8Format>
    {
        public const SecsFormat Format = SecsFormat.JIS8;
    }
}
