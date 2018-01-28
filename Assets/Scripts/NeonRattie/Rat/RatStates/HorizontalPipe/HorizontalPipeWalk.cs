using System;
using Flusk.PhysicsUtility;
using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Objects;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.HorizontalPipe
{
    public class HorizontalPipeWalk : Walk
    {
        public override RatActionStates State
        {
            get { return RatActionStates.HorizontalPipeMotion; }
        }

        private WalkingPoles pole;
        
        public override void Enter(IState state)
        {
            base.Enter(state);
            rat.RatAnimator.Wrapper.Scuttle = true;
            pole = rat.CurrentWalkable as WalkingPoles;
            rat.NullifyJumpBox();
            if (pole != null)
            {
                pole.CalculateMoveDirection(rat.RatPosition.position);
            }
        }

        public override void Tick()
        {
            Adjust();
            if (ChangeToRegularWalk())
            {
                rat.ChangeState(RatActionStates.PipeToWalk);
                return;
            }
            ChangeStates();
           
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            rat.RatAnimator.Wrapper.Scuttle = false;
        }

        public override void FixedTick()
        {
            if ( !Physics.Raycast(ray: rat.Down, maxDistance: 5f, layerMask: rat.GroundLayer))
            {
                rat.ChangeState(RatActionStates.Idle);
            }
        }

        protected virtual bool ChangeToRegularWalk()
        {
            Vector3 nosePoint = rat.NosePoint.position;
            Ray ray = new Ray(nosePoint, rat.RatPosition.forward);
            RaycastHit hit;
            bool hasHit = PhysicsCasting.SphereCastForType<WalkingPlane>(ray: ray, radius: 4f,
                info: out hit, distance: 1f, mask: rat.GroundLayer);
            if (hasHit)
            {
                rat.PipeToWalkable = hit.collider.GetComponent<WalkingPlane>();
            }

            return hasHit;
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