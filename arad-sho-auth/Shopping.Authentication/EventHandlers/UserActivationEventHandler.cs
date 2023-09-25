using System.Threading.Tasks;
using MassTransit;
using Shopping.Authentication.Interfaces;
using Shopping.Shared.Events.Interfaces.Users;

namespace Shopping.Authentication.EventHandlers
{
    public class UserActivationEventHandler
        : IConsumer<IDeActiveUserEvent>
    , IConsumer<IActiveUserEvent>
    {
        private readonly IAppRepository _appRepository;
        public UserActivationEventHandler()
        {
            _appRepository = Bootstrapper.WindsorContainer.Resolve<IAppRepository>();
        }
        public async Task Consume(ConsumeContext<IDeActiveUserEvent> context)
        {
            var message = context.Message;
            await _appRepository.DeActiveUser(message.UserId.ToString(), message.AppType);
        }
        public async Task Consume(ConsumeContext<IActiveUserEvent> context)
        {
            var message = context.Message;
            await _appRepository.ActiveUser(message.UserId.ToString(), message.AppType);
        }
    }
}