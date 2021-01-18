using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    /*Initiate thoughts on the project
        DataStore can be refactored to behaviour polymorphically
        A boolean value for "DataStoreType" feel more suited and less prone to typos and casing issues
        I feel EventSourcing could be used here as failed attempt are also recorded
        I also feel this may benefit from Domain Driven Design along with EventSourcing 
        Other factors to consider; race conditions, idempotency,
    */
    public class PaymentService : IPaymentService
    {
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            Account account = null;

            if (dataStoreType == "Backup")
            {
                var accountDataStore = new BackupAccountDataStore();
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }
            else
            {
                var accountDataStore = new AccountDataStore();
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }

            var result = new MakePaymentResult();

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        result.Success = false;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        result.Success = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        result.Success = false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        result.Success = false;
                    }
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;

                if (dataStoreType == "Backup")
                {
                    var accountDataStore = new BackupAccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
                else
                {
                    var accountDataStore = new AccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
            }

            return result;
        }
    }
}
