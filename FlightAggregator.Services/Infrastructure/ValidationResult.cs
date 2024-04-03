namespace FlightAggregator.Services.Infrastructure
{
    public class ValidationResult
    {
        private readonly IList<string> _messages = new List<string>();
        public IEnumerable<string> Messages => _messages;
        public bool Success => !_messages.Any();

        public ValidationResult() { }

        public ValidationResult(string message)
        {
            Add(message);
        }

        public ValidationResult(string[] messages)
        {
            foreach (var message in messages)
            {
                Add(message);
            }
        }

        public void Add(string message)
        {
            _messages.Add(message);
        }
    }

    public class ValidationResult<T> : ValidationResult
    {
        public T Value { get; }

        public ValidationResult(T value)
        {
            Value = value;
        }

        public ValidationResult(string message, T value) : base(message)
        {
            Value = value;
        }

        public ValidationResult(string[] messages, T value) : base(messages)
        {
            Value = value;
        }
    }
}
