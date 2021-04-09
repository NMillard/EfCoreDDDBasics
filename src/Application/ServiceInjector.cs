using Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Application {
    public static class ServiceInjector {
        public static IServiceCollection AddQueries(this IServiceCollection services) {
            services.AddScoped<IGetAuthorsQuery, GetAuthorsQuery>();
            services.AddScoped<IGetSimpleBooksQuery, SimpleBooksQuery>();
            
            return services;
        }
    }
}