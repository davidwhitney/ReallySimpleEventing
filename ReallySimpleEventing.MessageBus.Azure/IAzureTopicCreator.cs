namespace ReallySimpleEventing.MessageBus.Azure
{
    public interface IAzureTopicCreator
    {
        void CreateTopic(string s);
    }
}