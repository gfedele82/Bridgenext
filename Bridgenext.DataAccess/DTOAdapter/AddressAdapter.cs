using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.DataAccess.DTOAdapter
{
    public static class AddressAdapter
    {
        public static Addreesses ToDatabaseModel(this CreateAddressRequest addressRequest, Guid userId)
        {
            if (addressRequest == null || userId == Guid.Empty)
            {
                return null;
            }

            return new Addreesses
            {
                Id = Guid.NewGuid(),
                City = addressRequest.City,
                Country = addressRequest.Country,
                IdUser = userId,
                Line1 = addressRequest.Line1,
                Line2 = addressRequest.Line2,
                Zip = addressRequest.Zip,
                CreateDate = DateTime.Now,
                CreateUser = addressRequest.CreateUser,
                ModifyDate = DateTime.Now,
                ModifyUser = addressRequest.CreateUser,
            };
        }

        public static Addreesses ToDatabaseModel(this UpdateAddressRequest addressRequest, Addreesses existAddress)
        {
            if (addressRequest == null  || existAddress == null)
            {
                return null;
            }

            return new Addreesses
            {
                Id = existAddress.Id,
                City = addressRequest.City,
                Country = addressRequest.Country,
                IdUser = existAddress.IdUser,
                Line1 = addressRequest.Line1,
                Line2 = addressRequest.Line2,
                Zip = addressRequest.Zip,
                CreateDate = existAddress.CreateDate,
                CreateUser = existAddress.CreateUser,
                ModifyDate = DateTime.Now,
                ModifyUser = addressRequest.ModifyUser,
            };
        }

        public static Addreesses ToDatabaseModel(this UpdateAddressRequest addressRequest, Guid userId)
        {
            if (addressRequest == null)
            {
                return null;
            }

            return new Addreesses
            {
                Id = Guid.NewGuid(),
                City = addressRequest.City,
                Country = addressRequest.Country,
                IdUser = userId,
                Line1 = addressRequest.Line1,
                Line2 = addressRequest.Line2,
                Zip = addressRequest.Zip,
                CreateDate = DateTime.Now,
                CreateUser = addressRequest.ModifyUser,
                ModifyDate = DateTime.Now,
                ModifyUser = addressRequest.ModifyUser,
            };
        }

        public static CreateAddressRequest ToCreateModel(this UpdateAddressRequest updateAddress)
        {
            if(updateAddress == null)
            {
                return null;
            }

            return new CreateAddressRequest
            {
               City = updateAddress.City,
               Country = updateAddress.Country,
               CreateUser = updateAddress.ModifyUser,
               Line1 = updateAddress.Line1,
               Line2= updateAddress.Line2,
               Zip = updateAddress.Zip
            };
        }

        public static IEnumerable<CreateAddressRequest> ToCreateModel(this IEnumerable<UpdateAddressRequest> addressesRequest)
        {
            foreach (var address in addressesRequest)
            {
                yield return address.ToCreateModel();
            }
        }


        public static IEnumerable<Addreesses> ToDatabaseModel(this IEnumerable<CreateAddressRequest> addressesRequest, Guid userId)
        {
            foreach (var address in addressesRequest)
            {
                yield return address.ToDatabaseModel(userId);
            }
        }


        public static AddressDto ToDomainModel(this Addreesses dbAddress)
        {
            if (dbAddress == null)
            {
                return null;
            }

            return new AddressDto()
            {
                Id = dbAddress.Id,
                City = dbAddress.City,
                Country = dbAddress.Country,
                Zip = dbAddress.Zip,
                Line1 = dbAddress.Line1,
                Line2 = dbAddress.Line2,
                CreateDate = dbAddress.CreateDate,
                CreateUser = dbAddress.CreateUser,
                ModifyDate = dbAddress.ModifyDate,
                ModifyUser = dbAddress.ModifyUser,
  
            };
        }

        public static IEnumerable<AddressDto> ToDomainModel(this IEnumerable<Addreesses> dbAddresses)
        {
            foreach (var address in dbAddresses)
            {
                yield return address.ToDomainModel();
            }
        }
    }
}
