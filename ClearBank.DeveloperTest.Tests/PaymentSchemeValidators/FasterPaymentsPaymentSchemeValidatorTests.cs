using ClearBank.DeveloperTest.PaymentSchemeValidators;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.PaymentSchemeValidators
{
    [TestFixture]
    public class FasterPaymentsPaymentSchemeValidatorTests
    {
        [TestCase(AllowedPaymentSchemes.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        public void IsValid_ReturnTrue_ForFasterPaymentsPayment(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var validator = new FasterPaymentsPaymentSchemeValidator();

            var result = validator.IsValid(new Account() { AllowedPaymentSchemes = allowedPaymentSchemes }, new MakePaymentRequest());

            Assert.IsTrue(result);
        }

        [TestCase(AllowedPaymentSchemes.Bacs)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps)]
        public void IsValid_ReturnFalse_ForNonFasterPaymentsPayment(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var validator = new FasterPaymentsPaymentSchemeValidator();

            var result = validator.IsValid(new Account() { AllowedPaymentSchemes = allowedPaymentSchemes }, new MakePaymentRequest());

            Assert.IsFalse(result);
        }

        [TestCase(10,5)]
        [TestCase(0.5,0.4)]
        [TestCase(0,0)]
        public void IsValid_ReturnTrue_ForSufficientBalance(decimal balance, decimal requestedAmount)
        {
            var validator = new FasterPaymentsPaymentSchemeValidator();
            var account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = balance
            };
            var paymentRequest = new MakePaymentRequest()
            {
                Amount = requestedAmount
            };

            var result = validator.IsValid(account, paymentRequest);

            Assert.True(result);
        }

        [TestCase(1, 2)]
        [TestCase(0, 1)]
        [TestCase(-1, 0)]
        [TestCase(-1, 1)]
        public void IsValid_ReturnFalse_ForInsufficientBalance(decimal balance, decimal requestedAmount)
        {
            var validator = new FasterPaymentsPaymentSchemeValidator();
            var account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = balance
            };
            var paymentRequest = new MakePaymentRequest()
            {
                Amount = requestedAmount
            };

            var result = validator.IsValid(account, paymentRequest);

            Assert.IsFalse(result);
        }
    }
}