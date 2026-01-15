namespace MedAppointment.Logics.Patterns.ResultPattern
{
    public class Message
    {
        public Message(string textCode, string message)
        {
            TextCode = textCode;
            Text = message;
            Exception = null;
        }
        public Message(string textCode, string message, Exception exception)
        {
            TextCode = textCode;
            Text = message;
            Exception = exception;
        }

        public string TextCode { get; set; }
        public string Text { get; set; }
        public Exception? Exception { get; set; }
    }
}
