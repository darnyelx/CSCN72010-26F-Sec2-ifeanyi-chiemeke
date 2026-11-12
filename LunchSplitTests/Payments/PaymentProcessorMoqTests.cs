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
    // These tests keep the payment manager offline so each gateway scenario is predictable.
    public class PaymentProcessorMoqTests
    {
        [TestMethod]
        [TestCategory("M3-03")]
        public async Task ProcessPaymentAsync_WhenGatewayTimesOut_ThrowsPaymentGatewayTimeoutException()
        {
            // The real gateway is replaced with a timeout so this test only checks our translation.
            Mock<IPaymentProcessor> processor = new Mock<IPaymentProcessor>();
            processor
                .Setup(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
                .ThrowsAsync(new HttpRequestException("Sandbox gateway timed out."));

            PaymentManager manager = new PaymentManager(processor.Object);

            // A network failure should be exposed as the application's payment exception.
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
            // Return a successful result and focus the verification on the request sent to the gateway.
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

            // Keeping the cents exact matters because this value came from a split bill.
            processor.Verify(service => service.ProcessPaymentAsync(
                It.Is<PaymentRequest>(payment => payment.Amount == 37.42m)),
                Times.Once);
        }

        [TestMethod]
        [TestCategory("M3-05")]
        public async Task ProcessPaymentAsync_ForZeroDollarRequest_NeverInvokesGateway()
        {
            // A free bill is handled locally; there is no reason to create a gateway request.
            Mock<IPaymentProcessor> processor = new Mock<IPaymentProcessor>();
            PaymentManager manager = new PaymentManager(processor.Object);

            PaymentResult result = await manager.ProcessPaymentAsync(new PaymentRequest(
                0.00m,
                "CAD",
                "m3-05-zero",
                "cnon:card-nonce-ok"));

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("ZERO_AMOUNT", result.ErrorCode);
            // Times.Never protects the short-circuit from accidentally becoming a real charge.
            processor.Verify(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("M3-06")]
        public async Task ProcessBatchAsync_ForFourAttendees_InvokesGatewayFourTimes()
        {
            // Each attendee represents a separate payment, even though they belong to one bill.
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
            // This catches both skipped attendees and accidental duplicate charges.
            processor.Verify(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Exactly(4));
        }

        [TestMethod]
        [TestCategory("M3-07")]
        public async Task ProcessBatchAsync_WhenSecondAttendeeFails_ReportsTwoSuccessesAndOneFailure()
        {
            // Make only the middle request fail so the test proves the batch continues afterward.
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
            // The failed attendee should remain visible in the result rather than stopping the batch.
            Assert.AreEqual("CARD_DECLINED", result.Results[1].ErrorCode);
        }
    }
}
