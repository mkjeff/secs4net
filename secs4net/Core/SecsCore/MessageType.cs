namespace Secs4Net {
    enum MessageType : byte {
        DataMessage = 0,	    // 00000000
        SelectRequest = 1,		// 00000001	ReplyExpected
        SelectResponse = 2,		// 00000010
        //Deselect_req = 3,	    // 00000011	ReplyExpected
        //Deselect_rsp = 4,	    // 00000100
        LinkTestRequest = 5,	// 00000101	ReplyExpected
        LinkTestResponse = 6,	// 00000110
        //Reject_req = 7,	    // 00000111
        SeperateRequest = 9	    // 00001001
    }
}