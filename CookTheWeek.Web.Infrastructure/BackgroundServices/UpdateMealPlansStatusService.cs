namespace CookTheWeek.Web.Infrastructure.BackgroundServices
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;

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
                    var dbContext = scope.ServiceProvider.GetRequiredService<CookTheWeekDbContext>();

                    try
                    {
                        ICollection<MealPlan> mealPlans = await dbContext
                            .MealPlans
                            .Where(mp => !mp.IsFinished || mp.Meals.Any(m => !m.IsCooked))
                            .ToListAsync(stoppingToken); // Pass the cancellation token

                        foreach (var mealPlan in mealPlans)
                        {
                            if (mealPlan.StartDate.AddDays(6) < DateTime.Today)
                            {
                                mealPlan.IsFinished = true;

                                foreach (var meal in mealPlan.Meals)
                                {
                                    meal.IsCooked = true;
                                }
                            }
                        }

                        await dbContext.SaveChangesAsync(stoppingToken); // Pass the cancellation token
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred and meal plans status was not updated");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Delay and pass the cancellation token
            }
        }


    }
}
