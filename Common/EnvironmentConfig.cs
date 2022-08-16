namespace Sankirtana.Web.Common;

public class EnvironmentConfig
{
    public string MongoConnectionString { get; private set; }
    
    public EnvironmentConfig()
    {
        MongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")!;
        if (string.IsNullOrEmpty(MongoConnectionString))
        {
            throw new InvalidOperationException("There is no environment variable MONGO_CONNECTION_STRING");
        };
    }
}