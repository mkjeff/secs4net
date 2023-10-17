## LINQPad
If you like to use LINQPad to dump `Item` object in a simplified format, you probably need a [custom dump method](http://www.linqpad.net/CustomizingDump.aspx) method. Adjust your LINQPad **My Extensions** file as follow
```cs
static object ToDump(object obj)
{
    var objType = obj.GetType();
    if (Type.GetType("Secs4Net.Item,Secs4Net") is { } itemType && objType.IsSubclassOf(itemType))
    {
        return Dump(obj);
    }
    return obj;
}

static object Dump(dynamic item)
{
    return (int)item.Format switch
    {
        (int)SecsFormat.List => 	new { item.Format, Values = OnDemandList(item),	        },
        (int)SecsFormat.ASCII =>	new { item.Format, Values = item.GetString()	        },	 
        (int)SecsFormat.JIS8 => 	new { item.Format, Values = item.GetString()	        },
        (int)SecsFormat.Binary =>	new { item.Format, Values = OnDemandMemory<byte>(item)  },		 
        (int)SecsFormat.Boolean => 	new { item.Format, Values = OnDemandMemory<bool>(item)	},
        (int)SecsFormat.I8 => 		new { item.Format, Values = OnDemandMemory<long>(item)	},
        (int)SecsFormat.I1 => 		new { item.Format, Values = OnDemandMemory<sbyte>(item)	},
        (int)SecsFormat.I2 => 		new { item.Format, Values = OnDemandMemory<short>(item)	},
        (int)SecsFormat.I4 =>	 	new { item.Format, Values = OnDemandMemory<int>(item)	},
        (int)SecsFormat.F8 => 		new { item.Format, Values = OnDemandMemory<double>(item)},
        (int)SecsFormat.F4 => 		new { item.Format, Values = OnDemandMemory<float>(item)	},
        (int)SecsFormat.U8 => 		new { item.Format, Values = OnDemandMemory<ulong>(item)	},
        (int)SecsFormat.U1 => 		new { item.Format, Values = OnDemandMemory<byte>(item)	},
        (int)SecsFormat.U2 => 		new { item.Format, Values = OnDemandMemory<ushort>(item)},
        (int)SecsFormat.U4 => 		new { item.Format, Values = OnDemandMemory<uint>(item)	},
        _ => $"Unsupported Format: {item.Format}",
    };
     
    static object OnDemandList(dynamic item) => Util.OnDemand($"[{item.Count}]", () => item.Items);
    static object OnDemandMemory<T>(dynamic item) => Util.OnDemand($"[{item.Count}]", () => item.GetMemory<T>());
}

// You can also define namespaces, non-static classes, enums, etc.
enum SecsFormat
{
    List    = 0b_0000_00,
    Binary  = 0b_0010_00,
    Boolean = 0b_0010_01,
    ASCII   = 0b_0100_00,
    JIS8    = 0b_0100_01,
    I8      = 0b_0110_00,
    I1      = 0b_0110_01,
    I2      = 0b_0110_10,
    I4      = 0b_0111_00,
    F8      = 0b_1000_00,
    F4      = 0b_1001_00,
    U8      = 0b_1010_00,
    U1      = 0b_1010_01,
    U2      = 0b_1010_10,
    U4      = 0b_1011_00,
}
```