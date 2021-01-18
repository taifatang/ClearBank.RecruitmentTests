using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentSchemeValidators
{
    public class FasterPaymentsPaymentSchemeValidator : IPaymentSchemeValidator
    {
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            var isValid = true;

            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            {
                isValid = false;
            }
            else if (account.Balance < request.Amount)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}