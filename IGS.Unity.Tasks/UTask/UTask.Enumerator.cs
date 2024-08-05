using System;
using System.Collections;
using System.Reflection;

namespace IGS.Unity.Tasks
{
    public partial class UTask
    {
        public static UTask Enumerator(IEnumerator enumerator)
        {
            return Enumerator(enumerator, UTaskCancellationToken.None);
        }

        public static UTask Enumerator(IEnumerator enumerator, UTaskCancellationToken cancellationToken)
        {
            return new UTask(EnumeratorPromise.Create(enumerator, UTaskRunnerID.YieldUpdate, cancellationToken));   
        }

        sealed class EnumeratorPromise : IUTaskSource, IUTaskCore
        {
            int _initFrame;
            IEnumerator _innerEnumertor;

            UTaskCancellationToken _cancellationToken;
            UTaskCompletionSource _completionSource;

            public static EnumeratorPromise Create(IEnumerator enumerator, UTaskRunnerID runnerID, UTaskCancellationToken cancellationToken)
            {
                EnumeratorPromise task = new EnumeratorPromise();
                // initialize
                task._initFrame = -1;
                task._innerEnumertor = UnwrapEnumerator(enumerator);
                task._cancellationToken = cancellationToken;

                if(task.MoveNext())
                {
                    UTaskManager.AddTask(runnerID, task);
                }

                return task;
            }

            EnumeratorPromise() { }

            public bool MoveNext()
            {
                if(_innerEnumertor.IsNull())
                {
                    return true;
                }

                if(_cancellationToken.IsCancellationRequested)
                {
                    _completionSource.TrySetCanceled(_cancellationToken);
                    return false;
                }

                if(_initFrame == -1)
                {
                    _initFrame = UnityEngine.Time.frameCount;
                }
                else if(_initFrame == UnityEngine.Time.frameCount)
                {
                    return true;
                }

                try
                {
                    if(_innerEnumertor.MoveNext())
                        return true;
                }
                catch(Exception ex)
                {
                    _completionSource.TrySetError(ex);
                    return false;
                }

                return false;
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

            private static IEnumerator UnwrapEnumerator(IEnumerator enumerator)
            {
                while(enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    if(current.IsNull())
                    {
                        throw new NullReferenceException();
                    }
                    else if(current is UnityEngine.CustomYieldInstruction)
                    {
                        while((current as UnityEngine.CustomYieldInstruction).keepWaiting)
                        {
                            yield return null;
                        }
                    }
                    else if(current is UnityEngine.YieldInstruction)
                    {
                        IEnumerator inner = null;

                        if(current is UnityEngine.AsyncOperation)
                        {
                            inner = UnwrapAsyncOperation(current as UnityEngine.AsyncOperation);
                        }
                        else if(current is UnityEngine.WaitForSeconds)
                        {
                            inner = UnwrapWaitForSeconds(current as UnityEngine.WaitForSeconds);
                        }
                        else if(current is UnityEngine.WaitForEndOfFrame)
                        {
                            inner = UnwrapWaitForEndOfFrame(current as UnityEngine.WaitForEndOfFrame);
                        }

                        if(inner.IsNull())
                        {
                            goto WARN;
                        }
                        else
                        {
                            while(inner.MoveNext())
                            {
                                yield return null;
                            }
                        }

                    }
                    else if(current is IEnumerator)
                    {
                        while(UnwrapEnumerator(current as IEnumerator).MoveNext())
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        goto WARN;
                    }

                    continue;

                WARN:
                    GameLogger.Log("Unable to unwrap instruction", LogFilter.Error);
                    throw new Exception();
                }
            }

            private static IEnumerator UnwrapAsyncOperation(UnityEngine.AsyncOperation asyncOp)
            {
                while(!asyncOp.isDone)
                {
                    yield return null;
                }
            }

            private static IEnumerator UnwrapWaitForSeconds(UnityEngine.WaitForSeconds waitForSeconds)
            {
                var seconds = (float)waitForSeconds_Seconds.GetValue(waitForSeconds);
                var elapsed = 0.0f;

                while(true)
                {
                    yield return null;

                    elapsed += UnityEngine.Time.deltaTime;
                    if(elapsed >= seconds)
                    {
                        break;
                    }
                }
            }

            static readonly FieldInfo waitForSeconds_Seconds = typeof(UnityEngine.WaitForSeconds).GetField("m_Seconds", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            private static IEnumerator UnwrapWaitForEndOfFrame(UnityEngine.WaitForEndOfFrame waitForEndOfFrame)
            {
                int initFrame = UnityEngine.Time.frameCount;

                while(true)
                {
                    yield return null;

                    if(initFrame != UnityEngine.Time.frameCount)
                    {
                        break;
                    }
                }
            }
        }
    }
}
