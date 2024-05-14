//using MongoDB.Driver;

//public class MongoCollectionFactory : IMongoCollectionFactory
//{
//    private readonly IMongoDatabase _database;

//    public MongoCollectionFactory(IMongoDatabase database)
//    {
//        _database = database;
//    }

//    public IMongoCollection<T> GetCollection<T>(string name)
//    {
//        return _database.GetCollection<T>(name);
//    }
//}

//public interface IMongoCollectionFactory
//{
//    IMongoCollection<T> GetCollection<T>(string name);
//}