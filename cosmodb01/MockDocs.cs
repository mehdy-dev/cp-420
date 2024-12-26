using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmodb01
{
    public static class MockDocs
    {

        public static List<Product> GetMockDocs()
        {
            List<Product> docs = new List<Product>
            {
                new()
                {
                    id = "027D0B9A-F9D9-4C96-8213-C8546C4AAE71",
                    categoryId = "26C74104-40BC-4541-8EF5-9892F7F03D72",
                    name = "LL Road Seat/Saddle",
                    price = 27.12d,
                    tags = new string[]
                    {
                        "brown",
                        "weathered"
                    }
                },
                new()
                {
                    id = "0A2D0B9A-F9D9-4C96-8213-C8546C4AAE71",
                    categoryId = "26C74104-40BC-4541-8EF5-9892F7F03D72",
                    name = "HL Mountain Frame - Black, 42",
                    price = 1003.91d,
                    tags = new string[]
                    {
                        "black",
                        "steel"
                    }
                },
                new()
                {
                    id = "0B2D0B9A-F9D9-4C96-8213-C8546C4AAE71",
                    categoryId = "26C74104-40BC-4541-8EF5-9892F7F03D72",
                    name = "HL Mountain Frame - Black, 44",
                    price = 1003.91d,
                    tags = new string[]
                    {
                        "black",
                        "steel"
                    }
                },                new()
                {
                    id = "0A2D0B9A-F9D9-4C96-8213-C8546C4AAE74",
                    categoryId = "26C74104-40BC-4541-8EF5-9892F7F03D73",
                    name = "HL Mountain Frame - Black, 42",
                    price = 1003.91d,
                    tags = new string[]
                    {
                        "black",
                        "steel"
                    }
                },
                new()
                {
                    id = "0B2D0B9A-F9D9-4C96-8213-C8546C4AAE75",
                    categoryId = "26C74104-40BC-4541-8EF5-9892F7F03D73",
                    name = "HL Mountain Frame - Black, 44",
                    price = 1003.91d,
                    tags = new string[]
                    {
                        "black",
                        "steel"
                    }
                },new()
                {
                    id = "0A2D0B9A-F9D9-4C96-8213-C8546C4AAE76",
                    categoryId = "26C74104-40BC-4541-8EF5-9892F7F03D74",
                    name = "HL Mountain Frame - Black, 42",
                    price = 1003.91d,
                    tags = new string[]
                    {
                        "black",
                        "steel"
                    }
                },
                new()
                {
                    id = "0B2D0B9A-F9D9-4C96-8213-C8546C4AAE77",
                    categoryId = "26C74104-40BC-4541-8EF5-9892F7F03D74",
                    name = "HL Mountain Frame - Black, 44",
                    price = 1003.91d,
                    tags = new string[]
                    {
                        "black",
                        "steel"
                    }
                }
            };

            return docs;
        }
    }
}
