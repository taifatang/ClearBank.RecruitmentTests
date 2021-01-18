using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.PaymentSchemeValidators;

namespace ClearBank.DeveloperTest.Services
{
    /*Initiate thoughts on the project
        DataStore can be refactored to behaviour polymorphically
        A boolean value for "DataStoreType" feel more suited and less prone to typos and casing issues
        I feel EventSourcing could be used here as failed attempt are also recorded
        I also feel this may benefit from Domain Driven Design along with EventSourcing 
        Other factors to consider; race conditions, idempotency,
    */

    /*After thoughts on the project
      Making the code asynchronous may improve efficient unless there are dependencies on third party libraries
      Existing Code are broken and it's always returning False
      Instead of having a negative list of requirements, it maybe safer to have an allowed list e.g. If Scheme and Account support Bacs then deduct money.
    */

    public class PaymentService : IPaymentService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly Func<PaymentScheme, IPaymentSchemeValidator> _paymentSchemeValidator;

        public PaymentService(IAccountRepository accountRepository, Func<PaymentScheme, IPaymentSchemeValidator> paymentSchemeValidator)
        {
            _accountRepository = accountRepository;
            _paymentSchemeValidator = paymentSchemeValidator;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountRepository.GetAccount(request.DebtorAccountNumber);

            if (account == null)
            {
                //404 - account not found error
                return new MakePaymentResult() { Success = false, ErrorCode = "AccountNotFound" };
            }

            var isValid = _paymentSchemeValidator(request.PaymentScheme).IsValid(account, request);

            if (!isValid)
            {
                //402 - TakePaymentFailed 
                return new MakePaymentResult() { Success = false, ErrorCode = "PaymentSchemeUnsupported" };
            }

            account.Balance -= request.Amount;

            _accountRepository.UpdateAccount(account);

            return new MakePaymentResult() { Success = true };
        }
    }
}
