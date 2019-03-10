using System;
using System.Threading;

namespace Sierra.AGPW.HackerSim
{
    public class Scribe
    {
        #region Vars
        private const int _defaultPauseDuration = 1000;

        private int _characterWriteSpeedNormal;
        private int _characterWriteSpeedSlow;
        private int _characterWriteSpeedFast;
        #endregion
        #region Constructor
        public Scribe()
        {
            _characterWriteSpeedSlow = 500;
            _characterWriteSpeedNormal = 50;
            _characterWriteSpeedFast = 10;
        }
        public Scribe(ScribeWriteSpeedTemplate writeSpeed)
        {
            _characterWriteSpeedSlow = writeSpeed.Slow;
            _characterWriteSpeedNormal = writeSpeed.Normal;
            _characterWriteSpeedFast = writeSpeed.Fast;
        }
        #endregion
        #region Methods
        #region Write Methods
        public void Write(string message)
        {
            LogToConsole(message, _characterWriteSpeedNormal);
        }
        public void WriteLine(string message)
        {
            Write(message);
            Console.WriteLine();
        }
        public void WriteSlow(string message)
        {
            LogToConsole(message, _characterWriteSpeedSlow);
        }
        public void WriteLineSlow(string message)
        {
            WriteSlow(message);
            Console.WriteLine();
        }
        public void WriteFast(string message)
        {
            LogToConsole(message, _characterWriteSpeedFast);
        }
        public void WriteLineFast(string message)
        {
            WriteFast(message);
            Console.WriteLine();
        }
        public void WriteNewLine()
        {
            Console.WriteLine();
        }
        private void LogToConsole(string message, int charDelay)
        {
            var chars = message.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                Console.Write(chars[i]);
                Thread.Sleep(charDelay);
            }
        }
        #endregion
        #region Pause Methods
        public void Pause()
        {
            Pause(_defaultPauseDuration);
        }
        public void Pause(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
        #endregion
        #region Read Methods
        public string ReadLine()
        {
            WriteFast(" > ");
            return Console.ReadLine();
        }
        #endregion
        #endregion
    }
    public struct ScribeWriteSpeedTemplate
    {        
        public int Slow { get; private set;}
        public int Normal { get; private set;}
        public int Fast { get; private set;}

        public ScribeWriteSpeedTemplate(int slowSpeed, int normalSpeed, int fastSpeed)
        {
            Slow = slowSpeed;
            Normal = normalSpeed;
            Fast = fastSpeed;
        }
    }
}