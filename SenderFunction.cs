using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public static class SenderFunction
{
    [FunctionName("Sender")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [CosmosDB(
            databaseName: "myLRS",
            collectionName: "UnityData",
            ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
        ILogger log)
    {
        log.LogInformation("Sending...");
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        Document document = new Document(data);
        await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("myLRS", "UnityData"), document);

        log.LogInformation("Sent.");
        return new OkResult();
    }
}
