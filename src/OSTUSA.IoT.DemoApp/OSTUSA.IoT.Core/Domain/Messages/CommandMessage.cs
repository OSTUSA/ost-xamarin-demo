using System;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OSTUSA.IoT.Core.Domain.Messages
{
    public class CommandMessage
    {
        [JsonProperty("messageId")]
        public int MessageId { get; set; }

        [JsonProperty("command")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        public CommandType Command { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public enum CommandType
    {
        On,
        Off,
        Stop
    }
}
