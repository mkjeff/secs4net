using System;

namespace Cim.Eap {
    public sealed class ScenarioException : ApplicationException {
        public ScenarioException(string message, Exception innerException)
            : base(message, innerException) { }

        public ScenarioException(string message)
            : base(message) { }
    }
}
