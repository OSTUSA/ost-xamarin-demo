using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using OSTUSA.XamarinDemo.Core.Domain.Things;

namespace OSTUSA.XamarinDemo.Services.Azure
{
    public class ThingsService : IThingsService
    {
        private const string TwinMethodFormat = "things/{0}/twin";

        private readonly AzureClient _client;
        
        public ThingsService()
        {
            _client = new AzureClient();
        }

        public async Task<Twin> GetThingTwinAsync(string thingId)
        {
            return await _client.GetAsync<Twin>(string.Format(TwinMethodFormat, thingId));
        }
    }
}
