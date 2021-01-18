using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentSchemeValidators
{
    public interface IPaymentSchemeValidator
    {
        bool IsValid(Account account, MakePaymentRequest request);
    }
}