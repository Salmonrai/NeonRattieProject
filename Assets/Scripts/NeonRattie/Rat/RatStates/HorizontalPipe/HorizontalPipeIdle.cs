using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Objects;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.HorizontalPipe
{
    public class HorizontalPipeIdle : Idle
    {
        private WalkingPoles pole;
        
        public override void Enter(IState state)
        {
            base.Enter(state);
            pole = rat.CurrentWalkable as WalkingPoles;
        }
        
        protected override void AdjustToPlane()
        {
            float sign = 1;
            PlayerControls pc;
            if (PlayerControls.TryGetInstance(out pc))
            {
                if (pc.CheckKey(pc.Forward))
                {
                    sign = 1;
                }
                else if ( pc.CheckKey(pc.Back))
                {
                    sign = -1;
                }
            }
            rat.FreeWalk(pole.MoveDirection * sign);
            rat.RotateController.SetLookDirection(pole.MoveDirection * sign, pole.Up);
            FallTowards();
        }
    }
}