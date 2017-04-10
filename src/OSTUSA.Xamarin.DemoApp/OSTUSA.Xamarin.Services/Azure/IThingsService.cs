using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OSTUSA.XamarinDemo.Core.Domain.Things;
namespace OSTUSA.XamarinDemo.Services.Azure
{
    public interface IThingsService
    {
        Task<Twin> GetThingTwinAsync(string thingId);
    }
}
