using System;
using Flusk.Utility;

//aliases
using RatBrain = NeonRattie.Rat.RatController;

namespace NeonRattie.Rat.RatStates
{
    public class RatStateMachine : KeyStateMachine<RatActionStates, RatState>
    {
        protected RatBrain ratBrain;

        /// <summary>
        /// An event for when the rat state changes
        /// </summary>
        public Action<RatActionStates, RatActionStates> stateChanged;

        public void Init(RatBrain rat)
        {
            ratBrain = rat;
        }

        public void FixedTick()
        {
            if (CurrentState != null)
            {
                CurrentState.FixedTick();
            }
        }

        public override void ChangeState(RatState state)
        {
            var previousState = ((RatState) CurrentState).State;
            var nextState = state.State;
            base.ChangeState(state);
            if (stateChanged != null)
            {
                stateChanged(previousState, nextState);
            }
        }
    }
}
