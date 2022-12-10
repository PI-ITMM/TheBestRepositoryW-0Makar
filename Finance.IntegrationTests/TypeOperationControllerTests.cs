using Finance.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Finance.IntegrationTests
{
    [TestFixture]
    public class TypeOperationControllerTests
    {
        private readonly List<TypeOperation> testValueTypeOperations = new()
        {
            new TypeOperation
            {
                Name = "buy",
                IsIncome = false
            },
            new TypeOperation
            {
                Name = "sold",
                IsIncome = true
            },
            new TypeOperation
            {
                Name = "buy TV",
                IsIncome = false
            }
        };

        [Test]
        public async Task AddTypeOperation_OK()
        {
            var webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<FinanceContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<FinanceContext>(options =>
                    {
                        options.UseInMemoryDatabase("financeDbAddType1");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            var typeOperation = new TypeOperation
            {
                Name = "sold",
                IsIncome = true
            };
            var htppContent = new StringContent(JsonSerializer.Serialize<TypeOperation>(typeOperation), Encoding.UTF8, "application/json");

            var responce = await httpClient.PostAsync("api/typeoperation/", htppContent);

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task AddTypeOperation_BadRequest()
        {
            var webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<FinanceContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<FinanceContext>(options =>
                    {
                        options.UseInMemoryDatabase("financeDbAddType2");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            var typeOperation = new TypeOperation
            {
                IsIncome = true
            };
            var htppContent = new StringContent(JsonSerializer.Serialize<TypeOperation>(typeOperation), Encoding.UTF8, "application/json");

            var responce = await httpClient.PostAsync("api/typeoperation/", htppContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Test]
        public async Task GetAllTypeOperation_OK()
        {
            var webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<FinanceContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<FinanceContext>(options =>
                    {
                        options.UseInMemoryDatabase("financeDbGetType1");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            
            var responce = await httpClient.GetAsync("api/typeoperation/");

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task GetByBoolTypeOperation_OK()
        {
            var webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<FinanceContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<FinanceContext>(options =>
                    {
                        options.UseInMemoryDatabase("financeDbGetType2");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var responce = await httpClient.GetAsync("api/typeoperation/true");

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task EditTypeOperation_OK()
        {
            var webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<FinanceContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<FinanceContext>(options =>
                    {
                        options.UseInMemoryDatabase("financeDbEditType1");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            var typeOperation = new TypeOperation
            {
                TypeOperationId = 3,
                Name = "buy sofa",
                IsIncome = false
            };
            var htppContent = new StringContent(JsonSerializer.Serialize<TypeOperation>(typeOperation), Encoding.UTF8, "application/json");

            var responce = await httpClient.PutAsync("api/typeoperation/", htppContent);

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task EditTypeOperation_NotFound()
        {
            var webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<FinanceContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<FinanceContext>(options =>
                    {
                        options.UseInMemoryDatabase("financeDbEditType2");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            var typeOperation = new TypeOperation
            {
                TypeOperationId = 5,
                Name = "buy",
                IsIncome = false
            };
            var htppContent = new StringContent(JsonSerializer.Serialize<TypeOperation>(typeOperation), Encoding.UTF8, "application/json");

            var responce = await httpClient.PutAsync("api/typeoperation/", htppContent);

            Assert.AreEqual(HttpStatusCode.NotFound, responce.StatusCode);
        }

        [Test]
        public async Task DeleteTypeOperation_OK()
        {
            var webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<FinanceContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<FinanceContext>(options =>
                    {
                        options.UseInMemoryDatabase("financeDbDeleteType1");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var responce = await httpClient.DeleteAsync("api/typeoperation/3");

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task DeleteTypeOperation_NotFound()
        {
            var webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<FinanceContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<FinanceContext>(options =>
                    {
                        options.UseInMemoryDatabase("financeDbDeleteType2");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var responce = await httpClient.DeleteAsync("api/typeoperation/4");

            Assert.AreEqual(HttpStatusCode.NotFound, responce.StatusCode);
        }
    }
}
