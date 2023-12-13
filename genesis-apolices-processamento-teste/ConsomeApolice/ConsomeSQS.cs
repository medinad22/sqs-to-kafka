
using genesis_apolices_processamento_teste.ProcessaApolice;
using System.Text.Json;
using System;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Amazon.Runtime;
using Amazon;

namespace genesis_apolices_processamento_teste.ConsomeArquivo
{
    public class ConsomeSQS : BackgroundService
    {

        private readonly IProcessaApolice _processaApolice;
        private readonly SqsConfig _sqsConfig;
        public ConsomeSQS(IProcessaApolice processaApolice, IOptions<SqsConfig> sqsOptions)
        {
            _processaApolice = processaApolice;
            _sqsConfig = sqsOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var awsCreds = new BasicAWSCredentials(_sqsConfig.AccessKey, _sqsConfig.SecretKey);
            var config = new AmazonSQSConfig
            {
                ServiceURL = _sqsConfig.ServiceUrl
            };
            var amazonSqsClient = new AmazonSQSClient(awsCreds, config);

            while (!stoppingToken.IsCancellationRequested)
            {
                var request = new ReceiveMessageRequest()
                {
                    QueueUrl = _sqsConfig.ApoliceNormalizadaSQS,
                };
                var response = await amazonSqsClient.ReceiveMessageAsync(request);

                foreach (var message in response.Messages)
                {
                    if (ProcessMessage(message))
                    {
                        await DeleteMessage(amazonSqsClient, message, request.QueueUrl);
                    }

                }
            }
            
        }

        private bool ProcessMessage(Message message)
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    IncludeFields = true,
                };

                if (message.Body != null)
                {
                    _processaApolice.ProcessaApoliceNormalizada();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        private static async Task DeleteMessage(
          IAmazonSQS sqsClient, Message message, string qUrl)
        {
            Console.WriteLine($"\nDeleting message {message.MessageId} from queue...");

            await sqsClient.DeleteMessageAsync(qUrl, message.ReceiptHandle);
        }
    }
}
