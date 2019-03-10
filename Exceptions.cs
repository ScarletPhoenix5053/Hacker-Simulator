using System;

namespace Sierra.AGPW.HackerSim
{
    public class DuplicateKeyException : Exception
    {
            
        public DuplicateKeyException() : base() { }
        public DuplicateKeyException(string message) : base(message) { }
        public DuplicateKeyException(string message, System.Exception inner) : base(message, inner) { }

    }    
    public class ScenarioNotFoundException : Exception
    {
            
        public ScenarioNotFoundException() : base() { }
        public ScenarioNotFoundException(string message) : base(message) { }
        public ScenarioNotFoundException(string message, System.Exception inner) : base(message, inner) { }

    }
}