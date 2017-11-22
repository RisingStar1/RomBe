using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RomBe.Services.Controllers;
using RomBeRepository;
using RomBe.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using RomBe;
using System.Web.Http.Routing;
using System.Net;
using System.Web.Http.OData;

namespace APITester
{
    [TestClass]
    public class UserControllerTest
    {
        /// <summary>
        /// user is exist should return the user object
        /// </summary>
        [TestMethod]
        public void ShouldReturnTheUserObjectUserIsExist()
        {
            IUnitOfWork unitOfWork = new RomBeEntities();
            var userRepository = new UserRepository(unitOfWork);
            var userController = new UserController(userRepository)
                {
                    Request = new HttpRequestMessage()
                    {
                        Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                    }
                };

            User user = userController.Get(8);

            Assert.IsNotNull(user);
        }

        /// <summary>
        /// user is not exist should return null object
        /// </summary>
        [TestMethod]
        public void ShouldReturnTheNullUserIsNotExist()
        {
            IUnitOfWork unitOfWork = new RomBeEntities();
            var userRepository = new UserRepository(unitOfWork);
            var userController = new UserController(userRepository)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            User user = userController.Get(1);

            Assert.IsNull(user);
        }

        /// <summary>
        /// return all users in db
        /// </summary>
        [TestMethod]
        public void ShouldRetrunAllUsers()
        {
            IUnitOfWork unitOfWork = new RomBeEntities();
            var userRepository = new UserRepository(unitOfWork);
            var userController = new UserController(userRepository);

            // Act
            List<User> users = userController.Get();

            // Assert
            Assert.AreEqual(1, users.Count);

        }

        /// <summary>
        /// Add New User
        /// </summary>
        [TestMethod]
        public void AddNewUser()
        {
            IUnitOfWork unitOfWork = new RomBeEntities();
            var userRepository = new UserRepository(unitOfWork);

            // Arrange
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "User" } });
            var userController = new UserController(userRepository)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:1680/api/user/")
                {
                    Properties = 
                   {
                       { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                       { HttpPropertyKeys.HttpRouteDataKey, httpRouteData } 
                   }
                }
            };

            // Act
            Random random = new Random();
            var response = userController.Post(new User()
            {
                Username = "unitTest"+random.Next(int.MaxValue).ToString(),
                Password = "1234"
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        }

        /// <summary>
        /// Add New user when the ID is already in use
        /// </summary>
        [TestMethod]
        public void AddNewUserIsExist()
        {
            IUnitOfWork unitOfWork = new RomBeEntities();
            var userRepository = new UserRepository(unitOfWork);

            // Arrange
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "User" } });
            var userController = new UserController(userRepository)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:1680/api/user/")
                {
                    Properties = 
                   {
                       { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                       { HttpPropertyKeys.HttpRouteDataKey, httpRouteData } 
                   }
                }
            };

            // Act
            //Change the data or delete from db
            var response = userController.Post(new User()
            {
                Username = "test",
                Password = "1234"
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

        }

        /// <summary>
        /// Check the put/patch method
        /// </summary>
        [TestMethod]
        public void UpdateUserData()
        {
            // Arrange
            IUnitOfWork unitOfWork = new RomBeEntities();
            var userRepository = new UserRepository(unitOfWork);
            var userController = new UserController(userRepository)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };
            Delta<User> delta = new Delta<User>();
            delta.TrySetPropertyValue("Password", "1111");

            var response = userController.Put(8, delta);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
