namespace Bridgenext.Models.Constant.Exceptions
{
    public static class DocumentExceptions
    {
        public readonly static string RequiredObject = "User object can not be null.";
        public readonly static string CreateAddressNotExist = "It's necessary a exist user in the system to create/modify/delete documents.";
        public readonly static string RequiredName = "Name must not be empty or null.";
        public readonly static string RequiredDescription = "Description must not be empty or null.";
        public readonly static string RequiredFileContentNotNull = "File and Content must not be empty or null at the same time.";
        public readonly static string RequiredFileContentSameTime = "File and Content must not be selected at the same time.";
        public readonly static string UserNotExist = "User does not found";
    }
}
