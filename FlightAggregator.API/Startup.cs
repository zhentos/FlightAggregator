using FlightAggregator.Models.Models;
using FlightAggregator.Repositories.Implementation;
using FlightAggregator.Repositories.Interfaces;
using FlightAggregator.Services.Implementations;
using FlightAggregator.Services.Implementations.FlightProviders;
using FlightAggregator.Services.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FlightAggregator.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Flight Aggregator REST API",
                    Description = "Test Application",
                    TermsOfService = new Uri("https://google.com"),
                });
            });
            services.AddMemoryCache();
            services.AddScoped<IFlightsRepository, FlightsRepository>();
            services.AddScoped<IFlightProvidersService, AeroflotFlightsService>();
            services.AddScoped<IFlightProvidersService, AuroraFlightsService>();
            services.AddScoped<IFlightProvidersService, BelaviaFlightsService>();
            services.AddScoped<IFlightsAggregatorService, FlightsAggregatorService>();

            services.Configure<InMemoryCacheModel>(options => Configuration.GetSection("InMemoryCache").Bind(options));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
