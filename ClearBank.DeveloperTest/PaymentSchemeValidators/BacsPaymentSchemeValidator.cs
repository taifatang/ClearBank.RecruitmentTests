using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentSchemeValidators
{
    public class BacsPaymentSchemeValidator : IPaymentSchemeValidator
    {
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            var isValid = true;

            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
