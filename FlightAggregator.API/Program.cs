namespace FlightAggregator.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true)
               .AddCommandLine(args)
               .Build();

            return Host.CreateDefaultBuilder(args)
                         .ConfigureWebHostDefaults(webBuilder =>
                          {
                              webBuilder.UseStartup<Startup>();
                          });
        }
    }
}
