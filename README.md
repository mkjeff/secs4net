# secs4net

### Note: This project is just an implementation reference. 

About the v2, it is almost done. but it is hard to be compatible with v1.

.NET Core/.NET 5.0+ is the mainstream already, opposite .net framework stuck on the maintenance stage.

As mention above, as an implementation reference, I would try to keep the source code is clean and easy to understanding. Product support is not my goal coz I'm not in this industry for a long time.

If you already use secs4net in your product and can't migrate your system to `.NET 6.0`. I'd recommend using the fork version from [@TiltonJH](https://github.com/TiltonJH/secs4net).

[![NuGet](https://img.shields.io/nuget/v/secs4net.svg)](https://www.nuget.org/packages/secs4net/)

**Project Description**  
Note: Secs4net will only support .net6.0+ starting from v2.

SECS-II/HSMS-SS/GEM implementation on .NET. This library provide easy way to communicate with SEMI standard compatible device.  

1. Send message to device
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
        s3f17.SecsItem[1][0][2].Take(1..2); // LINQ Take with range syntax
    
        //access item value
        byte b2 = s3f17.SecsItem[0].FirstValue<byte>(); // with different type
        s3f17.SecsItem[0].FirstValue<byte>() = 0; // change original value 
        s3f17.SecsItem[0].GetFirstValueOrDefault<byte>(fallbackValueWhenItemIsEmpty); 
        string str = s3f17.SecsItem[1][0][0].GetString(); // str = "Id"

        //await secondary message
        var s3f18 = await secsGem.SendAsync(s3f17); 

        // LINQ query
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
2. Handle primary messages
    ```cs
    await foreach (var e in secsGem.GetPrimaryMessageAsync(cancellationToken))
    {
        try 
        {        
            using var primaryMsg = e.PrimaryMessage;
            //do something for primary message

            // reply secondary message to device
            using var secondaryMsg = ...
            await e.TryReplyAsync(secondaryMsg); 
        }
        catch (Exception ex) 
        {

        }
    };
    ```

3. Item construction is LINQ friendly

    ```cs
    using static Secs4Net.Item;

    var s16f15 = 
        new SecsMessage(16, 15)
        {
            Name = "CreateProcessJob",
            SecsItem = L(
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

4. Item is mutable with restrict.
    > Basic rule: The `Item.Count` has been fixed already while the item is created.
    
    That means you can only overwrite values on allocated memory. 
    e.q. A List item can override a sub-item via index operator but can't add/remove the sub-item. String Item still immutable, coz C# `string` is immutable as well.

5. Reuse pooled array for large item values

    All unmanaged data Item can created from `IMemoryOwner<T>`.

    The following sample uses the implementation of `IMemoryOwner<T>` from [`Microsoft.Toolkit.HighPerformance`](https://docs.microsoft.com/en-us/windows/communitytoolkit/high-performance/memoryowner) that has been referenced internally by secs4net..
    ```cs
    using var largeArrayOwner = MemoryOwner<int>.Allocate(size: 65535);
    
    // feed the value into largeArrayOwner.Memory or largeArrayOwner.Span
    FillLargeArray(largeArrayOwner.Memory);

    using var s6f11 = new SecsMessage(6, 11, replyExpected: false)
    {
        Name = "LargeDataEvent",
        SecsItem = L(
            L(
                I(1121),
                A(""),
                I4(largeArrayOwner))), // create Item from largeArrayOwner
    };

    // apply using on received message as well. coz the item that decoded by PipeDecoder also using MemoryOwner<T> when the data array is big.
    using var s6f12 = await secsGem.SendAsync(s6f11);
   
    ```
    > `IMemoryOwner<T>`, `Item` and `SecsMessage` have implemented `IDisposable` don't forget to Dispose it when they don't need anymore.
    Otherwise, the array will not return to the pool till GC collects.