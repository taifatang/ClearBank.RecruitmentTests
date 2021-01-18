using System;

namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentRequest
    {
        public string CreditorAccountNumber { get; set; }

        public string DebtorAccountNumber { get; set; }

        public decimal Amount { get; set; }
        //Wondering if this should be set at the time of execution internally?
        public DateTime PaymentDate { get; set; }

        public PaymentScheme PaymentScheme { get; set; }
    }
}
