
using Secs4Net;
using System.Collections.Generic;

namespace SecsMessageVisuallizer.ViewModel {
    public class SecsMessageCollectionViewModel {
        readonly SecsMessageList _msgList;

        public SecsMessageCollectionViewModel(SecsMessageList secsMsgList)
        {
            _msgList = secsMsgList;
        }

        public IEnumerable<SecsMessageViewModel> SecsMessages
        {
            get
            {
                foreach (SecsMessage msg in _msgList)
                    yield return new SecsMessageViewModel(msg);
            }
        }
    }
}
