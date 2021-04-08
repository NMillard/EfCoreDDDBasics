﻿using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataLayer {
    /*
     * Enable standalone use of our db context.
     * This makes it possible to make migrations and sql scripts without
     * having to build and run the main startup application.
     *
     * e.g we can now run 'dotnet ef migrations add' without specifying -s and -p.
     */
    internal class SqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {
        public AppDbContext CreateDbContext(string[] args) {
            IConfigurationRoot configs = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddUserSecrets(typeof(SqlDesignTimeDbContextFactory).Assembly)
                .Build();
            
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlServer(configs.GetConnectionString("sql"))
                .Options;

            return new AppDbContext(options);
        }
    }
}