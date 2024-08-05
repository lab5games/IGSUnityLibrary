
namespace IGS.Unity.Tasks
{
    public enum UTaskStatus : byte
    {
        // has not yet completed
        Pending = 0,
        // completed successfully
        Successed = 1,
        // completed with an error
        Faulted = 2,
        // completed due to cancellation
        Canceled = 3
    }

    internal enum UTaskRunnerID : byte
    {
        YieldUpdate = 0,
        Update = 1,
        LastUpdate = 2
    }
}
