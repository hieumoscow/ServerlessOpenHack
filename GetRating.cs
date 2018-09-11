
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Oteam15.Function
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratings/{id:guid}")]HttpRequest req,
            [CosmosDB(
                databaseName: "Ratings",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection",
                PartitionKey = "{id}",
                Id = "{id}")]
                //SqlQuery = "select * from Ratings r where r.id = {id}")]
                Rating rating,
                TraceWriter log)
        {
            return (ActionResult)new OkObjectResult(rating);
        }
    }
}
