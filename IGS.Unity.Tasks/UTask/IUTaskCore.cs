
namespace IGS.Unity.Tasks
{
    internal interface IUTaskCore
    {
        bool MoveNext();
        void OnCompleted();
    }
}
