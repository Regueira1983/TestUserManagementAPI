using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json.Serialization;
using UserManagementAPI.Models.UsersManagement;

namespace TestUserAPI
{
    [TestClass]
    public class UserAPITest
    {
        #region "List"
        [TestMethod]
        public void Test_list_Users()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);

            //Act: testing list user endpoint
            IEnumerable<TbUser> LstUsers;
            LstUsers = userController.GetTbUsers().Result.Value;

            //Assert: Check list of users
            Assert.IsTrue(LstUsers.Count() > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_list_Users_Exception()
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);

            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;
            LstUsers = userController.GetTbUsers();
        }
        #endregion

        #region "New User"
        [TestMethod]
        public void Test_New_User_Ok()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser newUserOk;
            TbUser userAPIOk = new TbUser();
            Task<ActionResult<TbUser>> newUser;
            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;


            //Act: testing list user endpoint
            LstUsers = userController.GetTbUsers();
            newUserOk = LstUsers.Result.Value.Last();
            newUser = userController.NewTbUsers(newUserOk);

            //Assert: Check status
            Assert.AreEqual(newUser.Status, StatusCodes.Status200OK);
        }

        [TestMethod]
        public void Test_New_User_Young()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser newUserYoung;
            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;
            Task<ActionResult<TbUser>> newUser;

            //Act: testing list user endpoint
            LstUsers = userController.GetTbUsers();
            newUserYoung = LstUsers.Result.Value.Last();
            newUserYoung.DateOfBirth = new DateOnly(2020, 10, 15);
            newUser = userController.NewTbUsers(newUserYoung);

            //Assert: Check status
            Assert.AreEqual(newUser.Status, StatusCodes.Status412PreconditionFailed);
        }

        [TestMethod]
        public void Test_New_User_Older()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser newUserOld;
            TbUser userAPIOk = new TbUser();
            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;

            //Act: testing list user endpoint
            LstUsers = userController.GetTbUsers();
            newUserOld = LstUsers.Result.Value.Last();
            newUserOld.DateOfBirth = new DateOnly(1950, 10, 15);
            userAPIOk = userController.NewTbUsers(newUserOld).Result.Value;

            //Assert: Check RetirementDate
            Assert.AreEqual(userAPIOk.RetirementDate, new DateOnly(2011, 10, 15));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_New_Users_Exception()
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser userAPIEmpty = new TbUser();

            Task<ActionResult<TbUser>> response;
            response = userController.NewTbUsers(userAPIEmpty);
        }
        #endregion

        #region "Edit User"
        [TestMethod]
        public void Test_Edit_User_Ok()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser editUserOk = new TbUser("Jhon", "Lopez", "JL@test.com", new DateOnly(1983, 11, 15));
            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;
            Task<IActionResult> response;

            //Act: testing list user endpoint
            LstUsers = userController.GetTbUsers();
            editUserOk.UserId = LstUsers.Result.Value.Last().UserId;

            response = userController.UpdUser(editUserOk);

            //Assert: Check status
            Assert.AreEqual(response.Status, StatusCodes.Status200OK);
        }

        [TestMethod]
        public void Test_Edit_User_Young()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser editUserOk = new TbUser("young", "Gomez", "JG@test.com", new DateOnly(2020, 10, 15));
            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;
            Task<IActionResult> response;

            //Act: testing list user endpoint
            LstUsers = userController.GetTbUsers();
            editUserOk.UserId = LstUsers.Result.Value.Last().UserId;

            response = userController.UpdUser(editUserOk);

            //Assert: Check status
            Assert.AreEqual(response.Status, StatusCodes.Status412PreconditionFailed);
        }

        [TestMethod]
        public void Test_Edit_User_Older()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser editUserOk = new TbUser("young", "Gomez", "JG@test.com", new DateOnly(1950, 10, 15));
            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;
            Task<IActionResult> response;

            //Act: testing list user endpoint
            LstUsers = userController.GetTbUsers();
            editUserOk.UserId = LstUsers.Result.Value.Last().UserId;

            response = userController.UpdUser(editUserOk);

            //Assert: Check status
            Assert.AreNotEqual(response.Status, StatusCodes.Status200OK);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_Edit_Users_Exception()
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser userAPIEmpty = new TbUser();
            TbUser userAPIEx = new TbUser();

            Task<IActionResult> response;
            response = userController.UpdUser(userAPIEmpty);
        }
        #endregion

        #region "Delete User"
        [TestMethod]
        public void Test_Delete_User_Ok()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;
            Task<IActionResult> response;

            //Act: testing list user endpoint
            LstUsers = userController.GetTbUsers();
            response = userController.DeleteUser(LstUsers.Result.Value.Last().UserId);

            //Assert: Check status
            Assert.AreEqual(((Microsoft.AspNetCore.Mvc.ObjectResult)response.Result).Value, "Removed");
        }

        [TestMethod]
        public void Test_Delete_User_NotFound()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            IEnumerable<TbUser> LstUsers = new List<TbUser>();
            Task<IActionResult> response;
            int delelteUserError = -1;

            //Act: testing list user endpoint
            response = userController.DeleteUser(delelteUserError);

            //Assert: Check status
            Assert.AreEqual(response.Result, StatusCodes.Status304NotModified);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_Delete_Users_Exception()
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController(configuration);
            TbUser userAPIEmpty = new TbUser();
            TbUser userAPIEx = new TbUser();
            Task<ActionResult<IEnumerable<TbUser>>> LstUsers;

            LstUsers = userController.GetTbUsers();

            Task<IActionResult> response = userController.DeleteUser(LstUsers.Result.Value.Last().UserId+100);
        }
        #endregion
    }
}