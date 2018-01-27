using System;
using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Objects;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.HorizontalPipe
{
    public class HorizontalPipeWalk : Walk
    {
        private WalkingPoles pole;
        
        public override void Enter(IState state)
        {
            base.Enter(state);
            pole = rat.CurrentWalkable as WalkingPoles;
        }

        public override void Tick()
        {
            Adjust();
            ChangeStates();
           
        }

        public override void FixedTick()
        {
            if ( !Physics.Raycast(ray: rat.Down, maxDistance: 5f, layerMask: rat.GroundLayer))
            {
                rat.ChangeState(RatActionStates.Idle);
            }
        }
        
        
        protected override void Adjust()
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
            AdjustToPlane();
            //FallTowards();
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
            rat.RotateController.SetLookDirection(pole.MoveDirection * sign, pole.Up);
            Vector3 position = rat.RatPosition.position;
            position.y = pole.ClosestPoint(rat.RatPosition.position).y + rat.IdealGroundDistance * 0.5f;
            rat.SetTransform(position, rat.RatPosition.rotation, rat.RatPosition.localScale);
        }

        protected override void OnUnWalk(float axis)
        {
            rat.ChangeState(RatActionStates.HorizontalPipeIdle);
        }
    }
}