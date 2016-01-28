using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public interface ITxReport {
        XElement XML { get; }
    }

    public interface ITxMessage {
        void Parse(XElement txElm);
    }
}