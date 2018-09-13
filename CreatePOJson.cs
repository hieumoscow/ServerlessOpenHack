
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CsvHelper;
using System.Linq;
using System.Net.Http;
using ServerlessOpenHack.models;
using System.Collections.Generic;

namespace Oteam15.Function
{
    public static class CreatePOJson
    {

        /*
        
        20180912063200

        [
  {
    "messageId": "18de70c9-55e0-4893-8e5a-a935d921bd3c",
    "content": "20180912063200-OrderLineItems.csv"
  },
  {
    "messageId": "618e2165-e5c8-42c1-afc6-924bb63c3ad8",
    "content": "20180912063200-ProductInformation.csv"
  },
  {
    "messageId": "4b34c161-3e95-457d-96fa-838a647a3783",
    "content": "20180912063200-OrderHeaderDetails.csv"
  }
]
         */
         
        private static readonly HttpClient httpClient;
        static CreatePOJson()
        {
            httpClient = new HttpClient();
        }

        // [FunctionName("CreatePOJson")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "CreatePOJson/{id}")]HttpRequest req,
              [Blob("data/{id}-OrderHeaderDetails.csv", FileAccess.Read, Connection = "AzureStorage6")] TextReader OrderDetailsReader,
              [Blob("data/{id}-OrderLineItems.csv", FileAccess.Read, Connection = "AzureStorage6")] TextReader OrderItemsReader,
              [Blob("data/{id}-ProductInformation.csv", FileAccess.Read, Connection = "AzureStorage6")] TextReader ProductInfoReader,
              //OrderLineItems, ProductInformation
              [CosmosDB(
                databaseName: "Ratings",
                collectionName: "orders",
                ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<OrderDetails> documentsToStore, ILogger log)
        {

            // https://oteam15storage6.blob.core.windows.net/data/20180912053600-OrderHeaderDetails.csv?sp=rl&st=2018-09-12T06:21:08Z&se=2018-09-13T06:21:08Z&sv=2017-11-09&sig=ptFbYvkqH0tB1xxP71KsXz9nH7XcksfIW6Tt8OCIQMU%3D&sr=b

            var orderDetailsJson = Convert(OrderDetailsReader);
            var orderItemsJson = Convert(OrderItemsReader);
            var productInfoJson = Convert(ProductInfoReader);

            List<OrderDetails> orderDetailsList = JsonConvert.DeserializeObject<List<OrderDetails>>(orderDetailsJson);
            List<OrderItems> orderItemsList = JsonConvert.DeserializeObject<List<OrderItems>>(orderItemsJson);
            List<ProductInfo> productInfoList = JsonConvert.DeserializeObject<List<ProductInfo>>(productInfoJson);
            List<string> retStrs = new List<string>();
            foreach (var item in orderDetailsList)
            {
                item.orderItemList = orderItemsList.FindAll(x => {
                    var ret = x.ponumber.Equals(item.ponumber);
                    if (ret)
                    {
                        var productInfo = productInfoList.First(y => y.productid.Equals(x.productid));
                        x.productname = productInfo.productname;
                        x.productdescription = productInfo.productdescription;
                    }
                    return ret;
                });

                await documentsToStore.AddAsync(item);
                retStrs.Add(item.ponumber);
            }


            return (ActionResult)new OkObjectResult(retStrs);

        }

        public static string Convert(TextReader sReader)
        {
            // var sReader = new StreamReader(blob);
            var csv = new CsvReader(sReader);

            csv.Read();
            csv.ReadHeader();

            var csvRecords = csv.GetRecords<object>().ToList();

            return JsonConvert.SerializeObject(csvRecords);
        }
    }
}
