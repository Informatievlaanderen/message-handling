using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple.Tests
{
    public class SqsJsonMessageTests
    {
        [Fact]
        public void CreateAndMap()
        {
            var serializer = new JsonSerializer();

            var message = new DummyMessage
            {
                Name = "abc"
            };

            var sqsJsonMessage = SqsJsonMessage.Create(message, serializer);
            var actual = Assert.IsType<DummyMessage>(sqsJsonMessage.Map(serializer));

            Assert.Equal(message.Name, actual.Name);
        }

        private class DummyMessage
        {
            public string Name { get; set; }
        }
    }
}
