namespace Bridgenext.Models.Constant.Exceptions
{
    public static class DocumentExceptions
    {
        public readonly static string RequiredObject = "User object can not be null.";
        public readonly static string CreateUserNotExist = "It's necessary a exist user in the system to create/modify/delete documents.";
        public readonly static string RequiredName = "Name must not be empty or null.";
        public readonly static string RequiredComment = "Comment must not be empty or null.";
        public readonly static string RequiredDescription = "Description must not be empty or null.";
        public readonly static string RequiredFileContentSameTime = "File and Content must not be selected or null/empty at the same time.";
        public readonly static string UserNotExist = "User does not found.";
        public readonly static string FileNotExist = "File does not exist.";
        public readonly static string RequiredId = "Id must not be empty or null.";
        public readonly static string FileDoesNotHaveFile = "File does not have a file associate.";
        public readonly static string DocumentDisabled = "It's necessary a exist user in the system to disable document and it should be Administrator.";
        public readonly static string DocumentNotExist = "Document does not exist.";
        public readonly static string DocumentNotMatch = "The system only can modify content in a Document with content.";
    }
}
