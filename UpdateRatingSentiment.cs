
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using System.Text;
using Microsoft.Azure.Documents.Client;

namespace Oteam15.Function
{
    public static class UpdateRatingSentiment
    {
        private static readonly HttpClient httpClient;
        static UpdateRatingSentiment()
        {
            httpClient = new HttpClient();
        }
        // [FunctionName("UpdateRatingSentiment")]
        public static async Task RunAsync([CosmosDBTrigger(
            databaseName: "Ratings",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection")]IReadOnlyList<Document> documents, 
            [CosmosDB(
                databaseName: "Ratings",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient documentClient,


            ILogger log)

        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var s = new List<SentimentReqItem>();// = documents.ToDictionary(<SentimentInput>(x=> new SentimentInput {text = x.userNote});
            var ratings =  new List<Rating>();
            for (int i = 0;i< documents.Count;i++){
                ratings.Add(JsonConvert.DeserializeObject<Rating>(documents[i].ToString()));
                s.Add( new SentimentReqItem(){id = documents[i].SelfLink, text =ratings[i].userNotes});
            }
            var sd = new SentimentReq(){documents = s};
            var ret = await GetRatingScore(sd);
                var dburi = UriFactory.CreateDocumentCollectionUri("Ratings", "ratings");

            for (int i = 0;i< ret.documents.Count;i++){
                ratings[i].sentimentScore = ret.documents[i].score;
                var re = await documentClient.UpsertDocumentAsync(dburi,ratings[i]);
            }
        }

        private static async Task<SentimentResp> GetRatingScore(SentimentReq sd){
            //https://southeastasia.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment
            var taUrl = "https://southeastasia.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
            var postData = JsonConvert.SerializeObject(sd);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, taUrl);
            request.Content = new StringContent(postData,
                                                Encoding.UTF8, 
                                                "application/json");
            request.Content.Headers.Add("Ocp-Apim-Subscription-Key","987bb1fbe0e54280b15341cfa8d45f26");

            var result = await httpClient.SendAsync(request);
            var ret = await result.Content.ReadAsAsync<SentimentResp>();
            
            return ret;
        }
    }

    public class SentimentReqItem{
        
        public string id {get;set;}
        public string text {get;set;}
    }
    public class SentimentReq{
        public List<SentimentReqItem> documents {get;set;}
    }

     public class SentimentRespItem
    {
        public double score { get; set; }
        public string id { get; set; }
    }

    public class SentimentResp
    {
        public List<SentimentRespItem> documents { get; set; }
        public List<object> errors { get; set; }
    }
}
