using Microsoft.Data.SqlClient;

namespace UniDocuments.Text.Services.FileStorage.Connection;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}