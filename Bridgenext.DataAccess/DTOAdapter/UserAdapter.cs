using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.DataAccess.DTOAdapter
{
    public static class UserAdapter
    {
        public static Users ToDatabaseModel(this CreateUserRequest userRequest)
        {
            if (userRequest == null)
            {
                return null;
            }

            var IdUser = Guid.NewGuid();
            return new Users
            {
                Id = IdUser,
                IdUserType = userRequest.IdUserType,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Email = userRequest.Email,
                CreateDate = DateTime.Now,
                CreateUser = userRequest.CreateUser,
                ModifyDate = DateTime.Now,
                ModifyUser = userRequest.CreateUser,
                UserTypes = new UsersTypes()
                {
                    Id = userRequest.IdUserType,
                    Type = Enum.GetName(typeof(UsersTypeEnum), userRequest.IdUserType)
                },
                Addreesses = userRequest.Addresses.ToDatabaseModel(IdUser).ToList()
                
            };
        }

        public static Users ToDatabaseModel(this UpdateUserRequest userRequest, Users existUser)
        {
            if (userRequest == null || existUser == null)
            {
                return null;
            }

            existUser.IdUserType = userRequest.IdUserType;
            existUser.FirstName = userRequest.FirstName;
            existUser.LastName = userRequest.LastName;
            existUser.Email = userRequest.Email;
            existUser.ModifyDate = DateTime.Now;
            existUser.ModifyUser = userRequest.ModifyUser;
            existUser.UserTypes.Id = userRequest.IdUserType;
            existUser.UserTypes.Type = Enum.GetName(typeof(UsersTypeEnum), userRequest.IdUserType);

            return existUser;
        }

        public static Users ToDatabaseModel(this DeleteUserRequest userRequest, Users existUser)
        {
            if (userRequest == null || existUser == null)
            {
                return null;
            }

            existUser.ModifyDate = DateTime.Now;
            existUser.ModifyUser = userRequest.ModifyUser;

            return existUser;

        }

        public static IEnumerable<UserDto> ToDomainModel(this IEnumerable<Users> dbUsers)
        {
            foreach (var users in dbUsers)
            {
                yield return users.ToDomainModel();
            }
        }

        public static UserDto ToDomainModel(this Users dbUser)
        {
            if (dbUser == null)
            {
                return null;
            }

            return new UserDto()
            {
                Id = dbUser.Id,
                Email = dbUser.Email,
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                CreateDate = dbUser.CreateDate,
                CreateUser = dbUser.CreateUser,
                ModifyDate = dbUser.ModifyDate,
                ModifyUser = dbUser.ModifyUser,
                UserType = new UserTypeDto()
                {
                    Id = dbUser.UserTypes.Id,
                    Type = dbUser.UserTypes.Type
                },
                Addresses = dbUser.Addreesses.ToDomainModel().ToList()
                
            };
        }
    }
}
