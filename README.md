# secs4net

### Note: This project is just an implementation reference. 

About the v2, it is almost done. but it is hard to be compatible with v1.
.net core is the mainstream already, opposite .net framework stuck on the maintenance stage.

As mention above, as an implementation reference, I would try to keep the source code is clean and easy to understanding. Product support is not my goal coz I'm not in this industry for a long time.

If you already use secs4net in your product and can't migrate your system to `.NET 6.0`. I'd recommend using the fork version from [@TiltonJH](https://github.com/TiltonJH/secs4net).

[![NuGet](https://img.shields.io/nuget/v/secs4net.svg)](https://www.nuget.org/packages/secs4net/)

**Project Description**  
Note: Secs4net will only support .net6.0+ starting from v2.

SECS-II/HSMS-SS/GEM implementation on .NET. This library provide easy way to communicate with SEMI standard compatible device.  

1\. Send message to device
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
    s3f17.SecsItem[1][0][2].Take(1..2); // LINQ Take with range
  
    //access item value
    byte b2 = s3f18.SecsItem[0].FirstValue<byte>(); // with different type
    s3f18.SecsItem[0].FirstValue<byte>() = 0; // change original value 
    s3f18.SecsItem[0].GetFirstValueOrDefault<byte>(fallbackValue); 
    string str = s3f18.SecsItem[0].GetString();

    //await secondary message
    var s3f18 = await device.SendAsync(s3f17); 

    // LINQ query
    var query =
        from a in s3f18.SecsItem.Items[3]
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
2\. Handle primary message via async-stream
```cs
await foreach (var e in _secsGem.GetPrimaryMessageAsync(cancellationToken))
{
    try 
    {
        //do something for primaryMsg
        var primaryMsg = e.PrimaryMessage;
      
        // reply secondary msg to device
        await e.TryReplyAsync( secondaryMsg ); 
    }
    catch (Exception ex) 
    {

    }
};
```

3\. SecsMessage/Item construction is also LINQ friendly

```cs
using static Secs4Net.Item;

var s16f15 = 
    new SecsMessage(16, 15, "CreateProcessJob"            
        L(
            U4(0),
            L(from pj in tx.ProcessJobs select
                L(
                    A(pj.Id),
                    B(0x0D),
                    L(from carrier in pj.Carriers select
                        L(
                            A(carrier.Id),
                            L(from slotInfo in carrier.SlotMap select
                                U1(slotInfo.SlotNo)))),
                            L(
                                U1(1),
                                A(pj.RecipeId),
                                L()),
                            Boolean(true),
                            L()))));
```

4\. Item is mutable with restrict.
    you can only overwrite existing memory. the `Item.Count` is fix when created.
    string Item can't be changed, coz .net `string` is immutable as well.
