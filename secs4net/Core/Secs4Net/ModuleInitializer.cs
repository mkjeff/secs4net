#if NET
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    internal static class ModuleInitializer
    {
        [ModuleInitializer]
        internal static void Initial()
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new PlatformNotSupportedException("This version is only work on little endian hardware.");
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
#endif
