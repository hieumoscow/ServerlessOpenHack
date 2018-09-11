

using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;

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
        //[FunctionName("GetRatingsByUserID")]
        public static void Run([CosmosDBTrigger(databaseName: "ToDoItems",
            collectionName: "Items",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents,
             TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                log.Info($"Documents modified: {documents.Count}");
                log.Info($"First document Id: {documents[0].Id}");
            }
        }
    }
    public class RatingClass
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string LocationName { get; set; }
        public string Rating { get; set; }
        public string UserNotes { get; set; }
        public string Timestamp { get; set; }
    }
}
