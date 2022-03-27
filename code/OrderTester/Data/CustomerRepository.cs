using Microsoft.Data.SqlClient;
using OrderTester.Utility;

namespace OrderTester.Data
{
    public class CustomerRepository : IRepository
    {
        public CustomerRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public void Create()
        {
            string sql = "INSERT Customer(Name) VALUES (@name);";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                string name = CustomerUtility.GetRandomCustomer();
                command.Parameters.AddWithValue("@name", name); 
                int count = command.ExecuteNonQuery();

                Console.WriteLine("Created customer, \"{0}\"", name);
            }  
        }

        public void Read()
        {
            string sql = "SELECT TOP 1 [CustomerID], [Name] FROM [Customer] ORDER BY NEWID();";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("Read customer, \"{0}\"", reader.GetString(1));
                    }
                }
            }
        }

        public void Update()
        {
            string sql1 = "SELECT TOP 1 [CustomerID], [Name] FROM [Customer] ORDER BY NEWID();";

            int customerID = -1;
            string customerName = string.Empty;

            using (SqlCommand command1 = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerID = reader.GetInt32(0);
                        customerName = reader.GetString(1);
                    } 
                }
            } 

            if (customerID > 0)
            {
                string sql2 = "UPDATE [Customer] SET [Name] = @name WHERE [CustomerID] = @customerID;";
                string name = CustomerUtility.GetRandomCustomer();

                using (SqlCommand command2 = new SqlCommand(sql2, connection))
                {
                    command2.Parameters.AddWithValue("@name", name);
                    command2.Parameters.AddWithValue("@customerID", customerID);
                    int count = command2.ExecuteNonQuery();

                    Console.WriteLine("Updated customer, \"{0}\" to \"{1}\"", customerName, name);
                }
            }              
        }

        public void Delete()
        {
            string sql1 = "SELECT TOP 1 [CustomerID], [Name] FROM [Customer] ORDER BY NEWID()";

            int customerID = -1;
            string customerName = string.Empty;

            using (SqlCommand command1 = new SqlCommand(sql1, connection))
            {
                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerID = reader.GetInt32(0);
                        customerName = reader.GetString(1);
                    }
                }
            }

            if (customerID > 0)
            {
                string sql2 = "DELETE FROM [Customer] WHERE [CustomerID] = @customerID;";
                using (SqlCommand command2 = new SqlCommand(sql2, connection))
                {
                    command2.Parameters.AddWithValue("@customerID", customerID);
                    int count = command2.ExecuteNonQuery();

                    Console.WriteLine("Deleted customer, \"{0}\"", customerName);
                }
            }
        }

        private SqlConnection connection;
    }
}