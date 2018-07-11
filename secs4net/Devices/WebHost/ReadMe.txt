This sample program is the demo of Secs4Net.You can simulate a device/host to communicate with other HSMS-SS/SECS-II device.
Try to send/reply message with following SML format, notice the end of angle brackets of the list item.(or refer to received message textbox)
This just a simple demo,expand it by yourself for fun.

S6F11ReadyToLoad: 'S6F11' W 
    <L [3]
        <U4 [1] 320 >
        <U2 [1] 114 > 
        <L [1]
            <L [2]
                <U2 [1] 500 >
                <L [1]
                    <U1 [1] 1 > 
                 >
             >
         >
     >
.

S6F12: 'S6F12'
    <B [1] 0x00 > 
.