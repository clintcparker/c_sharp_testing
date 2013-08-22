using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Threading.Tasks;
using FakesTesting;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class BadIdeaTests
    {
        [TestInitialize]
        public void SetupTests()
        {

        }

        [TestMethod]
        public void BadIdeaUnitTest()
        {
            using (ShimsContext.Create())
            {
                SetupBadIdeaHttpShims();

                var badIdeaClass = new BadIdea();
                var result = badIdeaClass.GetNewBadIdea();
                Assert.IsInstanceOfType(result, typeof (string));
            }
        }

        [TestMethod]
        public void PrivateGetBadIdeaTest()
        {
            using (ShimsContext.Create())
            {
                SetupBadIdeaHttpShims();

                //REMEMBER: DON'T FORGET TO SET THE INTERNALS VISIBLE ATTRIBUTE
                var privateBadIdea = new PrivateObject(new BadIdea());

                //PARADOX: OPTIONAL PARAMETERS ARE REQUIRED
                dynamic privateResult = privateBadIdea.Invoke("GetBadIdea", false);

                Assert.IsInstanceOfType(privateResult.badidea, typeof (string));
            }
        }

        [TestMethod]
        public void PrivateGetBadIdeaWithParamTest()
        {
            using (ShimsContext.Create())
            {
                SetupBadIdeaHttpShims();

                var privateBadIdea = new PrivateObject(new BadIdea());

                dynamic privateResult = privateBadIdea.Invoke("GetBadIdea", true);

                Assert.AreEqual(privateResult.badidea, "It's like soylent for foodies");
            }
        }

        [TestMethod]
        public void GetBadIdeaExceptionTest()
        {
            using (ShimsContext.Create())
            {
                ShimHttpClient.AllInstances.GetAsyncString = (client, s) =>
                    {
                        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        var task = Task.FromResult(httpResponseMessage);
                        return task;
                    };

                var badIdeaClass = new BadIdea();
                var result = badIdeaClass.GetNewBadIdea();
                Assert.IsInstanceOfType(result, typeof(string));
            }
        }

        private static void SetupBadIdeaHttpShims()
        {
            ShimHttpClient.AllInstances.GetAsyncString = (client, s) =>
            {
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{badidea:\"It's like tumblr for lamps\"}")
                };
                var task = Task.FromResult(httpResponseMessage);
                return task;
            };
        }
    }
}
