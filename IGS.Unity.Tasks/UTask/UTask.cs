using System;

namespace IGS.Unity.Tasks
{
    public partial class UTask
    {
        IUTaskSource _source;

        public UTaskStatus Status { get { return _source.GetStatus(); } }

        public UTask(IUTaskSource source)
        {
            _source = source;
        }

        public UTask RegisterCallbacks(Action onCompleted)
        {
            _source.RegisterCallbacks(onCompleted);

            return this;
        }

        public override string ToString()
        {
            return string.Format("UTask - {0}", Status);
        }
    }
}
