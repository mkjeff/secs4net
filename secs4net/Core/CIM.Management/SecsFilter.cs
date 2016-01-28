using System;
using Secs4Net;

namespace Cim.Management {
    public interface ISecsFilter {
        byte S { get; }
        byte F { get; }
        string Name { get; }
        string Description { get; }
        bool Eval(Item root);
    }

    [Serializable]
    public struct SecsFilter : ISecsFilter {
        public string Name { get; set; }
        public byte S { get; set; }
        public byte F { get; set; }

        public bool Eval(Item root) => true;

        public string Description => "S" + S + "F" + F + ":" + Name;
    }

    [Serializable]
    public struct S6F11Filter : ISecsFilter {
        public string Name { get; set; }
        public string CEID;

        public bool Eval(Item root) => CEID == root.Items[1].ToString();

        public byte S => 6;
        public byte F => 11;

        public string Description => "S6F11:" + Name + " CEID=" + CEID;
    }

    [Serializable]
    public struct AlarmFilter : ISecsFilter {
        public string AlarmId;

        public string Name { get; set; }
        public byte S => 5;
        public byte F => 1;

        public bool Eval(Item root) => AlarmId == root.Items[1].ToString();

        public string Description => "S5F1:" + Name + " AlarmId=" + AlarmId;
    }
}
