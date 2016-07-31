# secs4net

**Project Description**  
SECS-II/HSMS-SS/GEM implementation on .NET. This library provide easy way to communicate with SEMI standard compatible device.  

1\. Send message to device

<div style="color: black; background-color: white;">

<pre><span style="color: blue;">try</span>{
    <span style="color: blue;">var</span> s3f18 = await device.SendAsync(s3f17); <span style="color: green;">//await device's reply secondary message</span>
    <span style="color: green;">//access item value with strong type</span>
    <span style="color: blue;">byte</span> returnCode = (<span style="color: blue;">byte</span>)s3f18.SecsItem.Items[0]; <span style="color: green;">// access item value. Equal to s3f18.SecsItem.Items[0].Value<byte>()</span>
}<span style="color: blue;">catch</span>(SecsException){
    <span style="color: green;">// exception  when</span>
    <span style="color: green;">// T3 timeout</span>
    <span style="color: green;">// device reply SxF0</span>
    <span style="color: green;">// device reply S9Fx</span>
}
</pre>

</div>

2\. Receive message from device  
provide a delegate to SecsGem constructor's last argument

<div style="color: black; background-color: white;">

<pre>(primaryMsg, reply) => {
    <span style="color: blue;">try</span> {
       <span style="color: green;">//do something for primaryMsg</span>
       reply( secondaryMsg );  <span style="color: green;">// reply secondary msg to device</span>
    } <span style="color: blue;">catch</span> (Exception ex) {

    }
};
</pre>

</div>

3\. SecsMessage construction is LINQ friendly

<div style="color: black; background-color: white;">

<pre><span style="color: blue;">var</span> s16f15 = <span style="color: blue;">new</span> SecsMessage(16, 15, <span style="color: #a31515;">"CreateProcessJob"</span>,
                Item.L(
                    Item.U4(0),
                    Item.L(<span style="color: blue;">from</span> pj <span style="color: blue;">in</span> tx.ProcessJobs <span style="color: blue;">select</span>
                        Item.L(
                            Item.A(pj.Id),
                            Item.B(0x0D),
                            Item.L(<span style="color: blue;">from</span> carrier <span style="color: blue;">in</span> pj.Carriers <span style="color: blue;">select</span>
                                Item.L(
                                    Item.A(carrier.Id),
                                    Item.L(<span style="color: blue;">from</span> slotInfo <span style="color: blue;">in</span> carrier.SlotMap <span style="color: blue;">select</span>
                                        Item.U1(slotInfo.SlotNo)))),
                            Item.L(
                                Item.U1(1),
                                Item.A(pj.RecipeId),
                                Item.L()),
                            Item.Boolean(<span style="color: blue;">true</span>),
                            Item.L()))));
</pre>

</div>

4\. SecsMessage/Item is immutable.  
5\. SecsMessage/Item is .NET Remoting operatable. It mean you can build an scalable distributed device control system.


[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/mkjeff/secs4net/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

