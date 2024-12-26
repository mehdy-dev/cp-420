using Microsoft.Azure.Cosmos.Fluent;
using System;
using Azure.Identity;
using Azure.Core;
using Microsoft.Azure.Cosmos;
using System.Net;

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

        }
    }
}
