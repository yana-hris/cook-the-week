namespace CookTheWeek.Web.Infrastructure.HostedServices
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;

    public class WarmUpService : IHostedService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public WarmUpService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var apiEndpoint = "https://localhost:7279/api/health";

                var response = await client.GetAsync(apiEndpoint, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error warming up API: {ex.Message}");
                // Log or handle the exception as needed
            }
        }
        public Task StopAsync(CancellationToken cancellationToken) 
            => Task.CompletedTask;
    }
}
