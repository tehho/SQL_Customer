using System;

namespace SQL_CRM
{
    public class WebMessage
    {
        private string _sender;
        private string _message;
        public ConsoleColor? Color { get; private set; }

        public string From => _sender;
        public string Message => _message;

        public WebMessage(string sender, string message)
         : this(sender, message, null)
        {
        }

        public WebMessage(string sender, string message, ConsoleColor? color)
        {
            _sender = sender;
            _message = message;
            Color = color;
        }

        public override string ToString()
        {
            return $"{_sender}: {_message}";
        }
    }
}