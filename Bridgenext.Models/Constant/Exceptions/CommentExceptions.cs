namespace Bridgenext.Models.Constant.Exceptions
{
    public static class CommentExceptions
    {
        public readonly static string RequiredObject = "User object can not be null.";
        public readonly static string DocumentNotExist = "Document does not exist.";
        public readonly static string RequiredIdDocument = "Id Document must not be empty or null.";
        public readonly static string RequiredId = "Id must not be empty or null.";
        public readonly static string RequiredContent = "Content must not be empty or null.";
        public readonly static string CreateUserNotExist = "It's necessary a exist user in the system to create/delete comments.";
        public readonly static string CommentNotExist = "Comment does not exist.";
    }
}
