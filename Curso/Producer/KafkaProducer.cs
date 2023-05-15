using Confluent.Kafka;

namespace Curso.Producer
{
    public class KafkaProducer : IProducer
    {
        private readonly ProducerConfig _config;
        
        //public KafkaProducer(IOptions<KafkaOptions> options, ILogger logger)
        public KafkaProducer()
        {
            _config = new ProducerConfig {BootstrapServers = "10.13.19.96:29092" };

        }

        public void ProduceMessage(string topic, string message)
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            try
            {
                var deliveryReport = producer.ProduceAsync(topic, new Message<Null, string> { Value = message }).GetAwaiter().GetResult();

                Console.WriteLine($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'");
                producer.Flush();
            }
            catch (ProduceException<Null, string> e)
            {
                throw;
            }
        }
    }
}
