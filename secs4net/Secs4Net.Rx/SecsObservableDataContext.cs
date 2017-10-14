using System;
using System.Reactive.Linq;

namespace Secs4Net.Rx
{
    public class SecsObservableDataContext
    {
        private readonly SecsGem _secsGem;
        protected readonly IObservable<PrimaryMessageWrapper> MessageEvents;

        public SecsObservableDataContext(SecsGem secsGem)
        {
            _secsGem = secsGem;
            MessageEvents = Observable.FromEvent<EventHandler<PrimaryMessageWrapper>, PrimaryMessageWrapper>(
                h => _secsGem.PrimaryMessageReceived += h,
                h => _secsGem.PrimaryMessageReceived -= h);
        }

        public IObservable<PrimaryMessageWrapper> Match(SecsMessage pattern) 
            => MessageEvents.Where(a => a.Message.IsMatch(pattern));
    }
}
