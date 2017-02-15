using System;
using System.Threading.Tasks;
using OSTUSA.IoT.Core.Domain.Messages;

namespace OSTUSA.IoT.Services.Azure
{
    public interface IMessageService
    {
        Task SendMessageAsync(CommandMessage commandMessage);
    }
}
