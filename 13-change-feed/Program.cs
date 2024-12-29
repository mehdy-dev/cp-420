using Microsoft.Azure.Cosmos.Fluent;
using System;
using Azure.Identity;
using Azure.Core;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Reflection.Metadata;
using static Microsoft.Azure.Cosmos.Container;

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

            Container leaseContainer = await result.CreateContainerIfNotExistsAsync("productslease", "/id", 400);

            Container sourceContainer = client.GetContainer("cosmicworks", "products");
            /*
            ChangesHandler<Product> handleChanges = async (
                 IReadOnlyCollection<Product> changes,
                 CancellationToken cancellationToken
             ) => {
                 Console.WriteLine($"START\tHandling batch of changes...");
                 foreach (Product product in changes)
                 {
                     await Console.Out.WriteLineAsync($"Detected Operation:\t[{product.id}]\t{product.name}");
                 }
             };
            */


            ChangesHandler<Product> handleChanges = async (
                IReadOnlyCollection<Product> changes,
                CancellationToken cancellationToken
            ) =>
            {
                Console.WriteLine($"START\tHandling batch of changes...");
                foreach (Product product in changes)
                {
                    // Simulated previous state retrieval (e.g., from a cache or another system)
                    Product previousState = GetPreviousState(product.id);

                    if (previousState == null)
                    {
                        await Console.Out.WriteLineAsync($"Detected Operation:\t[INSERT]\t[{product.id}]\t{product.name}");
                    }
                    else
                    {
                        // Detect and log changes
                        await Console.Out.WriteLineAsync($"Detected Operation:\t[UPDATE]\t[{product.id}]\t{product.name}");
                        LogFieldChanges(previousState, product);
                    }

                    // Optional: Save current state as the new "previous state" for future comparisons
                    SaveCurrentState(product);
                }
            };


            var builderChangeFeed = sourceContainer.GetChangeFeedProcessorBuilder<Product>(
                    processorName: "productsProcessor",
                    onChangesDelegate: handleChanges
                );

            ChangeFeedProcessor processor = builderChangeFeed
                .WithInstanceName("consoleApp")
                .WithLeaseContainer(leaseContainer)
                .Build();

            await processor.StartAsync();

            Console.WriteLine($"RUN\tListening for changes...");
            Console.WriteLine("Press any key to stop");
            Console.ReadKey();

            await processor.StopAsync();


        }

        // Simulate retrieving the previous state (you'd implement this with your storage solution)
        static Product GetPreviousState(string id)
        {
            // Example: Fetch from in-memory cache, database, or secondary container
            return null; // Return null if the item is new
        }

        // Log field-level changes
        static void LogFieldChanges(Product oldItem, Product newItem)
        {
            // Compare fields and log changes
            if (oldItem.name != newItem.name)
                Console.WriteLine($"Field 'name' changed: '{oldItem.name}' -> '{newItem.name}'");
            if (oldItem.price != newItem.price)
                Console.WriteLine($"Field 'price' changed: '{oldItem.price}' -> '{newItem.price}'");
            if (!oldItem.tags.SequenceEqual(newItem.tags))
                Console.WriteLine($"Field 'tags' changed: '{string.Join(", ", oldItem.tags)}' -> '{string.Join(", ", newItem.tags)}'");
        }

        // Simulate saving the current state for future comparisons
        static void SaveCurrentState(Product product)
        {
            // Example: Save to cache, database, or secondary container
        }
    }
}

