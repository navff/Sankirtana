using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Configuration;

namespace Sankirtana.Web.Common;

public class EnvironmentConfig
{
    public string MongoConnectionString { get; private set; }
    
    public EnvironmentConfig()
    {
        var yaSecretKey = Environment.GetEnvironmentVariable("YA_OBJECT_STORAGE_SECRET_KEY")!;
        if (string.IsNullOrEmpty(yaSecretKey))
        {
            throw new InvalidOperationException("There is no environment variable YA_OBJECT_STORAGE_SECRET_KEY");
        };
        
        var oss = new YandexStorageService(new YandexStorageOptions()
        {
            AccessKey = "YCAJEWT3HNKiSCnrfiXOf_Bsm",
            SecretKey = yaSecretKey,
            BucketName = "secrets"
        });
        var result = oss.ObjectService.GetAsync("sankirtana.json").Result;
        
        var stream = result.ReadAsStreamAsync().Result.Value;
        var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        
        var config = Newtonsoft.Json.JsonConvert.DeserializeObject<SankirtanaConfig>(text);
        if (config != null)
        {
            MongoConnectionString = config.MongoConnectionString;
        }
        else
        {
            throw new InvalidOperationException("Cannot read config file from Yandex Object Storage");
        }
    }
    
    public class SankirtanaConfig
    {
        public string MongoConnectionString { get; set; } = "";
    }
}