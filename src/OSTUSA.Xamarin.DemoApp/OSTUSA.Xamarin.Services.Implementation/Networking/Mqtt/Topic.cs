using System;
namespace OSTUSA.XamarinDemo.Services.Networking.Mqtt
{
    public class Topic
    {
        private readonly string _base;

        public Topic(string baseTopic)
        {
            _base = baseTopic;
        }

        public string Accepted { get { return string.Format("{0}/accepted", _base); } }
        public string Rejected { get { return string.Format("{0}/rejected", _base); } }

        public static implicit operator string(Topic topic)
        {
            return topic._base;
        }
    }
}

