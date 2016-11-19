using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Secs4Net.Rx
{
    public class SecsObservableDataContext
    {
        readonly SecsGem _secsGem;
        protected readonly IObservable<PrimaryMessageWrapper> MessageEvents;

        public SecsObservableDataContext(SecsGem secsGem)
        {
            _secsGem = secsGem;
            MessageEvents = Observable.FromEvent<Action<PrimaryMessageWrapper>, PrimaryMessageWrapper>(
                h => _secsGem.PrimaryMessageReceived += h,
                h => _secsGem.PrimaryMessageReceived -= h);
        }

        public IObservable<PrimaryMessageWrapper> Match(SecsMessage pattern) 
            => MessageEvents.Where(a => a.Message.IsMatch(pattern));
    }
}
