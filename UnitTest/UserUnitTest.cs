using Microsoft.VisualStudio.TestTools.UnitTesting;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.Services;
using MythWikiBusiness.Models;
using UnitTest.FakeDAL;
using System.Collections.Generic;

namespace MythWikiTests
{
    [TestClass]
    public class UserServiceTests
    {
        private FakeUserRepo _fakeRepo;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _fakeRepo = new FakeUserRepo();
            _userService = new UserService(_fakeRepo);
        }

        [TestMethod]
        public void GetAllUsers_ShouldReturnListOfUsers()
        {
            // Act
            var result = _userService.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Register_ShouldAddNewUser()
        {
            // Arrange
            string username = "newuser";
            string password = "newpassword";
            string email = "newuser@example.com";

            // Act
            var result = _userService.Register(username, password, email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Name);
            Assert.AreEqual(password, result.Password);
            Assert.AreEqual(email, result.Email);

            var Users = _userService.GetAllUsers();
            Assert.AreEqual(3, Users.Count);
        }


    }
}
