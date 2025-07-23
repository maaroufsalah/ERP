using System.Reflection;
using ERP.Application.Interfaces.IServices;
using ERP.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            // ✅ AutoMapper configuration comme dans Ekygai
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // ✅ Service registrations (à ajouter quand vous créerez vos services)
            services.AddScoped<IProductService, ProductService>();
            // services.AddScoped<IOrderService, OrderService>();
            // services.AddScoped<ICustomerService, CustomerService>();

            // ✅ FluentValidation: Registers all validators automatically
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}