
using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TimerTrigger
{
    public static class TimerTriggerReceive
    {
        [Function("TimerTriggerReceive")]
        public static async Task Run(
            [TimerTrigger("*/10 * * * * *")] TimerInfo myTimer
            )
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            string queueName = "product-queue";


            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (await queueClient.ExistsAsync())
            {
                var message = await queueClient.ReceiveMessageAsync();

                if (message.Value != null)
                    await queueClient.DeleteMessageAsync(message.Value.MessageId, message.Value.PopReceipt);
            }

        }
    }
}

