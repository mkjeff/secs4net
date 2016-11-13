namespace Secs4Net
{
    public abstract class StringFormat<TFormat> : IFormat<string> where TFormat : IFormat<string>
    {
        public static readonly SecsItem Empty = new StringItem<TFormat>();

        private static readonly Pool<SecsItem<TFormat, string>> StringItemPool
            = new Pool<SecsItem<TFormat, string>>(p => new StringItem<TFormat>(p));

        /// <summary>
        /// Create <typeparamref name="TFormat"/> item
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static SecsItem Create(string str)
        {
            if (string.IsNullOrEmpty(str))
                return Empty;

            var item = StringItemPool.Acquire();
            item.SetValue(str);
            return item;
        }

        internal StringFormat()
        {
        }
    }

    public sealed class ASCIIFormat : StringFormat<ASCIIFormat>
    {
        public const SecsFormat Format = SecsFormat.ASCII;
    }

    public sealed class JIS8Format : StringFormat<JIS8Format>
    {
        public const SecsFormat Format = SecsFormat.JIS8;
    }
}
