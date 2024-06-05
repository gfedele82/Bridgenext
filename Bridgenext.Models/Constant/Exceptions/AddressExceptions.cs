namespace Bridgenext.Models.Constant.Exceptions
{
    public static class AddressExceptions
    {
        public readonly static string RequiredObject = "User object can not be null.";
        public readonly static string RequiredLine1 = "Line1 must not be empty or null.";
        public readonly static string RequiredCity = "City must not be empty or null.";
        public readonly static string RequiredCountry = "Country must not be empty or null.";
        public readonly static string RequiredZip = "Zip must not be empty or null.";
        public readonly static string CreateAddressNotExist = "It's necessary a exist user in the system to create/modify/delete addresses.";
        public readonly static string RequiredIdAddress = "Id Address must no be empty or null.";
        public readonly static string AddressNotExist = "Address does not found";
    }
}
