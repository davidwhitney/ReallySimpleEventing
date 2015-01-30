namespace ReallySimpleEventing.MessageBus
{
    public interface ITopicCreator
    {
        void CreateTopic(string topicName);
    }
}