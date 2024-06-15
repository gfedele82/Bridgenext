using Bogus;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.Test.Builders
{
    public class UserTestBuilder
    {
        private readonly CreateUserRequest _createUser;
        private readonly UpdateUserRequest _updateUser;
        private readonly DeleteUserRequest _deleteUser;
        private readonly Users _dbUser;
        private readonly string _adminUser = "admin@admin.admin";
        private readonly AddressTestBuilder _addressTestBuilder;

        public UserTestBuilder()
        {
            Faker faker = new("en_US");
            _addressTestBuilder = new AddressTestBuilder();

            _createUser = new CreateUserRequest()
            {
                Addresses = [_addressTestBuilder.CreateBuilder()],
                CreateUser = _adminUser,
                Email = faker.Person.Email,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                IdUserType = (int)UsersTypeEnum.Administrator
            };

            _updateUser = new UpdateUserRequest()
            {
                Addresses = [_addressTestBuilder.UpdateBuilder()],
                ModifyUser = _adminUser,
                Email = faker.Person.Email,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                IdUserType = (int)UsersTypeEnum.Administrator,
                Id = Guid.NewGuid()
            };

            _deleteUser = new DeleteUserRequest()
            {
                Id = Guid.NewGuid(),
                ModifyUser = _adminUser
            };

            _dbUser = new Users()
            {
                Addreesses = [_addressTestBuilder.DbBuild()],
                Email = faker.Person.Email,
                FirstName = faker.Person.FirstName,
                CreateUser = _adminUser,
                CreateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                IdUserType = (int)UsersTypeEnum.Administrator,
                LastName = faker.Person.LastName,
                ModifyDate = DateTime.Now,
                ModifyUser = _adminUser,
                UserTypes = new UsersTypes()
                {
                    Id = (int)UsersTypeEnum.Administrator,
                    Type = Enum.GetName(typeof(UsersTypeEnum), (int)UsersTypeEnum.Administrator)
                }
            };

        }

        public CreateUserRequest CreateBuilder() => _createUser;
        public UpdateUserRequest UpdateBuilder() => _updateUser;
        public DeleteUserRequest DeleteBuilder() => _deleteUser;
        public Users DbBuild() => _dbUser;


    }
}
