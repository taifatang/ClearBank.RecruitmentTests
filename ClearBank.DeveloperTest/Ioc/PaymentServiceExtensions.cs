using System.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.Ioc
{
    public static class PaymentServiceExtensions
    {
        public static IServiceCollection RegisterPaymentServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAccountRepository>(provider =>
            {
                var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

                if (dataStoreType == "Backup")
                {
                    return new BackupAccountDataStore();
                }

                return new AccountDataStore();
            });

            serviceCollection.AddTransient<IPaymentService, PaymentService>();

            return serviceCollection;
        }
    }
}
