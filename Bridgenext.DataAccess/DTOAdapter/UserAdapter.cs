using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema;

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

            var IdUser = Guid.NewGuid();
            return new Users
            {
                Id = userRequest.Id,
                IdUserType = userRequest.IdUserType,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Email = userRequest.Email,
                CreateDate = existUser.CreateDate,
                CreateUser = existUser.CreateUser,
                ModifyDate = DateTime.Now,
                ModifyUser = userRequest.ModifyUser,
                UserTypes = new UsersTypes()
                {
                    Id = userRequest.IdUserType,
                    Type = Enum.GetName(typeof(UsersTypeEnum), userRequest.IdUserType)
                }
            };
        }

        public static Users ToDatabaseModel(this DeleteUserRequest userRequest, Users existUser)
        {
            if (userRequest == null || existUser == null)
            {
                return null;
            }

            var IdUser = Guid.NewGuid();
            return new Users
            {
                Id = existUser.Id,
                IdUserType = existUser.IdUserType,
                FirstName = existUser.FirstName,
                LastName = existUser.LastName,
                Email = existUser.Email,
                CreateDate = existUser.CreateDate,
                CreateUser = existUser.CreateUser,
                ModifyDate = DateTime.Now,
                ModifyUser = userRequest.ModifyUser,
                UserTypes = new UsersTypes()
                {
                    Id = existUser.IdUserType,
                    Type = existUser.UserTypes.Type
                }
            };
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
