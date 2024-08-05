using System;

namespace IGS.Unity.Tasks
{
    internal class UTaskCanceledException : Exception 
    {
        public UTaskCanceledException(UTaskCancellationToken token)
        {
            GameLogger.Log(string.Format("The task has been canceled({0})", token.GetHashCode()), LogFilter.System);
        }
    }

    internal struct UTaskCompletionSource
    {
        bool _completed;
        Exception _error;

        public event Action onCompleted;

        public void OnCompleted()
        {
            _completed = true;

            switch(GetStatus())
            {    
                case UTaskStatus.Successed:
                case UTaskStatus.Canceled:
                    {
                        
                        if(onCompleted != null)
                            onCompleted();
                    }
                    break;

                case UTaskStatus.Faulted:
                    {
                        UnityEngine.Debug.LogException(_error);
                    }
                    break;
            }
        }

        public void TrySetError(Exception error)
        {
            if(_error.IsNull())
            {
                _error = error;
            }
        }

        public void TrySetCanceled(UTaskCancellationToken cancellationToken)
        {
            if(_error.IsNull())
            {
                _error = new UTaskCanceledException(cancellationToken);
            }
        }

        public UTaskStatus GetStatus()
        {
            if(!_completed)
                return UTaskStatus.Pending;

            if(_error.IsNull())
                return UTaskStatus.Successed;

            if(_error is UTaskCanceledException)
                return UTaskStatus.Canceled;

            return UTaskStatus.Faulted;
        }
    }
}
