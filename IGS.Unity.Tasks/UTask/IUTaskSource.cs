using System;

namespace IGS.Unity.Tasks
{
    public interface IUTaskSource
    {
        UTaskStatus GetStatus();
        void RegisterCallbacks(Action onCompleted);
    }
}
