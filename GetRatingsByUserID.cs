

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Oteam15;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessOpenHack
{
    public static class GetRatingsByUserID
    {
        //[FunctionName("GetRatingsByUserID")]
        //public static List<RatingClass> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    List<RatingClass> ratingList = new List<RatingClass>();
        //    string userId = req.Query["userId"];

        //    string requestBody = new StreamReader(req.Body).ReadToEnd();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    userId = userId ?? data?.userId;

        //    return ratingList;
        //    //return userId != null
        //    //    ? (ActionResult)new OkObjectResult($"Hello, {userId}")
        //    //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        //}
       
        //public static void Run(  [CosmosDBTrigger(databaseName: "Ratings",
        //    collectionName: "ratings",
        //    ConnectionStringSetting = "CosmosDBConnection",
        //    LeaseCollectionName = "leases",
        //    CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents,
        //    TraceWriter log)
        //{
        //    log.Info("Hi");
        //    if (documents != null && documents.Count > 0)
        //    {
        //        log.Info($"Documents modified: {documents.Count}");
        //        log.Info($"First document Id: {documents[0].Id}");
        //    }
        //}
        [FunctionName("GetRatingsByUserID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",
                Route = "GetRatingsByUserID/{id}")]HttpRequest req,
            [CosmosDB("ToDoItems", "Items",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "select * from Ratings r where r.userId = {id}")]
                IEnumerable<Rating> toDoItems,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            foreach (Rating toDoItem in toDoItems)
            {
                log.Info(toDoItem.userId.ToString());
            }
            return new OkResult();
        }
    }

}
