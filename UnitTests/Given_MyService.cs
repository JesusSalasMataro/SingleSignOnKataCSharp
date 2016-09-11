using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleSignOnKata.myservice;
using SingleSignOnKata.sso;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class Given_MyService
    {
        private Mock<SingleSignOnRegistry> _singleSignOnRegistry;
        private Mock<AuthenticationGateway> _authGateway;

        [TestInitialize]
        public void InitTest()
        {
            _singleSignOnRegistry = new Mock<SingleSignOnRegistry>();
            _authGateway = new Mock<AuthenticationGateway>();
        }

        [TestMethod]
        public void When_invalidSSOToken_Then_TokenIsRejected()
        {
            // ARRANGE
            _singleSignOnRegistry.Setup(s => s.is_valid(new SSOToken())).Returns(false);
            MyService service = new MyService(_singleSignOnRegistry.Object, _authGateway.Object);

            // ACT
            Response response = service.handleRequest(new Request("Foo", new SSOToken()));

            // ASSERT
            Assert.AreNotEqual("hello Foo!", response.getText());
        }

        [TestMethod]
        public void When_validSSOToken_Then_TokenIsAccepted()
        {
            // ARRANGE
            SSOToken token = new SSOToken();
            _singleSignOnRegistry.Setup(s => s.is_valid(token)).Returns(true);
            MyService service = new MyService(_singleSignOnRegistry.Object, _authGateway.Object);

            // ACT
            Response response = service.handleRequest(new Request("Foo", token));

            // ASSERT
            Assert.AreEqual("hello Foo!", response.getText());
        }

        [TestMethod]
        public void When_validUsernameAndPassword_Then_InitServiceOk()
        {
            // ARRANGE
            SSOToken actualToken = new SSOToken();
            MyService service = new MyService(_singleSignOnRegistry.Object, _authGateway.Object);          
            string username = "marianito";
            string password = "12345678";
            _authGateway.Setup(a => a.credentialsAreValid(username, password)).Returns(true);
            _singleSignOnRegistry.Setup(s => s.register_new_session(username, password)).Returns(actualToken);
            SSOToken expectedToken;

            // ACT
            expectedToken = service.InitService(username, password);

            // ASSERT
            Assert.AreEqual(expectedToken, actualToken);
        }

        [TestMethod]
        public void When_invalidUsernameAndPassword_Then_InitServiceNoOk()
        {
            // ARRANGE
            MyService service = new MyService(_singleSignOnRegistry.Object, _authGateway.Object);
            string username = "marianito";
            string password = "12345";
            _authGateway.Setup(a => a.credentialsAreValid(username, password)).Returns(false);
            SSOToken token;

            // ACT
            token = service.InitService(username, password);

            // ASSERT
            Assert.AreEqual(token, null);
        }
    }
}
