
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.EventHubs;
using System.Text;

namespace Oteam15.Function
{
    public static class CreateSales
    {
        [FunctionName("CreateSales")]
        // public static async Task RunAsync([EventHubTrigger("ot15eh", Connection = "EventHubConnection")] EventData myEventHubMessage, 
        // DateTime enqueuedTimeUtc, 
        // Int64 sequenceNumber,
        // string offset,
        public static async Task RunAsync([EventHubTrigger("ot15eh", Connection = "EventHubConnection")] string[] myEventHubMessages, 
        [CosmosDB(
                databaseName: "Ratings",
                collectionName: "sales",
                ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<string> documentsToStore,
         TraceWriter log)
        {
            //{"header":{"salesNumber":"86620caf-d74f-98c5-e842-7b041616e430","dateTime":"2018-09-12 14:33:36","locationId":"III999","locationName":"Fourth Coffee","locationAddress":"32108 18th Street","locationPostcode":"98112","totalCost":"11.97","totalTax":"1.20"},"details":[{"productId":"551a9be9-7f1c-447d-83ee-b18f5a6fb018","quantity":"3","unitCost":"3.99","totalCost":"11.97","totalTax":"1.20","productName":"Matcha Green Tea","productDescription":"Green tea ice cream is good for you because it is green."}]}
            //var eventStr = Encoding.UTF8.GetString(myEventHubMessage.Body.Array);
            // log.Info($"Event: {eventStr}");
            // // Metadata accessed by binding to EventData
            // log.Info($"EnqueuedTimeUtc={myEventHubMessage.SystemProperties.EnqueuedTimeUtc}");
            // log.Info($"SequenceNumber={myEventHubMessage.SystemProperties.SequenceNumber}");
            // log.Info($"Offset={myEventHubMessage.SystemProperties.Offset}");
            // // Metadata accessed by using binding expressions
            // log.Info($"EnqueuedTimeUtc={enqueuedTimeUtc}");
            // log.Info($"SequenceNumber={sequenceNumber}");
            // log.Info($"Offset={offset}");
            foreach (var message in myEventHubMessages)
                await documentsToStore.AddAsync(message);

        }
    }


    /*
{
    "userId": "cc20a6fb-a91f-4192-874d-132493685376",
    "productId": "4c25613a-a3c2-4ef3-8e02-9c335eb23204",
    "locationName": "Sample ice cream shop",
    "rating": 5,
    "userNotes": "I love the subtle notes of orange in this ice cream!"
}
     */
}
