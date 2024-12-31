using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class ItemsListener
    {
        private readonly ILogger _logger;

        public ItemsListener(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ItemsListener>();
        }

        [Function("ItemsListener")]
        public void Run([CosmosDBTrigger(
            databaseName: "cosmicworks",
            containerName: "products",
            Connection = "dp42014lab_DOCUMENTDB",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<MyDocument> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified: " + input.Count);
                _logger.LogInformation("First document Id: " + input[0].id);
                _logger.LogInformation("Just Add More Logs : " + input.Count);
            }
        }
    }

    public class MyDocument
    {
        public string id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        public bool Boolean { get; set; }
    }
}
