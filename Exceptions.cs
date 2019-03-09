using System;

namespace Sierra.AGPW.HackerSim
{
    public class DuplicateKeyException : Exception
    {
            
        public DuplicateKeyException() : base() { }
        public DuplicateKeyException(string message) : base(message) { }
        public DuplicateKeyException(string message, System.Exception inner) : base(message, inner) { }

    }
}