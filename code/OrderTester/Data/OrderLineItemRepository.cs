using Microsoft.Data.SqlClient;

namespace OrderTester.Data
{
    public class OrderLineItemRepository : IRepository
    {
        public OrderLineItemRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public void Create()
        {
            string sql1 = "SELECT SalesOrderID as OrderID FROM SalesOrder ORDER BY NEWID();";
            int orderID = -1;

            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderID = reader.GetInt32(0);
                    }
                }
            }

            if (orderID == -1) return;

            string sql2 = "SELECT ProductID FROM Product ORDER BY NEWID();";
            int productID = -1;

            using (SqlCommand command = new SqlCommand(sql2, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productID = reader.GetInt32(0);
                    }
                }
            }

            if (productID == -1) return;

            string sql3 = "INSERT SalesOrderLineItem(SalesOrderID, ProductID, Quantity) VALUES (@orderID, @productID, @quantity);";

            using (SqlCommand command = new SqlCommand(sql3, connection))
            {
                command.Parameters.AddWithValue("@orderID", orderID);
                command.Parameters.AddWithValue("@productID", productID);
                command.Parameters.AddWithValue("@quantity", RandomNumberGenerator.Next(1, 1000));

                int quantity = command.ExecuteNonQuery();
            }

            Console.WriteLine("Created Line Item for order {0}", orderID);
        }

        public void Read()
        {
            string sql = "SELECT TOP 1 [SalesOrderLineItemID], [SalesOrderID] FROM [SalesOrderLineItem] ORDER BY NEWID();";
            
            int lineID = -1;
            int orderID = -1;

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lineID = reader.GetInt32(0);
                        orderID = reader.GetInt32(1);
                    }
                }
            }

            if ((lineID == -1) || (orderID == -1)) return;

            Console.WriteLine("Read line item {0} from order {1}", lineID, orderID);
        }

        public void Update()
        {
            string sql1 = "SELECT TOP 1 [SalesOrderLineItemID], [SalesOrderID], [ProductID], [Quantity] FROM [SalesOrderLineItem] ORDER BY NEWID();";

            int lineID = -1;
            int orderID = -1;
            int productID = -1;
            int quantity = -1;

            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lineID = reader.GetInt32(0);
                        orderID = reader.GetInt32(1);
                        productID = reader.GetInt32(2);
                        quantity = reader.GetInt32(3);
                    }
                }
            }

            if ((lineID == -1) || (orderID == -1) || (productID == -1)) return;

            string sql2 = "UPDATE [SalesOrderLineITem] SET [Quantity] = @quantity WHERE [SalesOrderLineItemID] = @lineID and [SalesOrderID] = @orderID;";

            using (SqlCommand command = new SqlCommand(sql2, connection))
            {
                command.Parameters.AddWithValue("@quantity", quantity * 2);
                command.Parameters.AddWithValue("@lineID", lineID);
                command.Parameters.AddWithValue("@orderID", orderID);

                int count = command.ExecuteNonQuery();
            }

            Console.WriteLine("Updated line {0} on order {1}", lineID, orderID);
        }

        public void Delete()
        {
            string sql1 = "SELECT TOP 1 [SalesOrderLineItemID], [SalesOrderID] FROM [SalesOrderLineItem] ORDER BY NEWID();";

            int lineID = -1;
            int orderID = -1;

            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lineID = reader.GetInt32(0);
                        orderID = reader.GetInt32(1);
                    }
                }
            }

            if ((lineID == -1) || (orderID == -1)) return;

            string sql2 = "DELETE FROM [SalesOrderLineItem] WHERE [SalesOrderLineItemID] = @lineID and [SalesOrderID] = @orderID;";

            using (SqlCommand command = new SqlCommand(sql2, connection))
            {
                command.Parameters.AddWithValue("@lineID", lineID);
                command.Parameters.AddWithValue("@orderID", orderID);

                int count = command.ExecuteNonQuery();
            }       

            Console.WriteLine("Deleted line {0} from order {1}", lineID, orderID); 
        }

        private SqlConnection connection;

        private readonly Random RandomNumberGenerator = new Random(DateTime.Now.Second);
    }
}