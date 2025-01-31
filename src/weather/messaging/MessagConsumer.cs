using MassTransit;

namespace messaging
{
    public class Message
    {
        public string Text { get; set; }
    }

    public class MessageConsumer :
        IConsumer<Message>
    {
        readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation("Message Consumer Received Text: {Text}", context.Message.Text);

            return Task.CompletedTask;
        }
    }
}