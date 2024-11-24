using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TimerTrigger
{
    public static class TimerTriggerSend
    {
        [Function("TimerTriggerSend")]
        public static async Task Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer)
        {
        
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            string queueName = "product-queue";

            if (string.IsNullOrEmpty(connectionString)) return;

           
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            if (await queueClient.ExistsAsync())
            { 
                var product = $"Product-{Guid.NewGuid()}";
                await queueClient.SendMessageAsync(product);
                
            }
            
        }
    }
}
