namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountStatus Status { get; set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }

        /* Maybe something similar to this in the future?

            //public void AuthoriseAndCapture()
            //{
            //  Append(New PaymentAuthorisedEvent() { Amount = 10m })
            //}

            //public void Transition(PaymentAuthorisedEvent event)
            //{
            //  Amount = event.Amount
            //}

        */
    }
}
