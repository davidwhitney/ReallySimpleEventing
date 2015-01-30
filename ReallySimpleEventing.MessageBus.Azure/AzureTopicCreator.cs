﻿using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ReallySimpleEventing.MessageBus.Azure
{
    public class AzureTopicCreator : ITopicCreator
    {
        private readonly AzureServiceBusConfiguration _config;
        private readonly NamespaceManager _namespaceManager;

        public AzureTopicCreator(AzureServiceBusConfiguration config)
        {
            _config = config;
            var settings = new NamespaceManagerSettings
            {
                //TokenProvider = new TokenProvider()
            };
            var nsm = new NamespaceManager("service-namespace", settings);

            _namespaceManager = NamespaceManager.Create(); 
        }

        public void CreateTopic(string topicName)
        {
            if (_namespaceManager.TopicExists(topicName))
            {
                return;
            }

            _namespaceManager.CreateTopic(new TopicDescription(topicName)
            {
                DefaultMessageTimeToLive = _config.DefaultMessageTimeToLive,
            });
        }
    }
}