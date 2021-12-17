using System.Threading.Tasks;
using NUnit.Framework;

namespace Haraba.GoProxy.Tests
{
    public class MainTests
    {
        private const string GoProxyUrl = "http://localhost:8000/handle";
        private const string ChromeUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36";
        private const string ChromeJa3 = "771,4865-4866-4867-49195-49199-49196-49200-52393-52392-49171-49172-156-157-47-53,0-23-65281-10-11-35-16-5-13-18-51-45-43-27-21,29-23-24,0";
        
        [Test]
        public async Task GetResponse_ShouldApplyJA3()
        {
            var response = await GoHttpRequest.Create(GoProxyUrl)
                .WithJa3(ChromeJa3)
                .WithUserAgent(ChromeUserAgent)
                .GetResponseAsync("https://ja3er.com/json");
            
            Assert.AreEqual(true, response.Success);
            Assert.IsTrue(response.Payload.Content.Contains(ChromeJa3));
        }
        
        [Test]
        public async Task GetResponse_ShouldNotApplyJA3()
        {
            var response = await GoHttpRequest.Create(GoProxyUrl)
                .WithUserAgent(ChromeUserAgent)
                .GetResponseAsync("https://ja3er.com/json");
            
            Assert.AreEqual(true, response.Success);
            Assert.IsFalse(response.Payload.Content.Contains(ChromeJa3));
        }
    }
}