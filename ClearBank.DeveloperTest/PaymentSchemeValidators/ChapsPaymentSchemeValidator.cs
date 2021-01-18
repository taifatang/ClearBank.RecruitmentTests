using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentSchemeValidators
{
    public class ChapsPaymentSchemeValidator : IPaymentSchemeValidator
    {
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            var isValid = true;

            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            {
                isValid = false;
            }
            else if (account.Status != AccountStatus.Live)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}