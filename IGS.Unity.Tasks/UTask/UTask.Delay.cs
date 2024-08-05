using System;

namespace IGS.Unity.Tasks
{
    public partial class UTask
    {
        public static UTask DelayFrame()
        {
            return new UTask(DelayFramesPromise.Create(1, UTaskRunnerID.Update, UTaskCancellationToken.None));  
        }

        public static UTask DelayFrames(int waitFrames)
        {
            return DelayFrames(waitFrames, UTaskCancellationToken.None);
        }

        public static UTask DelayFrames(int waitFrames, UTaskCancellationToken cancellationToken)
        {
            return new UTask(DelayFramesPromise.Create(waitFrames, UTaskRunnerID.Update, cancellationToken));
        }

        public static UTask DelaySeconds(float waitSeconds)
        {
            return DelaySeconds(waitSeconds, UTaskCancellationToken.None);
        }

        public static UTask DelaySeconds(float waitSeconds, UTaskCancellationToken cancellationToken)
        {
            return new UTask(DelaySecondsPromise.Create(waitSeconds, UTaskRunnerID.Update, cancellationToken));
        }

        sealed class DelayFramesPromise : IUTaskSource, IUTaskCore
        {
            int _initFrame;
            int _waitFrameCount;
            int _currentFrameCount;

            UTaskCancellationToken _cancellationToken;
            UTaskCompletionSource _completionSource;

            public static DelayFramesPromise Create(int waitFrameCount, UTaskRunnerID runnerID, UTaskCancellationToken cancellationToken)
            {
                DelayFramesPromise task = new DelayFramesPromise();
                
                // initialize
                task._initFrame = UnityEngine.Time.frameCount;
                task._waitFrameCount = waitFrameCount;
                task._currentFrameCount = 0;
                task._cancellationToken = cancellationToken;

                UTaskManager.AddTask(runnerID, task);

                return task;
            }

            DelayFramesPromise() { }

            public bool MoveNext()
            {
                if(_cancellationToken.IsCancellationRequested)
                {
                    // try set canceled
                    _completionSource.TrySetCanceled(_cancellationToken);
                    return false;
                }

                if(_currentFrameCount == 0)
                {
                    if(_waitFrameCount <= 0)
                    {
                        // no need to delay
                        return false;
                    }

                    // skip on initial frame
                    if(_initFrame == UnityEngine.Time.frameCount)
                    {
                        return true;
                    }
                }

                if(++_currentFrameCount >= _waitFrameCount)
                {
                    // finished
                    return false;
                }

                return true;
            }

            public void OnCompleted()
            {
                _completionSource.OnCompleted();
            }

            public UTaskStatus GetStatus()
            {
                return _completionSource.GetStatus();
            }

            public void RegisterCallbacks(Action onCompleted)
            {
                _completionSource.onCompleted += onCompleted;
            }
        }

        sealed class DelaySecondsPromise : IUTaskSource, IUTaskCore
        {
            int _initFrame;
            float _waitSeconds;
            float _elapsedTime;

            UTaskCancellationToken _cancellationToken;
            UTaskCompletionSource _completionSource;

            public static DelaySecondsPromise Create(float waitSeconds, UTaskRunnerID runnerID, UTaskCancellationToken cancellationToken)
            {
                DelaySecondsPromise task = new DelaySecondsPromise();
                // initialize
                task._initFrame = UnityEngine.Time.frameCount;
                task._waitSeconds = waitSeconds;
                task._elapsedTime = 0;
                task._cancellationToken = cancellationToken;

                UTaskManager.AddTask(runnerID, task);

                return task;
            }

            DelaySecondsPromise() { }

            public bool MoveNext()
            {
                if(_cancellationToken.IsCancellationRequested)
                {
                    // try set canceled
                    _completionSource.TrySetCanceled(_cancellationToken);
                    return false;
                }

                if(_elapsedTime == 0.0f)
                {
                    if(_waitSeconds <= 0)
                    {
                        // no need to delay
                        return false;
                    }

                    // skip on initial frame
                    if(_initFrame == UnityEngine.Time.frameCount)
                    {
                        return true;
                    }
                }

                _elapsedTime += UnityEngine.Time.deltaTime;
                
                if(_elapsedTime >= _waitSeconds)
                {
                    // finished
                    return false;
                }

                return true;
            }

            public void OnCompleted()
            {
                _completionSource.OnCompleted();
            }

            public UTaskStatus GetStatus()
            {
                return _completionSource.GetStatus();
            }

            public void RegisterCallbacks(Action onCompleted)
            {
                _completionSource.onCompleted += onCompleted;
            }
        }
    }
}
