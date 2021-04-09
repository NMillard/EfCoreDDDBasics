using System;
using System.Collections.Generic;
using System.IO;
using Application;
using Application.Interfaces.Repositories;
using Application.Queries;
using DataLayer;
using Domain.GoodExamples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/*
 * 1.   Load required application variables from some storage.
 *      Locally, you should prefer to use User Secrets, and
 *      appsettings.json for non-sensitive settings.
 *
 * 2.   Construct the dependency container and register all
 *      required services exposed by service injectors.
 *
 * 3.   Grab the needed services and do stuff.
 */
IConfigurationRoot configs = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddUserSecrets("ebc731ae-5a5e-4978-a9fa-15fbf8febd26")
    .Build();

IServiceCollection container = new ServiceCollection()
    .AddAppContext(configs.GetConnectionString("sql"))
    .AddQueries()
    .AddRepositories();

ServiceProvider provider = container.BuildServiceProvider();

var context = provider.GetRequiredService<IAppContext>();
var authorQuery = provider.GetRequiredService<IGetAuthorsQuery>();
var booksQuery = provider.GetRequiredService<IGetSimpleBooksQuery>();

bool result = await context.CanConnectAsync();
if (!result) throw new InvalidOperationException();

IEnumerable<Author> authors = await authorQuery.ExecuteAsync();
foreach (Author author in authors) Console.WriteLine($"{author.Name} has {author.Books.Count.ToString()} books");

IEnumerable<ISimpleBook> simpleBooks = await booksQuery.ExecuteAsync(); 

Console.WriteLine("Application exiting...");