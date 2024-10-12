namespace CookTheWeek.Web.Infrastructure.BackgroundServices
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Services.Data.Services.Interfaces;

    public class UpdateMealPlansStatusService : BackgroundService
    {
        private readonly ILogger<UpdateMealPlansStatusService> logger;
        private readonly IServiceProvider serviceProvider;

        public UpdateMealPlansStatusService(IServiceProvider serviceProvider, 
            ILogger<UpdateMealPlansStatusService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var mealPlanService = scope.ServiceProvider.GetRequiredService<IMealPlanService>();

                    try
                    {
                        await mealPlanService.UpdateMealPlansStatusAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while updating meal plans` status.");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Delay and pass the cancellation token
            }
        }


    }
}
