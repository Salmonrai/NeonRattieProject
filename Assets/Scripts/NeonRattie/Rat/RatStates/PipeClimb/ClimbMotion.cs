using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbMotion : ClimbState, IClimb
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
            Vector3 fallTowards;
            if (!PolePoint(out fallTowards))
            {
                Vector3 point = rat.RatPosition.position + rat.RatPosition.up * 0.1f;
                rat.SetTransform(point, rat.RatPosition.rotation, rat.RatPosition.localScale);
                return;
            }
            Vector3 forward = rat.RatPosition.position +
                              rat.RatPosition.forward * rat.RunSpeed * Time.deltaTime;
            if (Vector3.Distance(fallTowards, rat.RatPosition.position) < 0.4f)
            {
                rat.TryMove(forward);
                if (!PlayerControls.Instance.CheckKey(PlayerControls.Instance.ClimbUpKey))
                {
                    rat.ChangeState(RatActionStates.ClimbIdle);
                }
            }
            FallTowards(fallTowards, 1 << rat.ClimbPole.gameObject.layer, 0.1f);
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