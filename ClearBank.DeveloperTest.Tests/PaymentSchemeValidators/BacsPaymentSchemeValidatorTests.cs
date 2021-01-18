using ClearBank.DeveloperTest.PaymentSchemeValidators;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.PaymentSchemeValidators
{
    [TestFixture]
    public class BacsPaymentSchemeValidatorTests
    {
        [TestCase(AllowedPaymentSchemes.Bacs)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps)]
        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        public void IsValid_ReturnTrue_ForBacsPayment(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var validator = new BacsPaymentSchemeValidator();

            var result = validator.IsValid(new Account() {AllowedPaymentSchemes = allowedPaymentSchemes }, new MakePaymentRequest());

            Assert.IsTrue(result);
        }

        [TestCase(AllowedPaymentSchemes.Chaps)]
        [TestCase(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        public void IsValid_ReturnFalse_ForNonBacsPayment(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var validator = new BacsPaymentSchemeValidator();

            var result = validator.IsValid(new Account() { AllowedPaymentSchemes = allowedPaymentSchemes }, new MakePaymentRequest());

            Assert.IsFalse(result);
        }
    }
}
