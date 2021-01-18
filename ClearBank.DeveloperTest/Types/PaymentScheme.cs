namespace ClearBank.DeveloperTest.Types
{
    public enum PaymentScheme
    {
        //With an Enum there is a risk of defaulting to "FasterPayments" if the consumer forgot to supply it.
        FasterPayments,
        Bacs,
        Chaps
    }
}
