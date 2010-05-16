using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Secs4Net {
    [Serializable]
    public sealed class SecsMessage : MarshalByRefObject, ISerializable {
        static SecsMessage() {
            if (!BitConverter.IsLittleEndian)
                throw new InvalidProgramException("This version is only work on little endian hardware.");
        }

        public override object InitializeLifetimeService() {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial) {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(30);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(10);
            }
            return lease;
        }

        public override string ToString() {
            return (Name ?? "Unknown") + ":'S" + S.ToString() + "F" + F.ToString() + "'" + (ReplyExpected ? " W" : string.Empty);
        }

        public byte S { get; private set; }
        public byte F { get; private set; }
        public bool ReplyExpected { get; internal set; }
        public Item SecsItem { get; private set; }
        public string Name { get; set; }
        public object Tag { get; set; }

        public ReadOnlyCollection<RawData> RawDatas { get { return _rawDatas.Value; } }
        readonly Lazy<ReadOnlyCollection<RawData>> _rawDatas;
        static readonly Lazy<ReadOnlyCollection<RawData>> emptyMsgDatas = Lazy.Create(new List<RawData> { new RawData(new byte[] { 0, 0, 0, 10 }), null }.AsReadOnly());
        #region Constructor

        public SecsMessage(byte s, byte f, string name, bool replyExpected, Item item) {
            if (s > 0x7F)
                throw new ArgumentOutOfRangeException("s", s, "Stream number不能超過127");

            this.S = s;
            this.F = f;
            this.Name = name;
            this.ReplyExpected = replyExpected;
            this.SecsItem = item;

            this._rawDatas = item == null ? emptyMsgDatas : Lazy.Create(() => {
                var result = new List<RawData> { null, null };
                uint length = 10 + Encode(SecsItem, result);
                byte[] msgLengthByte = BitConverter.GetBytes(length);
                Array.Reverse(msgLengthByte);
                result[0] = new RawData(msgLengthByte);
                return result.AsReadOnly();
            });
        }

        static uint Encode(Item item, List<RawData> buffer) {
            uint length = (uint)item.RawData.Count;
            buffer.Add(item.RawData);
            if (item.Format == SecsFormat.List)
                foreach (var secsItem in item.Items)
                    length += Encode(secsItem, buffer);
            return length;
        }

        public SecsMessage(byte s, byte f, string name, Item item)
            : this(s, f, name, true, item) { }

        public SecsMessage(byte s, byte f, string name)
            : this(s, f, name, null) { }

        internal SecsMessage(byte s, byte f, bool replyExpected, byte[] itemBytes, ref int index)
            : this(s, f, "Unknown", replyExpected, Decode(itemBytes, ref index)) { }

        #endregion
        #region SecsMessage => SML
        const int SmlIndent = 2;

        public void WriteTo(TextWriter writer) {
            writer.WriteLine(this.ToString());
            Write(writer, SecsItem, SmlIndent);
            writer.Write('.');
        }

        static void Write(TextWriter writer, Item item, int indent) {
            if (item == null) return;
            var indentStr = new string(' ', indent);
            writer.Write(indentStr);
            writer.Write('<');
            writer.Write(item.Format.ToSML());
            writer.Write(" [");
            writer.Write(item.Count);
            writer.Write("] ");
            switch (item.Format) {
                case SecsFormat.List:
                    writer.WriteLine();
                    foreach (var subItem in item.Items)
                        Write(writer, subItem, indent + SmlIndent);
                    writer.Write(indentStr);
                    break;
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    writer.Write('\'');
                    writer.Write(item.ToString());
                    writer.Write('\'');
                    break;
                default:
                    writer.Write(item.ToString());
                    break;
            }
            writer.WriteLine('>');
        }
        #endregion
        #region ISerializable Members
        SecsMessage(SerializationInfo info, StreamingContext context) {
            this.S = info.GetByte("s");
            this.F = info.GetByte("f");
            this.ReplyExpected = info.GetBoolean("w");
            this.Name = info.GetString("name");
            this._rawDatas = Lazy.Create(info.GetValue("rawbytes", typeof(ReadOnlyCollection<RawData>)) as ReadOnlyCollection<RawData>);
            int i = 0;
            if (this._rawDatas.Value.Count > 2)
                this.SecsItem = Decode((from arr in this._rawDatas.Value.Skip(2)
                                        from b in arr.Bytes
                                        select b).ToArray(), ref i);
        }

        [SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("s", this.S);
            info.AddValue("f", this.F);
            info.AddValue("w", this.ReplyExpected);
            info.AddValue("name", this.Name);
            info.AddValue("rawbytes", _rawDatas.Value);
        }
        #endregion

        static Item Decode(byte[] bytes, ref int index) {
            var format = (SecsFormat)(bytes[index] & 0xFC);
            var lengthBits = (byte)(bytes[index] & 3);
            index++;

            var itemLengthBytes = new byte[4];
            Array.Copy(bytes, index, itemLengthBytes, 0, lengthBits);
            Array.Reverse(itemLengthBytes, 0, lengthBits);
            int length = BitConverter.ToInt32(itemLengthBytes, 0);  // max to 3 byte length
            index += lengthBits;

            Item item = null;
            if (format == SecsFormat.List) {
                if (length == 0)
                    item = Item.L();
                else {
                    var list = new List<Item>(length);
                    for (int i = 0; i < length; i++)
                        list.Add(Decode(bytes, ref index));
                    item = Item.L(list);
                }
            } else {
                item = SecsExtension.BytesDecoders[format](bytes, index, length);
                index += length;
            }
            return item;
        }
    }
}
