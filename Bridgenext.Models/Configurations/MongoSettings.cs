namespace Bridgenext.Models.Configurations
{
    public class MongoSettings
    {
        public const string KEY = "MongoDB";
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DbName { get; set; }
        public string DbCollection { get; set; }
    }
}
