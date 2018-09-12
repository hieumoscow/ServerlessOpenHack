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

        [FunctionName("GetRatingsByUserID")]
        public static IEnumerable<Rating> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",
                Route = "GetRatingsByUserID/{id}")]HttpRequest req,
            [CosmosDB("Ratings", "ratings",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "select * from Ratings r where r.userId = {id}")]
                IEnumerable<Rating> RatingClassList,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");


            //foreach (Rating toDoItem in toDoItems)
            //{
            //    log.Info(toDoItem.userId.ToString());
            //}
            return RatingClassList;
        }
    }

}
