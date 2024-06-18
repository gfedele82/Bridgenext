using Bridgenext.DataAccess.DTOAdapter;
using Bridgenext.DataAccess.Interfaces;
using Bridgenext.DataAccess.Repositories;
using Bridgenext.Engine;
using Bridgenext.Engine.Validators;
using Bridgenext.Models.DTO;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;
using System;

namespace Bridgenext.Test.UnitTest.Engines
{
    [TestFixture]
    public class UsersEngineTest
    {
        Mock<ILogger<UsersEngine>> _logger;
        Mock<IUserRepository> _userRepository;
        Mock<IAddressRepository> _addressRepositoty;
        Mock<IValidator<CreateUserRequest>> _addUserRequestValidator;
        Mock<IValidator<CreateAddressRequest>> _addAddresesValidator;
        Mock<IValidator<DeleteUserRequest>> _deleteUserRequestValidator;
        Mock<IValidator<UpdateUserRequest>> _updateUserRequestValidator;
        Mock<IValidator<UpdateAddressRequest>> _updateAddressRequestValidator;
        Mock<IValidator<string>> _emailRequestValidator;
        UsersEngine _sut;
        CreateUserRequest _createUserRequest;
        CreateAddressRequest _createAddressRequest;
        DeleteUserRequest _deleteAddressRequest;
        UpdateUserRequest _updateUserRequest;
        UpdateAddressRequest _updateAddressRequest;
        UserTestBuilder _userTestBuilder;
        AddressTestBuilder _addressTestBuilder;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<UsersEngine>>();
            _userRepository = new Mock<IUserRepository>();
            _addressRepositoty = new Mock<IAddressRepository>();
            _addUserRequestValidator = new Mock<IValidator<CreateUserRequest>>();
            _addAddresesValidator = new Mock<IValidator<CreateAddressRequest>>();
            _deleteUserRequestValidator = new Mock<IValidator<DeleteUserRequest>>();
            _updateUserRequestValidator = new Mock<IValidator<UpdateUserRequest>>();
            _updateAddressRequestValidator = new Mock<IValidator<UpdateAddressRequest>>();
            _emailRequestValidator = new Mock<IValidator<string>>();
            _sut = new UsersEngine(_logger.Object,
                _userRepository.Object,
                _addressRepositoty.Object,
                _addUserRequestValidator.Object,
                _addAddresesValidator.Object,
                _deleteUserRequestValidator.Object,
                _updateUserRequestValidator.Object,
                _updateAddressRequestValidator.Object,
                _emailRequestValidator.Object);
            _userTestBuilder = new UserTestBuilder();
            _addressTestBuilder = new AddressTestBuilder();
            _createUserRequest = _userTestBuilder.CreateBuilder();
            _createAddressRequest = _addressTestBuilder.CreateBuilder();
            _deleteAddressRequest = _userTestBuilder.DeleteBuilder();
            _updateUserRequest = _userTestBuilder.UpdateBuilder();
            _updateAddressRequest = _addressTestBuilder.UpdateBuilder();
        }

        [Test]
        public void Given_ACreateUserRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _addUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            _addAddresesValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.CreateUser(_createUserRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _userRepository.Verify(x => x.InsertAsync(It.IsAny<Users>()), Times.Never);
        }

        [Test]
        public void Given_ACreateUserRequest2_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _addUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _addAddresesValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.CreateUser(_createUserRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _userRepository.Verify(x => x.InsertAsync(It.IsAny<Users>()), Times.Never);
        }

        [Test]
        public void Given_AModifyUserRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _updateUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            _updateAddressRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _addAddresesValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ModifyUser(_updateUserRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _userRepository.Verify(x => x.UpdateAsync(It.IsAny<Users>()), Times.Never);
        }

        [Test]
        public void Given_AModifyUserRequest2_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("The system cuold not proccess this operation. Please Try again.");

            var user = _userTestBuilder.DbBuild();
            var expectedDB = _updateUserRequest.ToDatabaseModel(user);

            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedDB);

            _updateUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _updateAddressRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            _addAddresesValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            var exceptionReceived = ClassicAssert.ThrowsAsync<ApplicationException>(async () => await _sut.ModifyUser(_updateUserRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _userRepository.Verify(x => x.UpdateAsync(It.IsAny<Users>()), Times.Never);
        }

        [Test]
        public void Given_AModifyUserRequest3_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("The system cuold not proccess this operation. Please Try again.");

            var user = _userTestBuilder.DbBuild();
            var expectedDB = _updateUserRequest.ToDatabaseModel(user);
            _updateUserRequest.Addresses.FirstOrDefault().Id = Guid.Empty;

            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedDB);

            _updateUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _updateAddressRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _addAddresesValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ApplicationException>(async () => await _sut.ModifyUser(_updateUserRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _userRepository.Verify(x => x.UpdateAsync(It.IsAny<Users>()), Times.Never);
        }

        [Test]
        public void Given_AEmailInvalid_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _emailRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.GetUserExistByEmail("test"));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _userRepository.Verify(x => x.InsertAsync(It.IsAny<Users>()), Times.Never);
        }

        [Test]
        public void Given_ADeleteUserRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _deleteUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.DeleteUser(_deleteAddressRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _userRepository.Verify(x => x.InsertAsync(It.IsAny<Users>()), Times.Never);
        }

        [Test]
        public async Task Given_NewCreateUserRequest_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_OneTransactionAdded()
        {
            _addUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _addAddresesValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();

            await _sut.CreateUser(_createUserRequest);

            _userRepository.Verify(x => x.InsertAsync(It.IsAny<Users>()), Times.Once);
        }

        [Test]
        public async Task Given_NewDeleteUserRequest_When_AllValidationsPass_Then_DataAccessIsCalledToDelete_And_Validates_OneTransactionAdded()
        {
            _deleteUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            var user = _userTestBuilder.DbBuild();
            var expectedDB = _updateUserRequest.ToDatabaseModel(user);

            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedDB);

            await _sut.DeleteUser(_deleteAddressRequest);

            _userRepository.Verify(x => x.DeleteAsync(It.IsAny<Users>()), Times.Once);
        }

        [Test]
        public async Task Given_NewUpdateUserRequest_When_AllValidationsPass_Then_DataAccessIsCalledToUpdate_And_Validates_OneTransactionAdded()
        {
            _updateUserRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _updateAddressRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _addAddresesValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            var user = _userTestBuilder.DbBuild();
            var expectedDB = _updateUserRequest.ToDatabaseModel(user);

            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedDB);

            await _sut.ModifyUser(_updateUserRequest);

            _userRepository.Verify(x => x.UpdateAsync(It.IsAny<Users>()), Times.Once);
        }

        [Test]
        public async Task Given_GetById_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_ReturnedData()
        {
            var expectedDB = _userTestBuilder.DbBuild();

            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedDB);

            var response = await _sut.GetUserById(expectedDB.Id);

            ClassicAssert.That(response.Id == expectedDB.Id);
        }

        [Test]
        public async Task Given_GetExist_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_ReturnedData()
        {
            var expectedDB = _userTestBuilder.DbBuild();

            _userRepository.Setup(x => x.IdExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            _emailRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();

            var response = await _sut.GetUserExistByEmail(expectedDB.Email);

            ClassicAssert.IsTrue(response);
        }

        [Test]
        public async Task Given_GetByEmail_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_ReturnedData()
        {
            var expectedDB = _userTestBuilder.DbBuild();
            List<Users> listUser = [expectedDB];

            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);

            var response = await _sut.GetUserByEmail(expectedDB.Email);

            ClassicAssert.That(response.Count > 0);
        }

        [TestCase(1, 10)]
        public async Task Given_APagination_WhenAllFieldsPass_Then_AListIsReturned(int pageNumber, int pageSize)
        {
            var pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var expectedDBUserDatabaseModels = new List<Users>(5);
            for (int index = 0; index < 5; index++)
            {
                var userTestBuilder = new UserTestBuilder();
                var userModel = userTestBuilder.DbBuild();

                expectedDBUserDatabaseModels.Add(userModel);
            }
            var expectedPaginatedList = new PaginatedList<Users>()
            {
                Items = expectedDBUserDatabaseModels,
                Total = 5
            };

            _userRepository.Setup(x => x.GetAllAsync(pagination)).ReturnsAsync(expectedPaginatedList);

            var actualPaginationResult = await _sut.GetAllUsers(pagination);

            ClassicAssert.IsNotNull(actualPaginationResult);
            ClassicAssert.IsTrue(actualPaginationResult.Items.Any());
            ClassicAssert.That(actualPaginationResult.Total > 0);
        }
    }
}
