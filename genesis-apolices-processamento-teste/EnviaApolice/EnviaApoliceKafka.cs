using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace genesis_apolices_processamento_teste.EnviaApolice
{
    public class EnviaApoliceKafka : IEnviaApolice
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaConfig _kafkaConfig;

        public EnviaApoliceKafka(IOptions<KafkaConfig> options)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = options.Value.BootstrapServer,
                // Other configuration settings...
            };
            _kafkaConfig = options.Value;
            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        public void ProduceMessage(string topic, string key, string value)
        {
            var message = new Message<string, string> { Key = key, Value = value };
            _producer.Produce(_kafkaConfig.ApoliceNormalizadaTopic, message);
        }
    }
}
