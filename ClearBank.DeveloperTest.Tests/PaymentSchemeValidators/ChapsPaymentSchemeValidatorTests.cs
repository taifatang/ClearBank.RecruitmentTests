using ClearBank.DeveloperTest.PaymentSchemeValidators;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.PaymentSchemeValidators
{
    [TestFixture]
    public class ChapsPaymentSchemeValidatorTests
    {
        [TestCase(AllowedPaymentSchemes.Chaps)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        public void IsValid_ReturnTrue_ForChapsPayment(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var validator = new ChapsPaymentSchemeValidator();
            var account = new Account()
            {
                AllowedPaymentSchemes = allowedPaymentSchemes,
                Status = AccountStatus.Live
            };

            var result = validator.IsValid(account, new MakePaymentRequest());

            Assert.IsTrue(result);
        }

        [TestCase(AllowedPaymentSchemes.Bacs)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments)]
        public void IsValid_ReturnFalse_ForNonChapsPayment(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var validator = new ChapsPaymentSchemeValidator();

            var result = validator.IsValid(new Account() { AllowedPaymentSchemes = allowedPaymentSchemes }, new MakePaymentRequest());

            Assert.IsFalse(result);
        }

        [TestCase(AccountStatus.Disabled)]
        [TestCase(AccountStatus.InboundPaymentsOnly)]
        public void IsValid_ReturnFalse_ForLimitedAccount(AccountStatus accountStatus)
        {
            var validator = new ChapsPaymentSchemeValidator();
            var account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = accountStatus
            };

            var result = validator.IsValid(account, new MakePaymentRequest());

            Assert.IsFalse(result);
        }
    }
}