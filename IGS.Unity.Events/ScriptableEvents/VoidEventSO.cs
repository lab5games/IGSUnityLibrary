using UnityEngine;

namespace IGS.Unity.Events
{
    [CreateAssetMenu(menuName = "IGS Unity/Events/Void Event", fileName = "New Void Event")]
    public class VoidEventSO : BaseScriptableEventSO<Void>
    {
        public void Raise()
        {
            Raise(Void.None);
        }
    }

    public struct Void
    {
        public static readonly Void None = new Void();
    }
}
