using MongoDB.Driver;

namespace Sankirtana.Web.Common;

public class DbStore
{
    public readonly IMongoDatabase DB;
    private EnvironmentConfig _environmentConfig;
    
    public DbStore(EnvironmentConfig environmentConfig)
    {
        _environmentConfig = environmentConfig;
        // строка подключения
        string connectionString = _environmentConfig.MongoConnectionString;
        var connection = new MongoUrlBuilder(connectionString);
        // получаем клиента для взаимодействия с базой данных
        MongoClient client = new MongoClient(connectionString);
        // получаем доступ к самой базе данных
        DB = client.GetDatabase(connection.DatabaseName);
    }
}