using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbMotion : RatState, IActionState, IClimb
    {
        public override RatActionStates State
        {
            get { return RatActionStates.ClimbMotion; }
        }

        public override void Enter(IState state)
        {
            base.Enter(state);
            PlayerControls.Instance.Unwalk += OnUnWalk;
        }

        public override void Tick()
        {
            base.Tick();
            Vector3 forward = rat.RatPosition.position +
                              rat.RatPosition.forward * rat.RunSpeed * Time.deltaTime;
            rat.TryMove(forward);
            if (!PlayerControls.Instance.CheckKey(PlayerControls.Instance.ClimbUpKey))
            {
                rat.ChangeState(RatActionStates.ClimbIdle);
            }
        }

        public override void Exit(IState nextState)
        {
            base.Exit(nextState);
            PlayerControls.Instance.Unwalk -= OnUnWalk;
        }

        private void OnUnWalk(float amount)
        {
            rat.ChangeState(RatActionStates.ClimbIdle);
        }
    }
}