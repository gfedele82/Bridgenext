using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Configurations;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Models.Schema.NotSQL;

namespace Bridgenext.DataAccess.Repositories
{
    public class MongoRepository : IMongoRepostory
    {
        private readonly string _stringConnection;
        private readonly string _databaseName;
        private readonly string _collectionName;

        public MongoRepository(IConfigurationRoot configuration)
        {
            var mongoSetting = configuration.GetSection("MongoDB").Get<MongoSettings>();
            _databaseName = mongoSetting.DbName;
            _collectionName = mongoSetting.DbCollection;
            _stringConnection = $"mongodb://{mongoSetting.User}:{mongoSetting.Password}@{mongoSetting.Server}/{mongoSetting.DbName}?authSource=admin";

        }

        public async Task<bool> CreateDocument(Documents document, string fileContent)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(_stringConnection);
            clientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var _client = new MongoClient(clientSettings);
            var _database = _client.GetDatabase(_databaseName);
            var _collection = _database.GetCollection<BsonDocument>(_collectionName);

            try
            {
                var _mongoDocument = new BsonDocument
                {
                { "IdDb", document.Id.ToString() },
                { "content", fileContent },
                { "uploadDate", DateTime.Now },
                { "CreateUser", document.CreateUser}
            };

                _collection.InsertOne(_mongoDocument);                 
            }
            catch
            {
                return false;
            }


            return true;
        }

        public async Task<List<MongoDocuments>> SearchByText(string text)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(_stringConnection);
            clientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var _client = new MongoClient(clientSettings);
            var _database = _client.GetDatabase(_databaseName);
            var _collection = _database.GetCollection<MongoDocuments>(_collectionName);

            var keys = Builders<MongoDocuments>.IndexKeys.Text(x => x.content);
            var indexModel = new CreateIndexModel<MongoDocuments>(keys);
            await _collection.Indexes.CreateOneAsync(indexModel);

            var filter = Builders<MongoDocuments>.Filter.Text(text);

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
