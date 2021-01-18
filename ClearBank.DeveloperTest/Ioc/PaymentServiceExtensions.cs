using System;
using System.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.PaymentSchemeValidators;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.Ioc
{
    public static class PaymentServiceExtensions
    {
        public static IServiceCollection RegisterPaymentServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<BacsPaymentSchemeValidator>();
            serviceCollection.AddSingleton<ChapsPaymentSchemeValidator>();
            serviceCollection.AddSingleton<FasterPaymentsPaymentSchemeValidator>();

            serviceCollection.AddSingleton<Func<PaymentScheme, IPaymentSchemeValidator>>(provider => scheme =>
            {
                return scheme switch
                {
                    PaymentScheme.Bacs =>  provider.GetRequiredService<BacsPaymentSchemeValidator>(),
                    PaymentScheme.Chaps => provider.GetRequiredService<ChapsPaymentSchemeValidator>(),
                    PaymentScheme.FasterPayments => provider.GetRequiredService<FasterPaymentsPaymentSchemeValidator>(),
                    _ => throw new NotImplementedException($"Unable to find IPaymentSchemeValidator implementation for {scheme}")
                };
            });

            serviceCollection.AddSingleton<IAccountRepository>(provider =>
            {
                var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

                if (dataStoreType == "Backup")
                {
                    return new BackupAccountDataStore();
                }

                return new AccountDataStore();
            });

            serviceCollection.AddSingleton<IPaymentService, PaymentService>();

            return serviceCollection;
        }
    }
}
