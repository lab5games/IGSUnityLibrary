using System;
using System.Collections.Generic;

namespace IGS.Unity.Tasks
{
    internal class UTaskRunner
    {
        public readonly UTaskRunnerID RunnerID;
        
        readonly object _arrayLock = new object();
        readonly object _runningAndQueueLock = new object();

        IUTaskCore[] _tasks = new IUTaskCore[16];
        Queue<IUTaskCore> _taskQueue = new Queue<IUTaskCore>();

        bool _running = false;
        int _tail = 0;

        public int CountActive { get { return _tail; } }

        public int CountInactive { get { return _taskQueue.Count; } }

        public UTaskRunner(UTaskRunnerID runnerID)
        {
            RunnerID = runnerID;
        }

        public int Clear()
        {
            lock(_arrayLock)
            {
                var rest = 0;

                for(int i=0; i<_tasks.Length; i++)
                {
                    if(_tasks[i] != null)
                    {
                        rest++;
                    }

                    _tasks[i] = null;
                }

                _tail = 0;

                return rest;
            }
        }

        public void Add(IUTaskCore task)
        {
            if(_running)
            {
                lock(_runningAndQueueLock)
                {
                    _taskQueue.Enqueue(task);
                    return;
                }
            }
            else
            {
                lock(_arrayLock)
                {
                    if(_tail == _tasks.Length)
                    {
                        Array.Resize(ref _tasks, checked(_tasks.Length * 2));
                    }

                    _tasks[_tail] = task;
                    _tail++;
                }
            }
        }

        public void Run()
        {
            lock(_runningAndQueueLock)
            {
                _running = true;
            }

            lock(_arrayLock)
            {
                int right = _tasks.Length - 1;

                for(int left=0; left<_tasks.Length; left++)
                {
                    // loop from left
                    if(_tasks[left] != null)
                    {
                        try
                        {
                            if(!_tasks[left].MoveNext())
                            {
                                _tasks[left].OnCompleted();
                                _tasks[left] = null;
                            }
                            else 
                            {
                                // keep next loop from left
                                continue;
                            }
                        }
                        catch(Exception ex)
                        {
                            _tasks[left] = null;
                            UnityEngine.Debug.LogException(ex);
                        }
                    }

                    // loop from right
                    while(left < right)
                    {
                        if(_tasks[right] != null)
                        {
                            try
                            {
                                if(!_tasks[right].MoveNext())
                                {
                                    _tasks[right].OnCompleted();
                                    _tasks[right] = null;

                                    right--;
                                    // keep next loop from right
                                    continue;
                                }
                                else
                                {
                                    // swap left and right
                                    _tasks[left] = _tasks[right];
                                    _tasks[right] = null;

                                    right--;
                                    // keep next loop from left
                                    goto NEXT_LOOP_LEFT;
                                }
                            }
                            catch(Exception ex)
                            {
                                _tasks[right] = null;
                                UnityEngine.Debug.LogException(ex);

                                right--;
                            }    
                        }
                        else
                        {
                            right--;
                        }
                    }

                    _tail = left;
                    break; // loop end

                    NEXT_LOOP_LEFT:
                    continue;
                }
            }

            lock(_runningAndQueueLock)
            {
                _running = false;

                while(_taskQueue.Count > 0)
                {
                    if(_tail == _tasks.Length)
                    {
                        Array.Resize(ref _tasks, checked(_tasks.Length * 2));
                    }

                    _tasks[_tail] = _taskQueue.Dequeue();
                    _tail++;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Runner - {0}, Active={1}, Waiting={2}", RunnerID, CountActive, CountInactive);
        }
    }
}
