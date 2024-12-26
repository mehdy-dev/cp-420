using Microsoft.Azure.Cosmos.Fluent;
using System;
using Azure.Identity;
using Azure.Core;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Reflection.Metadata;

namespace cosmodb01
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            CosmosClientBuilder builder = new CosmosClientBuilder("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
                .WithConsistencyLevel(ConsistencyLevel.Eventual);

            builder.AddCustomHandlers(new LogHandler());

            CosmosClient client = builder.Build();

            Database result = await client.CreateDatabaseIfNotExistsAsync("cosmicworks");

            Container container = await result.CreateContainerIfNotExistsAsync("products", "/categoryId", 400);

            try
            {

                var items = MockDocs.GetMockDocs();

               foreach (var item in items)
                {
                    if (item != null)
                        await container.CreateItemAsync<Product>(item);
                }

            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                // Add logic to handle conflicting ids
            }
            catch (CosmosException ex)
            {
                // Add general exception handling logic
            }



            string id = "027D0B9A-F9D9-4C96-8213-C8546C4AAE71";
            string categoryId = "26C74104-40BC-4541-8EF5-9892F7F03D72";
            PartitionKey partitionKey = new(categoryId);

            ItemResponse<Product> response = await container.ReadItemAsync<Product>(id, partitionKey);

            Console.WriteLine($"Item in database with id: {response.Resource.id} and name: {response.Resource.name}");
            Console.WriteLine($"[{response.Resource.id}]\t{response.Resource.name} ({response.Resource.price:C})");


            response.Resource.price = 35.00d;
            response.Resource.tags = new string[] { "brown", "new", "crisp" };
            response.Resource.ttl = 1000;

            await container.ReplaceItemAsync<Product>(response.Resource, id, partitionKey);


            await container.DeleteItemAsync<Product>(id, partitionKey);

            PartitionKey partitionKey1 = new("26C74104-40BC-4541-8EF5-9892F7F03D73");

           // await container.DeleteAllItemsByPartitionKeyStreamAsync < (partitionKey1);

        }
    }
}
