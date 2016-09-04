using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleSignOnKata.myservice;
using SingleSignOnKata.sso;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class MyServiceTests
    {
        [TestMethod]
        public void invalidSSOTokenIsRejected()
        {
            MyService service = new MyService(null);
            Response response = service.handleRequest(new Request("Foo", null));
            Assert.AreNotEqual("hello Foo!", response.getText());
        }
    }
}
