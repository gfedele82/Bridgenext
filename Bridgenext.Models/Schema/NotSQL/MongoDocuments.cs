using MongoDB.Bson;

namespace Bridgenext.Models.Schema.NotSQL
{
    public class MongoDocuments
    {
        public ObjectId _id { get; set; }

        public string IdDb { get; set; }

        public string content {  get; set; }

        public DateTime uploadDate { get; set; }

        public string CreateUser { get ; set; }
    }
}
