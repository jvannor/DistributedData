using Microsoft.Data.SqlClient;
using OrderTester.Utility;

namespace OrderTester.Data
{
    public class ProductRepository : IRepository
    {
        public ProductRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public void Create()
        {
            string sql = "INSERT Product(Code, Description, Price) VALUES (@code, @description, @price);";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                string description = ProductUtility.GetRandomProduct();
                int hash = description.GetHashCode();
                string code = string.Format("0x{0:X8}", hash);
                int price = ProductUtility.GetRandomPrice();

                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@price", price);

                int count = command.ExecuteNonQuery();

                Console.WriteLine("Created product, \"{0}\"", description);
            }  
        }

        public void Read()
        {
            string sql = "SELECT TOP 1 [ProductID], [Description] FROM [Product] ORDER BY NEWID();";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("Read product, \"{0}\"", reader.GetString(1));
                    }
                }
            }
 
        }

        public void Update()
        {
            string sql1 = "SELECT TOP 1 [ProductID], [Description] FROM [Product] ORDER BY NEWID();";

            int productID = -1;
            string productDescription = string.Empty;

            using (SqlCommand command1 = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productID = reader.GetInt32(0);
                        productDescription = reader.GetString(1);
                    } 
                }
            } 

            if (productID > 0)
            {
                string sql2 = "UPDATE [Product] SET [Price] = @price WHERE [ProductID] = @productID;";
                int price = ProductUtility.GetRandomPrice();

                using (SqlCommand command2 = new SqlCommand(sql2, connection))
                {
                    command2.Parameters.AddWithValue("@price", price);
                    command2.Parameters.AddWithValue("@productID", productID);
                    int count = command2.ExecuteNonQuery();

                    Console.WriteLine("Updated product, \"{0}\" to \"{1:C}\"", productDescription, price);
                }
            }              

        }

        public void Delete()
        {
            string sql1 = "SELECT TOP 1 [ProductID], [Description] FROM [Product] ORDER BY NEWID()";

            int productID = -1;
            string productDescription = string.Empty;

            using (SqlCommand command1 = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productID = reader.GetInt32(0);
                        productDescription = reader.GetString(1);
                    }
                }
            }

            if (productID > 0)
            {
                string sql2 = "DELETE FROM [Product] WHERE [ProductID] = @productID;";
                using (SqlCommand command2 = new SqlCommand(sql2, connection))
                {
                    command2.Parameters.AddWithValue("@productID", productID);
                    int count = command2.ExecuteNonQuery();

                    Console.WriteLine("Deleted product, \"{0}\"", productDescription);
                }
            }

        }

        private SqlConnection connection;
    }
}