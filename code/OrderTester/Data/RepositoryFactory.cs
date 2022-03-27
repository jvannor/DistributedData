using Microsoft.Data.SqlClient;

namespace OrderTester.Data
{
    public static class RepositoryFactory
    {
        public static IRepository CreateRepository(string repositoryType, SqlConnection connection)
        {
            IRepository repository;

            switch (repositoryType)
            {
                case "CUSTOMER":
                    repository = new CustomerRepository(connection);
                    break;
                case "PRODUCT":
                    repository = new ProductRepository(connection);
                    break;
                case "ORDER":
                    repository = new OrderRepository(connection);
                    break;
                case "LINEITEM":
                    repository = new OrderLineItemRepository(connection);
                    break;
                default:
                    repository = new CustomerRepository(connection);
                    break;
            }

            return repository;
        }
    }
}