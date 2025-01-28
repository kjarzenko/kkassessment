using KKBundle.API.Domains.Pricing;
using KKBundle.API.Domains.Providers;

using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProviderFactory,ProviderFactory>();
builder.Services.AddScoped<IPricingService, PricingService>();

var section = builder.Configuration.GetSection(ProviderTopicsOptions.OptionsName);

builder.Services.Configure<ProviderTopicsOptions>(o =>
{
    var dictionary = section.GetChildren()
        .ToDictionary(k => k.Key, v => v.Value);

    o.ProviderTopics = dictionary;
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
