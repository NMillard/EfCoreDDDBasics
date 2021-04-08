using Application.Interfaces.Repositories;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer {
    public static class ServiceInjector {
        
        /*
         * Register our db context with the DI container, without exposing the concrete AppDbContext class.
         */
        public static IServiceCollection AddAppContext(this IServiceCollection services, string connectionString) {
            services.AddDbContext<AppDbContext>(builder => {
                builder.UseSqlServer(connectionString);
            });

            /*
             * You typically don't want to expose the db context, not even thru an interface.
             * But, if you have to, then make sure you'll only expose the very minimum functionality
             * required by clients.
             */
            services.AddScoped<IAppContext, AppDbContext>();
            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services) {
            services.AddScoped<IAuthorRepository, SqlAuthorRepository>();
            
            return services;
        }
    }
}