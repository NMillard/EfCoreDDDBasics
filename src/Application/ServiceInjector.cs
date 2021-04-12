using Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Application {
    public static class ServiceInjector {
        public static IServiceCollection AddQueries(this IServiceCollection services) {
            services.AddTransient<IGetAuthorsQuery, GetAuthorsQuery>()
                .AddTransient<IGetSimpleBooksQuery, SimpleBooksQuery>();
            
            return services;
        }
    }
}