using System;

namespace platychat_dotnet.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string UsersCollectionName { get; set; }
    public string ChatsCollectionName { get; set; }
}
