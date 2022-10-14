﻿using System.Text.Json.Serialization;

using MassTransit;

using MediatR;

using YourBrand.Inventory.Application;
using YourBrand.Inventory.Application.Common.Interfaces;
using YourBrand.Inventory.Application.Items.Commands;
using YourBrand.Inventory.Application.Items.Queries;
using YourBrand.Inventory.Infrastructure;
using YourBrand.Inventory.Infrastructure.Persistence;
using YourBrand.Inventory.Application.Items;
using YourBrand.Documents.Client;
using YourBrand.Payments.Client;
using YourBrand.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

if(args.Contains("--connection-string")) 
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = args[args.ToList().IndexOf("--connection-string") + 1];
}

builder.Services
    .AddApplication()
    .AddInfrastructure(Configuration);

builder.Services.AddControllers();

// Set the JSON serializer options
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    // options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentityServices();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Inventory API";
    c.Version = "0.1";
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    //x.AddConsumers(typeof(Program).Assembly);

    //x.AddRequestClient<IncomingTransactionBatch>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

 builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.Authority = "https://identity.local";
                        options.Audience = "myapi";

                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            NameClaimType = "name"
                            
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnTokenValidated = context =>
                            {
                                // Add the access_token as a claim, as we may actually need it
                                var accessToken = context.SecurityToken as JwtSecurityToken;
                                if (accessToken != null)
                                {
                                    ClaimsIdentity? identity = context.Principal.Identity as ClaimsIdentity;
                                    if (identity != null)
                                    {
                                        identity.AddClaim(new Claim("access_token", accessToken.RawData));
                                    }
                                }

                                return Task.CompletedTask;
                            }
                        };

                        //options.TokenValidationParameters.ValidateAudience = false;

                        //options.Audience = "openid";

                        //options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    });

        //builder.Services.AddApiKeyAuthentication("https://localhost/api/apikeys/");

builder.Services.AddDocumentsClients((sp, http) =>
{
    http.BaseAddress = new Uri($"{Configuration.GetServiceUri("nginx", "https")}/api/documents/");
});

builder.Services.AddPaymentsClients((sp, http) =>
{
    http.BaseAddress = new Uri($"{Configuration.GetServiceUri("nginx", "https")}/api/payments/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/Items/{personId}", async (string personId, IMediator mediator, CancellationToken cancellationToken)
    => await mediator.Send(new GetItem(personId), cancellationToken))
    .WithName("Items_GetItems")
    .WithTags("Items")
    .Produces<ItemDto>(StatusCodes.Status200OK);

app.MapDelete("/Items/{personId}", async (string personId, IMediator mediator, CancellationToken cancellationToken)
    => await mediator.Send(new DeleteItem(personId), cancellationToken))
    .WithName("Items_DeleteItem")
    .WithTags("Items")
    .Produces(StatusCodes.Status200OK);

app.MapPost("/Items", async (AddItem createItem, IMediator mediator, CancellationToken cancellationToken)
    => await mediator.Send(createItem, cancellationToken))
    .WithName("Items_CreateItems")
    .WithTags("Items")
    .Produces<ItemDto>(StatusCodes.Status200OK);

app.MapControllers();

if(args.Contains("--seed")) 
{
    await SeedData.EnsureSeedData(app);
    return;
}

app.Run();