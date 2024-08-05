
namespace IGS.Unity.Tasks
{
    public struct UTaskCancellationToken
    {
        public static readonly UTaskCancellationToken None = new UTaskCancellationToken();

        UTaskCancellationTokenSource _source;

        public bool IsCancellationRequested 
        { 
            get 
            {
                if(_source == null)
                    return false;

                return _source.IsCancellationRequested; 
            } 
        }

        public UTaskCancellationToken(UTaskCancellationTokenSource source)
        {
            _source = source;
        }
    }

    public class UTaskCancellationTokenSource
    {
        public UTaskCancellationToken Token { get; private set; }

        public bool IsCancellationRequested { get; private set; }

        public UTaskCancellationTokenSource()
        {
            Token = new UTaskCancellationToken(this);
        }

        public void Cancel()
        {
            IsCancellationRequested = true;
        }
    }
}
