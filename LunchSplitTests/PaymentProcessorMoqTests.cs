using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using LunchSplit;
using LunchSplit.Payments;
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

        [TestMethod]
        [TestCategory("M3-06")]
        public async Task ProcessBatchAsync_ForFourAttendees_InvokesGatewayFourTimes()
        {
            Mock<IPaymentProcessor> processor = new Mock<IPaymentProcessor>();
            processor
                .Setup(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(new PaymentResult { IsSuccess = true });

            PaymentManager manager = new PaymentManager(processor.Object);
            List<PaymentRequest> requests = new List<PaymentRequest>
            {
                new PaymentRequest(10.00m, "CAD", "m3-06-1", "cnon:card-nonce-ok"),
                new PaymentRequest(20.00m, "CAD", "m3-06-2", "cnon:card-nonce-ok"),
                new PaymentRequest(30.00m, "CAD", "m3-06-3", "cnon:card-nonce-ok"),
                new PaymentRequest(40.00m, "CAD", "m3-06-4", "cnon:card-nonce-ok")
            };

            PaymentBatchResult result = await manager.ProcessBatchAsync(requests);

            Assert.AreEqual(4, result.SuccessCount);
            processor.Verify(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Exactly(4));
        }

        [TestMethod]
        [TestCategory("M3-07")]
        public async Task ProcessBatchAsync_WhenSecondAttendeeFails_ReportsTwoSuccessesAndOneFailure()
        {
            Mock<IPaymentProcessor> processor = new Mock<IPaymentProcessor>();
            processor
                .Setup(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
                .ReturnsAsync((PaymentRequest request) => request.IdempotencyKey == "m3-07-2"
                    ? new PaymentResult { IsSuccess = false, ErrorCode = "CARD_DECLINED" }
                    : new PaymentResult { IsSuccess = true });

            PaymentManager manager = new PaymentManager(processor.Object);
            List<PaymentRequest> requests = new List<PaymentRequest>
            {
                new PaymentRequest(10.00m, "CAD", "m3-07-1", "cnon:card-nonce-ok"),
                new PaymentRequest(20.00m, "CAD", "m3-07-2", "cnon:card-nonce-declined"),
                new PaymentRequest(30.00m, "CAD", "m3-07-3", "cnon:card-nonce-ok")
            };

            PaymentBatchResult result = await manager.ProcessBatchAsync(requests);

            Assert.AreEqual(2, result.SuccessCount);
            Assert.AreEqual(1, result.FailureCount);
            Assert.AreEqual("CARD_DECLINED", result.Results[1].ErrorCode);
        }
    }
}
