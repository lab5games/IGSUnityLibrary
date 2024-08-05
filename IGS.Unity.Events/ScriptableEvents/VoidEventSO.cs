using UnityEngine;

namespace IGS.Unity.Events
{
    public struct Void
    {
        public static readonly Void None = new Void();
    }

    [CreateAssetMenu(menuName = "IGS Library/Events/Void Event", fileName = "New Void Event")]
    public class VoidEventSO : BaseScriptableEventSO<Void>
    {
        public void Raise()
        {
            Raise(Void.None);
        }
    }
}
