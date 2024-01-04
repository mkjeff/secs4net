# secs4net

[![.NET](https://github.com/mkjeff/secs4net/actions/workflows/dotnet.yml/badge.svg)](https://github.com/mkjeff/secs4net/actions/workflows/dotnet.yml) [![Nuget](https://img.shields.io/nuget/dt/secs4net)](https://www.nuget.org/stats/packages/Secs4Net?groupby=Version) [![NuGet](https://img.shields.io/nuget/v/secs4net.svg)](https://www.nuget.org/packages/Secs4Net) [![codecov](https://codecov.io/gh/mkjeff/secs4net/graph/badge.svg?token=AgiQxizSvE)](https://codecov.io/gh/mkjeff/secs4net)

**Project Description**  

SECS-II/HSMS-SS/GEM implementation on .NET. This library provides an easy way to communicate with SEMI-standard compatible devices.  

**Getting started**

## Install Nuget package
    > dotnet add package Secs4Net

## Configure via .NET dependency injection
[Sample code reference](https://github.com/mkjeff/secs4net/blob/base/samples/DeviceWorkerService/ServiceProvider.cs)
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

    //access an unmanaged array item
    byte b2 = s3f17.SecsItem[0].FirstValue<byte>(); // with different type
    s3f17.SecsItem[0].FirstValue<byte>() = 0; // change original value 
    byte b3 = s3f17.SecsItem[0].GetFirstValueOrDefault<byte>(fallbackValueWhenItemIsEmpty); 
    Memory<byte> bytes = s3f17.SecsItem[0].GetMemory<byte>();

    // access string item
    string str = s3f17.SecsItem[1][0][0].GetString(); // str = "Id"

    //await the secondary message
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

## Change the `Item` value (restricted)
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

// apply using on received message as well. coz the item decoded by PipeDecoder also uses MemoryOwner<T> when the data array is big.
using var s6f12 = await secsGem.SendAsync(s6f11);
```
   > `IMemoryOwner<T>`, `Item`, and `SecsMessage` have implemented `IDisposable` don't forget to `Dispose` it when they don't need anymore.
    Otherwise, the array will not return to the pool till GC collects.
   
   > Since the length of the max encoded bytes in a single non-List Item was `16,777,215`(3 bytes), we split raw data into separated items.
    In that case, creating the Items from sliced `Memory<T>` is more efficient.
