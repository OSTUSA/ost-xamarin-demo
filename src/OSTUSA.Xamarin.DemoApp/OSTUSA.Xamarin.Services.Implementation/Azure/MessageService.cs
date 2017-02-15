using System;
using System.Threading.Tasks;
using OSTUSA.XamarinDemo.Core.Domain.Messages;
using System.Net.Http;
using System.Text;

namespace OSTUSA.XamarinDemo.Services.Azure
{
    public class MessageService : IMessageService
    {
        private const string BaseUrl = "https://ostiotdemo.azurewebsites.net/api/";
        private const string MessagesMethodFormat = "things/{0}/messages";
        private const string ThingId = "myraspberrypi";

        private readonly HttpClient _httpClient;

        public MessageService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseUrl) };
        }

        public async Task SendMessageAsync(CommandMessage commandMessage)
        {
            var method = string.Format(MessagesMethodFormat, ThingId);
            await _httpClient.PostAsync(method, new StringContent(commandMessage.ToString(), Encoding.UTF8, "application/json"));
        }
    }
}
