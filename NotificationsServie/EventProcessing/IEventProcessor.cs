namespace NotificationsServie.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}