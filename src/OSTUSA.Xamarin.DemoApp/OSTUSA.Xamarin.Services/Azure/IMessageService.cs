using System;
using System.Threading.Tasks;
using OSTUSA.XamarinDemo.Core.Domain.Messages;

namespace OSTUSA.XamarinDemo.Services.Azure
{
    public interface IMessageService
    {
        Task SendMessageAsync(CommandMessage commandMessage);
    }
}
