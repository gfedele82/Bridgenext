namespace Bridgenext.Models.Constant.Exceptions
{
    public static class UserExceptions
    {
        public readonly static string RequiredObject = "User object can not be null.";
        public readonly static string RequiredFirstName = "First Name must not be empty or null.";
        public readonly static string RequiredLastName = "Last Name must not be empty or null.";
        public readonly static string RequiredEmail = "Email must not be empty or null.";
        public readonly static string InvalidEmail = "Email must be valid format.";
        public readonly static string RequiredUserType = "Type User doesn't exist.";
        public readonly static string UserExist = "User already exist.";
        public readonly static string CreateUserNotExist = "It's necessary a exist user in the system to create/modify/delete users and it should be Administrator.";
        public readonly static string RequiredIdUser = "Id User must no be empty or null.";
        public readonly static string CanNotDeleteUserAdmin = "User Admin can not deleted.";
        public readonly static string UserNotExist = "User does not found";
    }
}
