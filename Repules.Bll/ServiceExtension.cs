using Repules.Bll;
using Repules.Bll.Managers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddBll(this IServiceCollection services)
        {
            services.AddTransient<IFlightService, FlightService>();
            services.AddTransient<IAirportService, AirportService>();
            services.AddTransient<IFlightLogFileService, FlightLogFileService>();
            services.AddTransient<IGPSRecordService, GPSRecordService>();
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddTransient<AirportManager>();
            services.AddTransient<FlightLogFileManager>();
            services.AddTransient<FlightManager>();
            return services;
        }
    }
}
