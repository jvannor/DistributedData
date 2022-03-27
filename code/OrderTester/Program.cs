using System.Threading;
using Microsoft.Data.SqlClient;
using OrderTester.Data;

string verb = args[0].ToUpper();
string directObject = args[1].ToUpper();
int count = int.Parse(args[2]);
int delay = int.Parse(args[3]);

SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
builder.DataSource = "localhost";
builder.InitialCatalog = "Orders";
builder.IntegratedSecurity = true;
builder.TrustServerCertificate = true;

using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
{
    IRepository repository = RepositoryFactory.CreateRepository(directObject, connection);

    try
    {
        connection.Open();
        for (int i=0; i<count; i++)
        {
            try
            {
                switch(verb)
                {
                    case "CREATE":
                        repository.Create();
                        break;
                    case "READ":
                        repository.Read();
                        break;
                    case "UPDATE":
                        repository.Update();
                        break;
                    case "DELETE":
                        repository.Delete();
                        break;
                    default:
                        repository.Read();
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("The application encountered an unexpected exception, {0}", ex.ToString());
            }

            Thread.Sleep(delay);
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine("The application encountered an unexpected exception, {0}", ex.ToString());
    }
}