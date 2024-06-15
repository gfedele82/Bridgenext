using Bogus;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.Test.Builders
{
    public class AddressTestBuilder
    {
        private readonly CreateAddressRequest _createAddress;
        private readonly UpdateAddressRequest _updateAddress;
        private readonly Addreesses _dbAddress;
        private readonly string _adminUser = "admin@admin.admin";

        public AddressTestBuilder() {

            Faker faker = new("en_US");

            _createAddress = new CreateAddressRequest()
            {
                City = faker.Address.City(),
                Country = faker.Address.Country(),
                CreateUser = _adminUser,
                Line1 = faker.Address.StreetAddress(),
                Zip = faker.Address.ZipCode()
            };

            _updateAddress = new UpdateAddressRequest()
            {
                City = faker.Address.City(),
                Country = faker.Address.Country(),
                ModifyUser = _adminUser,
                Line1 = faker.Address.StreetAddress(),
                Zip = faker.Address.ZipCode(),
                Id = Guid.NewGuid()
            };

            Guid idUser = Guid.NewGuid();
            _dbAddress = new Addreesses()
            {
                City = faker.Address.City(),
                Country = faker.Address.Country(),
                CreateUser = _adminUser,
                Line1 = faker.Address.StreetAddress(),
                Zip = faker.Address.ZipCode(),
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                ModifyUser = _adminUser,
                ModifyDate = DateTime.Now,
                IdUser = idUser,
                User = new Users()
                {
                    Id = idUser,
                    Email = _adminUser
                }
            };
        }

        public CreateAddressRequest CreateBuilder() => _createAddress;

        public UpdateAddressRequest UpdateBuilder() => _updateAddress;

        public Addreesses DbBuild() => _dbAddress;

    }
}
