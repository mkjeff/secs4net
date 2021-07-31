using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
namespace Cim.Eap {
    sealed class EAPConfig : ConfigurationSection {
        public static EAPConfig Instance = (EAPConfig)ConfigurationManager.GetSection("eap");

        [ConfigurationProperty("id", DefaultValue = "EQP", IsRequired = true)]
        [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 3, MaxLength = 7)]
        public string ToolId => (string)this["id"];

        [ConfigurationProperty("tcs")]
        [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
        public string TcsId => string.IsNullOrEmpty((string)this["tcs"]) ? this.ToolId : (string)this["tcs"];

        [Description(@"預先定義好的SECS message清單")]
        [ConfigurationProperty("sml", IsRequired = true)]
        [CallbackValidator(Type = typeof(EAPConfig), CallbackMethodName = "CheckFileExist")]
        public string SmlFile => (string)this["sml"];

        [Description(@"GEM event report define link config file")]
        [ConfigurationProperty("gem", IsRequired = true)]
        [CallbackValidator(Type = typeof(EAPConfig), CallbackMethodName = "CheckFileExist")]
        public string GemXml => (string)this["gem"];

        [ConfigurationProperty("driver", IsRequired = true)]
        [TypeConverter(typeof(EapDriverConverter))]
        public EapDriver Driver => (EapDriver)this["driver"];

        [DisplayName("IP Address")]
        [Description("Active Mode: EQP IP address, Passive Mode: local socket binding address")]
        [ConfigurationProperty("ip", DefaultValue = "127.0.0.1", IsRequired = true)]
        [CallbackValidator(Type = typeof(EAPConfig), CallbackMethodName = "IPAddressCheck")]
        public string IP => (string)this["ip"];

        [Description("Active Mode: EQP socket listen port, Passive Mode: local socket listen port")]
        [ConfigurationProperty("port", DefaultValue = 5000, IsRequired = true)]
        [IntegerValidator(MinValue = 4000, MaxValue = 5000)]
        public int TcpPort => (int)this["port"];

        [ConfigurationProperty("deviceId", DefaultValue = (ushort)0)]
        public ushort DeviceId => (ushort)this["deviceId"];

        [Description("Active / Passive")]
        [ConfigurationProperty("mode", IsRequired = true)]
        public ConnectionMode Mode => (ConnectionMode)this["mode"];

        [Description("Socket Receive Buffer Size")]
        [ConfigurationProperty("recvBufferSize", DefaultValue = 16 * 1024)]
        [IntegerValidator(MinValue = 8 * 1024, MaxValue = 64 * 1024)]
        public int SocketRecvBufferSize => (int)this["recvBufferSize"];

        [Description("LinkTest interval time(豪秒)")]
        [ConfigurationProperty("LinkTest", DefaultValue = 30000)]
        [IntegerValidator(MinValue = 20000, MaxValue = 60000)]
        public int LinkTestInterval => (int)this["LinkTest"];

        [Description("T3 timeout(豪秒)")]
        [ConfigurationProperty("t3", DefaultValue = 45000)]
        [IntegerValidator(MinValue = 30000, MaxValue = 120000)]
        [DefaultValue((int)45000)]
        public int T3 => (int)this["t3"];

        [Description("T5 timeout(豪秒)")]
        [ConfigurationProperty("t5", DefaultValue = 10000)]
        [IntegerValidator(MinValue = 10000, MaxValue = 20000)]
        public int T5 => (int)this["t5"];

        [Description("T6 timeout(豪秒)")]
        [ConfigurationProperty("t6", DefaultValue = 5000)]
        [IntegerValidator(MinValue = 5000, MaxValue = 10000)]
        public int T6 => (int)this["t6"];

        [Description("T7 timeout(豪秒)")]
        [ConfigurationProperty("t7", DefaultValue = 10000)]
        [IntegerValidator(MinValue = 1000, MaxValue = 240000)]
        public int T7 => (int)this["t7"];

        [Description("T8 timeout(豪秒)")]
        [ConfigurationProperty("t8", DefaultValue = 5000)]
        [IntegerValidator(MinValue = 1000, MaxValue = 20000)]
        public int T8 => (int)this["t8"];

        public static void CheckFileExist(object value) {
            var filePath = value as string;        
            if (!string.IsNullOrWhiteSpace(filePath) && !File.Exists(filePath))
                throw new FileNotFoundException("相關組態檔找不到", value as string);
        }
        public static void IPAddressCheck(object value) {
            IPAddress.Parse(value as string);
        }
    }

    sealed class EapDriverConverter: ConfigurationConverterBase {
        static EapDriverConverter() {
            TypeDescriptor.AddAttributes(typeof(EapDriver), new TypeConverterAttribute(typeof(EapDriverConverter)));
        }
        static readonly TypeNameConverter typeNameConverter = new TypeNameConverter();
        static readonly SubclassTypeValidator validator = new SubclassTypeValidator(typeof(EapDriver));
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            Type type = (Type)typeNameConverter.ConvertFrom(context, culture, value);
            if (!type.IsClass) throw new ConfigurationErrorsException(type.FullName + "不是類別");
            validator.Validate(type);
            return Activator.CreateInstance(type);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) => value.GetType().AssemblyQualifiedName;
    }

    public enum ConnectionMode {
        Active,
        Passive
    }
}