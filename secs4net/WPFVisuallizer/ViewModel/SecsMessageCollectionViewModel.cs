
using Secs4Net;
using System.Collections.Generic;
using System.Linq;

namespace SecsMessageVisuallizer.ViewModel {
    public class SecsMessageCollectionViewModel {
        readonly SecsMessageList _msgList;

        public SecsMessageCollectionViewModel(SecsMessageList secsMsgList)
        {
            _msgList = secsMsgList;
        }

        public IEnumerable<SecsMessageViewModel> SecsMessages =>
                _msgList.Select(msg => new SecsMessageViewModel(msg));
    }
}
