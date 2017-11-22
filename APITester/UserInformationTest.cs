using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RomBe.Entities;
using RomBeRepository;
using RomBe.Services.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;

namespace APITester
{
    [TestClass]
    public class UserInformationTest
    {
        /// <summary>
        /// return the user information object
        /// </summary>
        [TestMethod]
        public void ShouldReturnUserInformationObject()
        {

            IUnitOfWork unitOfWork = new RomBeEntities();
            var userInformationRepository = new UserInformationRepository(unitOfWork);
            var userInformationController = new UserInformationController(userInformationRepository)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            var userInformation = userInformationController.Get(6);

            Assert.IsNotNull(userInformation);
        }

        //return status code 404 not found
        [TestMethod]
        public void ShouldReturnUserStatusCodeNotFound()
        {

            IUnitOfWork unitOfWork = new RomBeEntities();
            var userInformationRepository = new UserInformationRepository(unitOfWork);
            var userInformationController = new UserInformationController(userInformationRepository)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            HttpResponseMessage userInformation = (HttpResponseMessage)userInformationController.Get(6);

            Assert.AreEqual(HttpStatusCode.NotFound, userInformation.StatusCode);
        }
    }
}
