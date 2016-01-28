namespace Secs4Net {
    enum MessageType : byte {
        Data_Message = 0,	/// 00000000
        Select_req = 1,		/// 00000001	ReplyExpected
        Select_rsp = 2,		/// 00000010
        //Deselect_req = 3,	/// 00000011	ReplyExpected
        //Deselect_rsp = 4,	/// 00000100
        Linktest_req = 5,	/// 00000101	ReplyExpected
        Linktest_rsp = 6,	/// 00000110
        //Reject_req = 7,	/// 00000111
        Seperate_req = 9	/// 00001001
    }
}