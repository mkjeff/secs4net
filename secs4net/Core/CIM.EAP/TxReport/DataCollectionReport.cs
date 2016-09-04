using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Cim.Eap.Data;
namespace Cim.Eap.Tx {

    [DebuggerDisplay("DC Item Count={_DataItems.Count}", Name = "{ProcessJob.Id,nq}")]
    public struct DataCollectionReport : IEnumerable<DataItem>, ITxReport {
        readonly Lazy<IDictionary<string, DataItem>> _lotDatas;
        readonly Lazy<IDictionary<string, DataItem>> _waferDatas;
        readonly Lazy<IDictionary<string, DataItem>> _siteDatas;

        public readonly ProcessJob ProcessJob;

        public DataCollectionReport(ProcessJob processJob) : this(processJob, false) { }
        public DataCollectionReport(ProcessJob processJob, bool threadSafe) {
            var factory = threadSafe ?
                new Func<IDictionary<string, DataItem>>(ThreadSafeDictionaryFactory) :
                new Func<IDictionary<string, DataItem>>(DictionaryFactory);
            _lotDatas = new Lazy<IDictionary<string, DataItem>>(factory, threadSafe);
            _waferDatas = new Lazy<IDictionary<string, DataItem>>(factory, threadSafe);
            _siteDatas = new Lazy<IDictionary<string, DataItem>>(factory, threadSafe);
            ProcessJob = processJob;
        }

        static IDictionary<string, DataItem> ThreadSafeDictionaryFactory() => new ConcurrentDictionary<string, DataItem>(StringComparer.Ordinal);

        static IDictionary<string, DataItem> DictionaryFactory() => new Dictionary<string, DataItem>(StringComparer.Ordinal);

        byte GetWaferSlotNo(byte slot) {
            if (ProcessJob == null)
                return slot;

            var measurementSlots = ProcessJob.Carriers.First()._MeasurementSlots;
            if (measurementSlots == null)
                return slot;
            return measurementSlots[slot];
        }

        public void AddLotData<T>(string name, T value, Func<T, string> toStr) {
            _lotDatas.Value[name] = new DataItem {
                Type = MeasurementType.Lot,
                Name = name,
                Value = value,
                Xml = GenDataItemElm(
                    MeasurementType.Lot,
                    name, null, null, null, null,
                    toStr(value))
            };
        }

        public void AddLotData<T>(string name, T value) {
            AddLotData<T>(name, value, DefaultValueFormater);
        }

        public void AddWaferData<T>(string name, byte slot, T value, Func<T, string> toStr) {
            string carrierId = ProcessJob.Carriers.First().Id;
            _waferDatas.Value[name + ":" + slot] = new DataItem {
                Type = MeasurementType.Wafer,
                Name = name,
                SlotNo = slot,
                Value = value,
                Xml = GenDataItemElm(
                    MeasurementType.Wafer,
                    name,
                    GetWaferSlotNo(slot).ToString(),
                    slot.ToString(), null,
                    carrierId,
                    toStr(value))
            };
        }

        public void AddWaferData<T>(string name, byte slot, T value) {
            AddWaferData<T>(name, slot, value, DefaultValueFormater);
        }

        public void AddSiteData<T>(string name, byte slot, string site, T value, Func<T, string> toStr) {
            string carrierId = ProcessJob.Carriers.First().Id;
            _siteDatas.Value[name + ":" + slot + ":" + site] = new DataItem {
                Type = MeasurementType.Site,
                Name = name,
                SlotNo = slot,
                Site = site,
                Value = value,
                Xml = GenDataItemElm(
                    MeasurementType.Site,
                    name,
                    GetWaferSlotNo(slot).ToString(),
                    slot.ToString(),
                    site,
                    carrierId,
                    toStr(value))
            };
        }

        public void AddSiteData<T>(string name, byte slot, string site, T value) {
            AddSiteData<T>(name, slot, site, value, DefaultValueFormater);
        }

        static string DefaultValueFormater<T>(T value) => value == null ? string.Empty : value.ToString();

        static string ValueToXmlStr(string value) {
            if (value.Length == 0)
                return "*";
            if (value.Length > 12)
                return value.Substring(0, 12);
            return value;
        }

        static XElement GenDataItemElm(MeasurementType type, string name, string slot, string position, string site, string carrierId, string value) 
            => new XElement("DataItem",
                 new XAttribute("MeasurementType", type),
                 new XAttribute("ItemName", name),
                 new XAttribute("WaferSlotNo", slot ?? string.Empty),
                 new XAttribute("WaferPosition", position ?? string.Empty),
                 new XAttribute("SitePosition", site ?? string.Empty),
                 new XAttribute("CarrierID", carrierId ?? string.Empty),
                 new XAttribute("WaferID", string.Empty),
                ValueToXmlStr(value));

        public IEnumerable<DataItem> LotItems => _lotDatas.IsValueCreated ? _lotDatas.Value.Values : Enumerable.Empty<DataItem>();
        public IEnumerable<DataItem> WaferItems => _waferDatas.IsValueCreated ? _waferDatas.Value.Values : Enumerable.Empty<DataItem>();
        public IEnumerable<DataItem> SiteItems => _siteDatas.IsValueCreated ? _siteDatas.Value.Values : Enumerable.Empty<DataItem>();

        #region IEnumerable<DataItem> Members

        public IEnumerator<DataItem> GetEnumerator() {
            foreach (var a in LotItems)
                yield return a;
            foreach (var a in WaferItems)
                yield return a;
            foreach (var a in SiteItems)
                yield return a;
        }

        #endregion
        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        public XElement XML => new XElement("Transaction",
            new XAttribute("TxName", "DataCollectionReport"),
            new XAttribute("Type", "Event"),
            new XElement("Tool",
                new XAttribute("ToolID", string.Empty),
                new XAttribute("LoadPortID", string.Empty),
                new XAttribute("ProcessJobID", ProcessJob.Id)),
            new XElement("DataItems", new XAttribute("DataCollectionDefinitionID", ProcessJob.DataCollectionDefinitionID),
                from item in this
                select item.Xml));
    }

    [DebuggerDisplay("value={Value}", Name = "{Name}")]
    public struct DataItem {
        public MeasurementType Type;
        public string Name;
        public byte SlotNo;
        public string Site;
        public object Value;
        internal XElement Xml;
    }

    public enum MeasurementType : byte {
        Lot,
        Wafer,
        Site
    }
}