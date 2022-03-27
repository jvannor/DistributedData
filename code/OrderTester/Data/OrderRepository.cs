using Microsoft.Data.SqlClient;

namespace OrderTester.Data
{
    public class OrderRepository : IRepository
    {
        public OrderRepository(SqlConnection connection)
        {
            this.connection = connection;
        }
        public void Create()
        {
            string sql1 = "SELECT TOP 1 [CustomerID], [Name] FROM [Customer] ORDER BY NEWID();";

            int customerID = -1;
            string customerName = string.Empty;

            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerID = reader.GetInt32(0);
                        customerName = reader.GetString(1);
                    }
                }
            }

            if (customerID == -1) return;

            int productQuantity = RandomNumberGenerator.Next(1, 10);
            string sql2 = $"SELECT TOP {productQuantity} [ProductID] FROM [Product] ORDER BY NEWID();";
            
            List<int> products = new List<int>();
            using (SqlCommand command = new SqlCommand(sql2, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(reader.GetInt32(0));
                    }
                }
            }

            if (products.Count == 0) return;

            string shipping = DateTime.Now.Second % 2 == 0 ? "UPS" : "FEDEX";
            string terms = $"Net {DateTime.Now.Second / 10}";

            string sql3 = "INSERT SalesOrder(CustomerID, Date, ShippingInstructions, PaymentTerms) OUTPUT INSERTED.[SalesOrderId] VALUES (@customerID, @date, @shipping, @terms);";
            int orderId = -1;

            using (SqlCommand command = new SqlCommand(sql3, connection))
            {
                command.Parameters.AddWithValue("@customerID", customerID);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@shipping", shipping);
                command.Parameters.AddWithValue("@terms", terms);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        orderId = reader.GetInt32(0);
                    }
                }
            }

            if (orderId == -1) return;

            foreach(int productID in products)
            {
                string sql4 = "INSERT SalesOrderLineItem(SalesOrderID, ProductID, Quantity) VALUES (@salesOrderID, @productID, @quantity);";
                using (SqlCommand command = new SqlCommand(sql4, connection))
                {
                    command.Parameters.AddWithValue("@salesOrderID", orderId);
                    command.Parameters.AddWithValue("@productID", productID);
                    command.Parameters.AddWithValue("@quantity", RandomNumberGenerator.Next(1, 100));

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Created order for \"{0}\" with {1} products", customerName, products.Count);
        }

        public void Read()
        {
            string sql1 = "SELECT TOP 1 SO.SalesOrderID as OrderID,  CU.Name as CustomerName, SO.Date as OrderDate, SO.PaymentTerms as PaymentTerms, SO.ShippingInstructions as ShippingTerms FROM SalesOrder SO JOIN Customer CU on SO.CustomerID = CU.CustomerID ORDER BY NEWID();"; 

            int orderID = -1;
            string customerName = string.Empty;

            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        orderID = reader.GetInt32(0);
                        customerName = reader.GetString(1);
                    }
                }
            }

            if (orderID == -1) return;

            string sql2 = "SELECT SOLI.SalesOrderLineItemID as LineID, PR.Code as ProductCode, PR.Description as ProductName, PR.Price as ProductPrice, SOLI.Quantity as OrderQty FROM SalesOrderLineItem SOLI JOIN Product PR on SOLI.ProductID = PR.ProductID WHERE SOLI.SalesOrderID = @orderID;";
            int lineItemQuantity = 0;
            using (SqlCommand command = new SqlCommand(sql2, connection))
            {
                command.Parameters.AddWithValue("@orderID", orderID);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        lineItemQuantity++;
                    }
                }
            }

            if (lineItemQuantity == 0) return;

            Console.WriteLine("Read order for, \"{0}\", with {1} line items", customerName, lineItemQuantity);
        }

        public void Update()
        {
            string sql1 = "SELECT SalesOrderID as OrderID, Date FROM SalesOrder ORDER BY NEWID();";
            
            int orderID = -1;
            DateTime shipping = DateTime.MinValue;

            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        orderID = reader.GetInt32(0);
                        shipping = reader.GetDateTime(1);
                    }
                }
            }

            if (orderID == -1) return;

            string sql2 = "UPDATE SalesOrder SET Date = @date WHERE SalesOrderID = @orderID;";

            using (SqlCommand command = new SqlCommand(sql2, connection))
            {
                command.Parameters.AddWithValue("@orderID", orderID);
                command.Parameters.AddWithValue("@date", shipping.AddDays(RandomNumberGenerator.Next(1, 14)));

                int count = command.ExecuteNonQuery();
            }

            Console.WriteLine("Updated order {0} shipping date", orderID);
        }

        public void Delete()
        {
            string sql1 = "SELECT SalesOrderID as OrderID FROM SalesOrder ORDER BY NEWID();";
            
            int orderID = -1;

            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        orderID = reader.GetInt32(0);
                    }
                }
            }

            if (orderID == -1) return;

            string sql2 = "DELETE FROM SalesOrderLineItem WHERE SalesOrderId = @orderID;";

            using (SqlCommand command = new SqlCommand(sql2, connection))
            {
                command.Parameters.AddWithValue("@orderID", orderID);
                int count = command.ExecuteNonQuery();
            }

            string sql3 = "DELETE FROM SalesOrder WHERE SalesOrderId = @orderID;";

            using (SqlCommand command = new SqlCommand(sql3, connection))
            {
                command.Parameters.AddWithValue("@orderID", orderID);
                int count = command.ExecuteNonQuery();
            }

            Console.WriteLine("Deleted order {0}", orderID);
        }

        private SqlConnection connection;

        private readonly Random RandomNumberGenerator = new Random(DateTime.Now.Second);
    }
}