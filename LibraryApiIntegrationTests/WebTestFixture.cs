using LibraryApi;
using LibraryApi.Domain;
using LibraryApi.Services;
using LibraryApiIntegrationTests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryApiIntegrationTests
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var systemTimeDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ISystemTime));

                if (systemTimeDescriptor != null)
                {
                    services.Remove(systemTimeDescriptor);
                    services.AddTransient<ISystemTime, TestingSystemTime>();
                }

                var dbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<LibraryDataContext>)
                );

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<LibraryDataContext>(options =>
                    {
                        options.UseInMemoryDatabase("JustAUniqueName");
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;

                        var db = scopedServices.GetRequiredService<LibraryDataContext>();

                        db.Database.EnsureCreated(); // basically "update-database"

                        // TODO: Add Some Data
                        DataUtils.ReinitializeDb(db);
                    }
                }
            });
        }
    }
}
            