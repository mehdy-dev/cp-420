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

            string sql = "SELECT * FROM products p";

            QueryDefinition query = new(sql);


            using FeedIterator<Product> feed = container.GetItemQueryIterator<Product>(
                queryDefinition: query
            );

            while (feed.HasMoreResults)
            {

                FeedResponse<Product> response = await feed.ReadNextAsync();
                foreach (Product product in response)
                {
                    Console.WriteLine($"[{product.id}]\t{product.name,35}\t{product.price,15:C}");
                }

            }


        }
    }
}
