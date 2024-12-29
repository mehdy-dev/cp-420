using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Database = Microsoft.Azure.Cosmos.Database;

namespace cosmodb01
{
    internal class Program
    {

        static async Task Main(string[] args)
        {

            CosmosClientOptions options = new()
            {
                AllowBulkExecution = true
            };

            CosmosClientBuilder builder = new CosmosClientBuilder("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
                .WithConsistencyLevel(ConsistencyLevel.Eventual);

            builder.AddCustomHandlers(new LogHandler());

            CosmosClient client = builder.Build();

            Database result = await client.CreateDatabaseIfNotExistsAsync("cosmicworks");

            Container container = await result.CreateContainerIfNotExistsAsync("products", "/categoryId", 400);



            List<Product> productsToInsert = new Faker<Product>()
                .StrictMode(true)
                .RuleFor(o => o.id, (f, o) => Guid.NewGuid().ToString())
                .RuleFor(o => o.name, (f, o) => f.Commerce.ProductName())
                .RuleFor(o => o.price, (f, o) => Convert.ToDouble(f.Commerce.Price(max: 1000, min: 10, decimals: 2)))
                .RuleFor(o => o.categoryId, (f, o) => f.Commerce.Department(1))
                .RuleFor(o => o.ttl, (f, o) => 60 * 24 * 60 * 60) // Set TTL to 2 months in seconds
                .RuleFor(o => o.tags, (f, o) => f.Random.ListItems(
                    new[] { "popular", "new", "discounted", "featured", "trending" },
                    f.Random.Number(1, 3) // Assign 1 to 3 random tags
                ).ToArray())
                .Generate(200);



            List<Task> concurrentTasks = new List<Task>();

            foreach (Product product in productsToInsert)
            {
                concurrentTasks.Add(
                    container.CreateItemAsync(product, new PartitionKey(product.categoryId))
                );
            }

            await Task.WhenAll(concurrentTasks);

            Console.WriteLine("Bulk tasks complete");


        }
    }
}
