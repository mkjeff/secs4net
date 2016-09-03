# secs4net

**Project Description**  
SECS-II/HSMS-SS/GEM implementation on .NET. This library provide easy way to communicate with SEMI standard compatible device.  

1\. Send message to device
```cs
try{
    var s3f18 = await device.SendAsync(s3f17); //await device's reply secondary message
    //access item value with strong type
    byte returnCode = (byte)s3f18.SecsItem.Items[0]; // access item value. Equal to s3f18.SecsItem.Items[0].Value()
}catch(SecsException){
    // exception  when
    // T3 timeout
    // device reply SxF0
    // device reply S9Fx
}
```
2\. Handle primary message from device, provide a delegate to SecsGem constructor
```cs
(primaryMsg, reply) => {
    try {
       //do something for primaryMsg
       reply( secondaryMsg );  // reply secondary msg to device
    } catch (Exception ex) {

    }
};
```

3\. SecsMessage construction is LINQ friendly

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
