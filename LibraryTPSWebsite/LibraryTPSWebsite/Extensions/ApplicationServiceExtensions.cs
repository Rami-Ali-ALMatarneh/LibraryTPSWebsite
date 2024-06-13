using LibraryTPSWebsite.Data;
using LibraryTPSWebsite.Repositories.IRepo;
using LibraryTPSWebsite.Repositories.SqlRepo;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryTPSWebsite.Extensions
    {
    public static class ApplicationServiceExtensions
        {
        public static IServiceCollection AddServiceExtensions( this IServiceCollection services, IConfiguration configuration )
            {
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionStrings"));

            });
            services.AddCors();
            services.AddScoped<IShelfRepository, SqlShelfRepository>();
            services.AddScoped<IBookRepository, SqlBookRepository>();
            return services;
            }
        }
    }
