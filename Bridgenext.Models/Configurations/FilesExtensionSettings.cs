namespace Bridgenext.Models.Configurations
{
    public class FilesExtensionSettings
    {
        public const string KEY = "FilesExtension";

        public List<string> Image { get; set; }

        public List<string> Video { get; set; }

        public List<string> Document { get; set; }
    }
}
