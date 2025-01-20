namespace CookTheWeek.Services.Data.Events.Dispatchers
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Services.Data.Events.EventHandlers;

    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DomainEventDispatcher> logger;

        public DomainEventDispatcher(IServiceProvider serviceProvider,
            ILogger<DomainEventDispatcher> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public async Task DispatchAsync<TEvent>(TEvent domainEvent)
        {
            var handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();

            if (!handlers.Any())
            {
                logger.LogError($"No handlers found for event type {typeof(TEvent).Name}");
                return;
            }

            foreach (var handler in handlers)
            {
                logger.LogInformation($"Found handler: {handler.GetType().Name}");
               
                await handler.HandleAsync(domainEvent);
               
            }
        }
    }
}
