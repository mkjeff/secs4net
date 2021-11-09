# secs4net

[![NuGet](https://img.shields.io/nuget/v/secs4net.svg)](https://www.nuget.org/packages/Secs4Net)

**Project Description**  

SECS-II/HSMS-SS/GEM implementation on .NET. This library provide easy way to communicate with SEMI standard compatible device.  

**Getting started**

## Install nuget package
	> dotnet add package Secs4Net

## Configure .NET dependency injection
   ```cs
	public void ConfigureServices(IServiceCollection services)
	{
		// "secs4net" configuration section in the appsettings.json
		//  "secs4net": {
		//    "DeviceId": 0,
		//    "IsActive": true,
		//    "IpAddress": "127.0.0.1",
		//    "Port": 5000
		//  }  
		services.AddSecs4Net<DeviceLogger>(Configuration); 
	}

	class DeviceLogger : ISecsGemLogger
	{
		// implement ISecsGemLogger methods
	}
   ```

## Basic usage
   ```cs
	try
	{
		var s3f17 = new SecsMessage(3, 17)
		{
			Name = "CreateProcessJob",
			SecsItem = L(
				U4(0),
				L(
					L(
						A("Id"),
						B(0x0D),
						L(
							A("carrier id"),
							L(
								U1(1)),
							L(
								U1(1),
								A("recipe"),
								L()),
							Boolean(true),
							L()))))
		};

		//access list
		s3f17.SecsItem[1][0][0] == A("Id"); 

		foreach(var item in s3f17.SecsItem[1][0][2].Items)
		{

		}
	
		//access unmanaged arry item
		byte b2 = s3f17.SecsItem[0].FirstValue<byte>(); // with different type
		s3f17.SecsItem[0].FirstValue<byte>() = 0; // change original value 
		byte b3 = s3f17.SecsItem[0].GetFirstValueOrDefault<byte>(fallbackValueWhenItemIsEmpty); 
		Memory<byte> bytes = s3f17.SecsItem[0].GetMemory<byte>();

		// access string item
		string str = s3f17.SecsItem[1][0][0].GetString(); // str = "Id"

		//await secondary message
		var s3f18 = await secsGem.SendAsync(s3f17); 

		// process message with LINQ
		var query =
			from a in s3f18.SecsItem[3]
			select new {
				num = a.FirstValue<int>(),
			};
	}
	catch(SecsException)
	{
		// exception  when
		// T3 timeout
		// device reply SxF0
		// device reply S9Fx
	}
   ```

## Handle primary messages
   ```cs
	await foreach (var e in secsGem.GetPrimaryMessageAsync(cancellationToken))
	{     
		using var primaryMsg = e.PrimaryMessage;
		//do something for primary message

		// reply secondary message to device
		using var secondaryMsg = new SecsMessage(...);
		await e.TryReplyAsync(secondaryMsg); 
	};
   ```

## Creates `Item` via LINQ
   ```cs
	using static Secs4Net.Item;

	var s16f15 = 
		new SecsMessage(16, 15)
		{
			Name = "CreateProcessJob",
			SecsItem = L(
				U4(0),
				L(
					from pj in tx.ProcessJobs 
					select
					L(
						A(pj.Id),
						B(0x0D),
						L(
							from carrier in pj.Carriers 
							select
							L(
								A(carrier.Id),
								L(
									from slotInfo in carrier.SlotMap 
									select
									U1(slotInfo.SlotNo)))),
								L(
									U1(1),
									A(pj.RecipeId),
									L()),
								Boolean(true),
								L()))));
   ```

## Modify `Item` in restrict.
  > Basic rule: The `Item.Count` has been fixed while the item was created.

You can only overwrite values on existing memory. String Item is immutable, coz C# `string` is immutable as well.

## Reuse array for large item values
All unmanaged data Item can created from `IMemoryOwner<T>` or `Memory<T>`.

The following sample uses the implementation of `IMemoryOwner<T>` from [`Microsoft.Toolkit.HighPerformance`](https://docs.microsoft.com/en-us/windows/communitytoolkit/high-performance/memoryowner) that has been referenced internally by secs4net..
   
   ```cs
	var largeArrayOwner = MemoryOwner<int>.Allocate(size: 65535);
	
	// feed the value into largeArrayOwner.Memory or largeArrayOwner.Span
	FillLargeArray(largeArrayOwner.Memory);

	using var s6f11 = new SecsMessage(6, 11, replyExpected: false)
	{
		Name = "LargeDataEvent",
		SecsItem = L(
			L(
				I2(1121),
				A(""),
				I4(largeArrayOwner))), // create Item from largeArrayOwner
	};

	// apply using on received message as well. coz the item that decoded by PipeDecoder also using MemoryOwner<T> when the data array is big.
	using var s6f12 = await secsGem.SendAsync(s6f11);
   
   ```
   > `IMemoryOwner<T>`, `Item` and `SecsMessage` have implemented `IDisposable` don't forget to Dispose it when they don't need anymore.
	Otherwise, the array will not return to the pool till GC collects.
   
   > Since the max encoded bytes length in a single non-List Item was `16,777,215`(3 bytes), we split raw data into separated items.
	In that case, creating the Items from sliced `Memory<T>` is more efficient.

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
		(int)SecsFormat.List => 	new { item.Format, Values = OnDemandList(item),			},
		(int)SecsFormat.ASCII =>	new	{ item.Format, Values = item.GetString()			},	 
		(int)SecsFormat.JIS8 => 	new	{ item.Format, Values = item.GetString()			},
		(int)SecsFormat.Binary =>	new	{ item.Format, Values = OnDemandMemory<byte>(item)	},		 
		(int)SecsFormat.Boolean => 	new { item.Format, Values = OnDemandMemory<bool>(item)	},
		(int)SecsFormat.I8 => 		new	{ item.Format, Values = OnDemandMemory<long>(item)	},
		(int)SecsFormat.I1 => 		new	{ item.Format, Values = OnDemandMemory<sbyte>(item)	},
		(int)SecsFormat.I2 => 		new	{ item.Format, Values = OnDemandMemory<short>(item)	},
		(int)SecsFormat.I4 =>	 	new	{ item.Format, Values = OnDemandMemory<int>(item)	},
		(int)SecsFormat.F8 => 		new	{ item.Format, Values = OnDemandMemory<double>(item)},
		(int)SecsFormat.F4 => 		new	{ item.Format, Values = OnDemandMemory<float>(item)	},
		(int)SecsFormat.U8 => 		new	{ item.Format, Values = OnDemandMemory<ulong>(item)	},
		(int)SecsFormat.U1 => 		new	{ item.Format, Values = OnDemandMemory<byte>(item)	},
		(int)SecsFormat.U2 => 		new	{ item.Format, Values = OnDemandMemory<ushort>(item)},
		(int)SecsFormat.U4 => 		new	{ item.Format, Values = OnDemandMemory<uint>(item)	},
		_ => $"Unsupported Format: {item.Format}",
	};
	 
	static object OnDemandList(dynamic item) => Util.OnDemand($"[{item.Count}]", () => item.Items);
	static object OnDemandMemory<T>(dynamic item) => Util.OnDemand($"[{item.Count}]", () => item.GetMemory<T>());
}

// You can also define namespaces, non-static classes, enums, etc.
enum SecsFormat
{
	List = 0b_0000_00,
	Binary = 0b_0010_00,
	Boolean = 0b_0010_01,
	ASCII = 0b_0100_00,
	JIS8 = 0b_0100_01,
	I8 = 0b_0110_00,
	I1 = 0b_0110_01,
	I2 = 0b_0110_10,
	I4 = 0b_0111_00,
	F8 = 0b_1000_00,
	F4 = 0b_1001_00,
	U8 = 0b_1010_00,
	U1 = 0b_1010_01,
	U2 = 0b_1010_10,
	U4 = 0b_1011_00,
}
```