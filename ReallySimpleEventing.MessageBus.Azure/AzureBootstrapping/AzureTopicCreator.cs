using System;
using System.Reflection;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ReallySimpleEventing.MessageBus.Azure.AzureBootstrapping
{
    public class AzureTopicCreator : IAzureTopicCreator
    {
        private readonly RseAzureServiceBusConfiguration _config;
        private readonly NamespaceManager _namespaceManager;

        public AzureTopicCreator(RseAzureServiceBusConfiguration config)
        {
            _config = config;
            _namespaceManager = new NamespaceManager(_config.SubscriptionNamespace);
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