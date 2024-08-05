using System;
using System.Collections.Generic;

namespace IGS.Unity
{
    public class FSMLite<TState> where TState : struct
    {
        public class StateHooks
        {
            public Action<TState> onEnter;
            public Action<TState> onExit;
            public Action onUpdate;
        }

        Dictionary<TState, StateHooks> _states = new Dictionary<TState, StateHooks>();

        public StateHooks this[TState state]
        {
            get
            {
                StateHooks hooks = null;

                if(!_states.TryGetValue(state, out hooks))
                {
                    hooks = new StateHooks();
                    _states[state] = hooks;
                }

                return hooks;
            }
        }

        public TState State { get; private set; }

        public void Update(TState changed)
        {
            // update current state
            StateHooks currentState = _states[State];

            if(currentState.onUpdate != null)
                currentState.onUpdate();

            // exit, if state no changed
            if(Equals(State, changed))
                return;

            // exit current state
            if(currentState.onExit != null)
                currentState.onExit(changed);

            // enter in next state
            StateHooks nextState = _states[changed];

            if(nextState.onEnter != null)
                nextState.onEnter(State);

            State = changed;
        }
    }
}
