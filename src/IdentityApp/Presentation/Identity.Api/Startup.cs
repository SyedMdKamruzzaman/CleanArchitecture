using System.IO.Compression;
using Identity.Api.Configs;
using Identity.Api.Extensions;
using Identity.Api.Filters;
using Identity.Api.Swagger;
using Identity.Api.Utils;
using Identity.Application.Queries.UserQueries;
using Identity.Infrastructure.Services;
using Identity.Persistence.RelationalDB;
using Identity.Persistence.RelationalDB.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.ResponseCompression;
using Swashbuckle.AspNetCore.SwaggerUI;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Identity.Api;

public static class Startup
{
    private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    private static bool InDocker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

    // This method gets called by the runtime. Use this method to add services to the container.
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ThrowIfNull(nameof(builder));

        IServiceCollection services = builder.Services;
        ////services.AddCors(options =>
        ////{
        ////    options.AddPolicy(
        ////        name: myAllowSpecificOrigins,
        ////        builder =>
        ////        {
        ////            builder
        ////            ////.WithOrigins("https://localhost:44364")
        ////            .AllowAnyOrigin()
        ////            .AllowAnyHeader()
        ////            .AllowAnyMethod();
        ////        });
        ////});

        services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins, builder =>
            {
                builder
                .WithOrigins("https://localhost:44364", "http://localhost:7300")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true);
            });
        });

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add(typeof(BadRequestResultFilter));
            options.Filters.Add(typeof(ExceptionHandlerFilter));
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        });

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        string dbConnectionString = string.Empty;

        if (InDocker)
        {
            dbConnectionString = builder.Configuration.GetConnectionString("DockerDbConnection");
        }
        else
        {
            string connectionName = builder.Environment.IsDevelopment() ? "IdentityDbConnection" : "IdentityDbConnection";
            dbConnectionString = builder.Configuration.GetConnectionString(connectionName);
        }

        services.AddIdentityDbContext(dbConnectionString);

        JwtConfig jwtConfig = new JwtConfig("SampleIdentity.com", "SampleIdentitySecretKey", 86400);
        services.AddJwtAuthentication(jwtConfig);

        services.AddJwtTokenGenerator(jwtConfig);

        services.AddExternalLogins(builder.Configuration);

        string sendGridApiKey = "yourSendGridKey";
        services.AddSendGrid(sendGridApiKey);

        services.AddMediatR(typeof(GetRefreshTokenQuery));

        services.AddServicesOfAllTypes();

        services.AddSwaggerGeneration();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void ConfigureMiddlewares(this WebApplication app)
    {
        app.ThrowIfNull(nameof(app));

        app.ApplyDatabaseMigrations();

        app.Use((context, next) =>
        {
            context.Request.EnableBuffering();
            return next();
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseApiVersioning();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocExpansion(DocExpansion.None);

            IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json", $"API {description.GroupName.ToUpperInvariant()}");
            }
        });

        app.UseResponseCompression();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors(MyAllowSpecificOrigins);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
