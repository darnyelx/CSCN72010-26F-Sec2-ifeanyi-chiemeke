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

        [TestMethod]
        [TestCategory("M3-04")]
        public async Task ProcessPaymentAsync_ForwardsTheExactDollarAmount()
        {
            Mock<IPaymentProcessor> processor = new Mock<IPaymentProcessor>();
            processor
                .Setup(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(new PaymentResult { IsSuccess = true });

            PaymentManager manager = new PaymentManager(processor.Object);
            PaymentRequest request = new PaymentRequest(
                37.42m,
                "CAD",
                "m3-04-amount",
                "cnon:card-nonce-ok");

            await manager.ProcessPaymentAsync(request);

            processor.Verify(service => service.ProcessPaymentAsync(
                It.Is<PaymentRequest>(payment => payment.Amount == 37.42m)),
                Times.Once);
        }

        [TestMethod]
        [TestCategory("M3-05")]
        public async Task ProcessPaymentAsync_ForZeroDollarRequest_NeverInvokesGateway()
        {
            Mock<IPaymentProcessor> processor = new Mock<IPaymentProcessor>();
            PaymentManager manager = new PaymentManager(processor.Object);

            PaymentResult result = await manager.ProcessPaymentAsync(new PaymentRequest(
                0.00m,
                "CAD",
                "m3-05-zero",
                "cnon:card-nonce-ok"));

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("ZERO_AMOUNT", result.ErrorCode);
            processor.Verify(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Never);
        }
    }
}
