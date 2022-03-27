using Microsoft.Data.SqlClient;

namespace OrderTester.Data
{
    public interface IRepository
    {
        void Create();

        void Read();

        void Update();

        void Delete();
    }
}