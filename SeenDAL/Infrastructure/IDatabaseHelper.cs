using Microsoft.Data.SqlClient;

namespace SeenDAL.Infrastructure
{
    public interface IDatabaseHelper
    {
        SqlConnection CreateConnection();
    }
}
