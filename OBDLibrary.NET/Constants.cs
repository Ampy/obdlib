namespace OBD2.Library
{
	internal static class Constants
    {
        #region "Serial port related"
        public const int SLEEP_TIME_IN_MS = 100;
        public const BaudRate SERIAL_PORT_DEFAULT_BAUD_RATE = BaudRate.B38400;
		public const int SERIAL_PORT_BUFFER_SIZE = 1024;
	    public const int SERIAL_PORT_READ_TIMEOUT_IN_MS = 1000;
        public const int SERIAL_PORT_WRITE_TIMEOUT_IN_MS = 3000;
	    public const char SERIAL_PORT_READY_PROMPT_CHAR = '>';
	    public const char SERIAL_PORT_MESSAGE_TERMINATOR_CHAR = '\r';
	    public const int SERIAL_PORT_RECEIVE_TIMEOUT_IN_MS = 10000;
        #endregion

        public const string NO_DATA = "NO DATA";
        public static readonly string[] StringsToRemove = new[] { "\n", "\r>" };
        public const string SPLIT_VALUE_CHAR = " ";
	    public const string HEX_FORMAT = "X2";
    }
}

