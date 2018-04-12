using System;

namespace SQL_CRM
{
    public class WebMessage
    {
        private readonly string _sender;
        private readonly string _message;
        public ConsoleColor? Color { get; private set; }

        public string From => _sender;
        public string Message => _message;

        public WebMessage(string message)
            : this(null, message, null)
        {
        }
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
            return !string.IsNullOrWhiteSpace(_sender) ? $"{_sender}: {_message}" : _message;
        }
    }
}