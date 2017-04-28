# secs4net

[![NuGet](https://img.shields.io/nuget/v/secs4net.svg)](https://www.nuget.org/packages/secs4net/)

**Project Description**  
SECS-II/HSMS-SS/GEM implementation on .NET. This library provide easy way to communicate with SEMI standard compatible device.  

1\. Send message to device
```cs
try
{
    //await secondary message
    var s3f18 = await device.SendAsync(s3f17); 
    
    //access item value
    byte b1 = (byte)s3f18.SecsItem.Items[0]; 
    byte b2 = s3f18.SecsItem.Items[0].GetValue<byte>();
    string str = s3f18.SecsItem.Items[0].GetValue<string>();

    // LINQ query
    var query =
        from a in s3f18.SecsItem.Items[3].Items
        select new {
            num = a.GetValue<int>(),
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
2\. Handle primary message from device
```cs
secsGem.PrimaryMessageReceived += async (sender, messageWrapper) => 
{
    try 
    {
        //do something for primaryMsg
        var primaryMsg = messageWrapper.Message;
	   

        // reply secondary msg to device
        await messageWrapper.ReplyAsync( secondaryMsg ); 
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

4\. SecsMessage/Item is immutable(API level).  
