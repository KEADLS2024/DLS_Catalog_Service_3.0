using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using DLS_Catalog_Service.Repositories;
using DLS_Catalog_Service.Model;

var builder = WebApplication.CreateBuilder(args);

// RabbitMQ Configuration
var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ");
var factory = new ConnectionFactory()
{
    HostName = rabbitMQConfig["HostName"],
    UserName = rabbitMQConfig["UserName"],
    Password = rabbitMQConfig["Password"]
};
var connection = factory.CreateConnection();
builder.Services.AddSingleton<IConnection>(connection);

// MongoDB Configuration
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddScoped<MongoDbContext>();

// Register repositories with RabbitMQ dependency
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>(provider =>
    new CatalogRepository(
        provider.GetRequiredService<MongoDbContext>(),
        provider.GetRequiredService<IConnection>()
    )
);
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(provider =>
    new CategoryRepository(
        provider.GetRequiredService<MongoDbContext>(),
        provider.GetRequiredService<IConnection>()
    )
);

// Registering Product and ProductDetail repositories with RabbitMQ support
builder.Services.AddScoped<IProductRepository, ProductRepository>(provider =>
    new ProductRepository(provider.GetRequiredService<MongoDbContext>(), provider.GetRequiredService<IConnection>())
);
builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>(provider =>
    new ProductDetailRepository(provider.GetRequiredService<MongoDbContext>(), provider.GetRequiredService<IConnection>())
);

// Add controllers and other required services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline if in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.Run();
