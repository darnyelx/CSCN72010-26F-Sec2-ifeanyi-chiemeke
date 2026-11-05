using System.Net.Http;
using System.Threading.Tasks;
using LunchSplit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LunchSplitTests
{
    [TestClass]
    public class PaymentProcessorMoqTests
    {
        [TestMethod]
        [TestCategory("M3-03")]
        public async Task ProcessPaymentAsync_WhenGatewayTimesOut_ThrowsPaymentGatewayTimeoutException()
        {
            Mock<IPaymentProcessor> processor = new Mock<IPaymentProcessor>();
            processor
                .Setup(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
                .ThrowsAsync(new HttpRequestException("Sandbox gateway timed out."));

            PaymentManager manager = new PaymentManager(processor.Object);

            await Assert.ThrowsExactlyAsync<PaymentGatewayTimeoutException>(async () =>
                await manager.ProcessPaymentAsync(new PaymentRequest(
                    25.00m,
                    "CAD",
                    "m3-03-timeout",
                    "cnon:card-nonce-ok")));
        }
    }
}
