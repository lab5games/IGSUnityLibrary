using UnityEngine;
using UnityEngine.Events;

namespace IGS.Unity.Events
{
    public abstract class BaseScriptableEventSO<T> : ScriptableObject
    {
        public event UnityAction<T> onEvent;

        public void Clear()
        {
            onEvent = null;
        }

        public void Register(UnityAction<T> onEvent)
        {
            this.onEvent += onEvent;
        }

        public void Unregister(UnityAction<T> onEvent)
        {
            this.onEvent -= onEvent;
        }

        public void Raise(T arg)
        {
            if(onEvent != null)
                onEvent.Invoke(arg);
        }
    }
}
