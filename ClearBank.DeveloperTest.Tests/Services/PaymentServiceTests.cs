using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.PaymentSchemeValidators;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IPaymentSchemeValidator> _paymentSchemeValidatorMock;
        private PaymentService _paymentService;
        private Account _account;

        [SetUp]
        public void SetUp()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _paymentSchemeValidatorMock = new Mock<IPaymentSchemeValidator>();

            _paymentService = new PaymentService(_accountRepositoryMock.Object, _ => _paymentSchemeValidatorMock.Object);

            _account = new Account()
            {
                AccountNumber = Guid.NewGuid().ToString()
            };

            _accountRepositoryMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(_account);
            _paymentSchemeValidatorMock.Setup(x => x.IsValid(It.IsAny<Account>(), It.IsAny<MakePaymentRequest>()))
                .Returns(true);
        }

        [Test]
        public void MakePayment()
        {
            var paymentResult = _paymentService.MakePayment(new MakePaymentRequest());

            Assert.IsTrue(paymentResult.Success);
        }

        [Test]
        public void MakePayment_ReturnsFalse_ForAccountNotFound()
        {
            _accountRepositoryMock.Setup(x => x.GetAccount(It.IsAny<string>())).Returns((Account)null);
            var request = new MakePaymentRequest()
            {
                DebtorAccountNumber = Guid.NewGuid().ToString()
            };

            var paymentResult = _paymentService.MakePayment(request);

            Assert.IsFalse(paymentResult.Success);
            Assert.That(paymentResult.ErrorCode, Is.EqualTo("AccountNotFound"));
            _accountRepositoryMock.Verify(x => x.GetAccount(request.DebtorAccountNumber), Times.Once);
        }

        [Test]
        public void MakePayment_ReturnsFalse_ForUnsupportedPaymentScheme()
        {
            _paymentSchemeValidatorMock.Setup(x => x.IsValid(It.IsAny<Account>(), It.IsAny<MakePaymentRequest>()))
                .Returns(false);

            var paymentResult = _paymentService.MakePayment(new MakePaymentRequest());

            Assert.IsFalse(paymentResult.Success);
            Assert.That(paymentResult.ErrorCode, Is.EqualTo("PaymentUnsupported"));
        }

        [TestCase(10, 5, 5)]
        [TestCase(10, 0, 10)]
        [TestCase(0, 10, -10)]
        [TestCase(0.10, 0.05, 0.05)]
        [TestCase(-1, 10, -11)]

        //probably should not be allowed to request negative amount?
        [TestCase(10, -5, 15)]
        public void MakePayment_DeductBalance_ForSuccessfulPayment(decimal balance, decimal requestedAmount, decimal expectedAmount)
        {
            _account.Balance = balance;
            var request = new MakePaymentRequest() { Amount = requestedAmount };

            var paymentResult = _paymentService.MakePayment(request);

            Assert.IsTrue(paymentResult.Success);
            Assert.That(_account.Balance, Is.EqualTo(expectedAmount));
            _accountRepositoryMock.Verify(x => x.UpdateAccount(_account), Times.Once);
        }
    }
}
