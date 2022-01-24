namespace SampleSimpleKafkaMessages
{
    using System;

    public class SomeSimpleMessage
    {
        public DateTime DateTime { get; set; }
        public string Message { get; set; }

        public SomeSimpleMessage(string message)
        {
            DateTime = DateTime.Now;
            Message = message;
        }

        public override string ToString() => $"{nameof(SomeSimpleMessage)}: [ {nameof(DateTime)}={DateTime}, {nameof(Message)}={Message} ]";
    }
}
