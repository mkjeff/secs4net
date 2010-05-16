using System;

namespace Secs4Net {
    sealed class Lazy<T> where T : class {
        readonly Func<T> creator;
        T _value;
        public Lazy(Func<T> func) {
            this.creator = func;
        }
        public Lazy(T value) {
            _value = value;
        }
        public T Value { get { return this._value ?? (_value = creator()); } }
    }

    static class Lazy {
        public static Lazy<T> Create<T>(Func<T> creator) where T : class {
            return new Lazy<T>(creator);
        }

        public static Lazy<T> Create<T>(T value) where T : class {
            return new Lazy<T>(value);
        }
    }
}
