namespace EmployeeCrudService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}