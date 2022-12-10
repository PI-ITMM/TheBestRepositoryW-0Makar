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
    public class OperationControllerTests
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
        private readonly List<FinanceOperation> testValueFinanceOperations = new()
        {
            new FinanceOperation
            {
                Value = 15,
                Data = "2010-02-01T12:10:24",
                TypeOperationId = 1
            },
            new FinanceOperation
            {
                Value = 20,
                Data = "2010-02-02T12:10:24",
                TypeOperationId = 2
            },
            new FinanceOperation
            {
                Value = 30,
                Data = "2010-02-03T12:10:24",
                TypeOperationId = 2
            }
        };

        [Test]
        public async Task AddOperation_OK()
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
                        options.UseInMemoryDatabase("financeDbAdd1");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            var financeOperation = new FinanceOperation
            {
                Value = 30,
                Data = "2010-02-03T12:10:24",
                TypeOperationId = 3
            };
            var htppContent = new StringContent(JsonSerializer.Serialize<FinanceOperation>(financeOperation), Encoding.UTF8, "application/json");

            var responce = await httpClient.PostAsync("api/operation/", htppContent);

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task AddOperation_BadRequest()
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
                        options.UseInMemoryDatabase("financeDbAdd2");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            var financeOperation = new FinanceOperation
            {
                Value = 30,
                TypeOperationId = 3
            };
            var htppContent = new StringContent(JsonSerializer.Serialize<FinanceOperation>(financeOperation), Encoding.UTF8, "application/json");

            var responce = await httpClient.PostAsync("api/operation/", htppContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Test]
        public async Task GetAllOperation_OK()
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
                        options.UseInMemoryDatabase("financeDbGet1");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var responce = await httpClient.GetAsync("api/operation/");

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task GetByDataOperation_OK()
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
                        options.UseInMemoryDatabase("financeDbGet2");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var responce = await httpClient.GetAsync("api/operation/2010-02-02T12:10:24");

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task GetByPeriodOperation_OK()
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
                        options.UseInMemoryDatabase("financeDbGet3");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var responce = await httpClient.GetAsync("api/operation/2010-02-02T12:10:24/2012-02-03T12:10:24");

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task EditOperation_OK()
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
                        options.UseInMemoryDatabase("financeDbEdit1");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            var financeOperation = new FinanceOperation
            {
                FinanceOperationId = 3,
                Value = 30,
                Data = "2010-02-03T12:10:24",
                TypeOperationId = 2
            };
            var htppContent = new StringContent(JsonSerializer.Serialize<FinanceOperation>(financeOperation), Encoding.UTF8, "application/json");

            var responce = await httpClient.PutAsync("api/operation/", htppContent);

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task EditOperation_NotFound()
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
                        options.UseInMemoryDatabase("financeDbEdit2");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            var financeOperation = new FinanceOperation
            {
                FinanceOperationId = 5,
                Value = 30,
                Data = "2010-02-03T12:10:24",
                TypeOperationId = 2
            };
            var htppContent = new StringContent(JsonSerializer.Serialize<FinanceOperation>(financeOperation), Encoding.UTF8, "application/json");

            var responce = await httpClient.PutAsync("api/operation/", htppContent);

            Assert.AreEqual(HttpStatusCode.NotFound, responce.StatusCode);
        }

        [Test]
        public async Task DeleteOperation_OK()
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
                        options.UseInMemoryDatabase("financeDbDelete1");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var responce = await httpClient.DeleteAsync("api/operation/3");

            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }

        [Test]
        public async Task DeleteOperation_NotFound()
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
                        options.UseInMemoryDatabase("financeDbDelete2");
                    });
                });
            });
            var dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<FinanceContext>();
            await dbContext.TypeOperations.AddRangeAsync(testValueTypeOperations);
            await dbContext.Operations.AddRangeAsync(testValueFinanceOperations);
            await dbContext.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var responce = await httpClient.DeleteAsync("api/operation/4");

            Assert.AreEqual(HttpStatusCode.NotFound, responce.StatusCode);
        }
    }
}
