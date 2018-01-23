﻿using Flusk.Management;
using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Idle : RatState
    {

        public bool hasMovedMouse = false;
        private const float RESET_TIME = 10;
        private Timer searchTime;

        private Timer timeOut;

        public override RatActionStates State 
        { 
            get {return RatActionStates.Idle;}
        }

        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayIdle();
            PlayerControls.Instance.Walk += OnWalkPressed;
            PlayerControls.Instance.Jump += OnJump;

            rat.AttachedMonoBehaviours[typeof(RotateController)].enabled = true;     
            
            rat.RatAnimator.PlayIdle();
            
            timeOut = new Timer(5, TimeOut);
        }

        public override void Tick()
        {
            base.Tick();
            FallTowards();
            AdjustToPlane();
            ChangeStates();
            
            timeOut.Tick(Time.deltaTime);
        }
        
        public override void Exit(IState previousState)
        {
            PlayerControls.Instance.Walk -= OnWalkPressed;
            PlayerControls.Instance.Jump -= OnJump;
            rat.RatAnimator.PlayIdle(false);
            OnLongIdleComplete();
        }

        private void TimeOut()
        {
            rat.RatAnimator.PlayLongIdle(true, OnLongIdleComplete);
        }

        private void OnLongIdleComplete()
        {
            // Cancel long idle
            rat.RatAnimator.LongIdleComplete -= TimeOut;
            rat.RatAnimator.PlayLongIdle(false);
        }

        private void ChangeStates()
        {
            var playerControls = PlayerControls.Instance;

            if (playerControls.CheckKey(playerControls.Forward))
            {
                rat.ChangeState(RatActionStates.Walk);
                return;
            }

            if (MouseManager.Instance == null)
            {
                return;
            }

            if (!(MouseManager.Instance.Delta.magnitude > 0))
            {
                return;
            }

            StartSearch();
            if (searchTime != null)
            {
                searchTime.Tick(Time.deltaTime);
            }
        }

        public override void FixedTick()
        {
            TryJumpFromClimb();
        }

        private void UndoSearch ()
        {
            searchTime = null;
            rat.RatAnimator.PlayIdle();
        }

        private void StartSearch()
        {
            if (searchTime != null)
            {
                return;
            }
            searchTime = new Timer(RESET_TIME, UndoSearch);
        }

        

        private void OnWalkPressed(float axisValue)
        {
            if (StateMachine != null )
            {
                StateMachine.ChangeState(RatActionStates.Walk);
            }
        }

        protected override void OnJump(float axis)
        {
            // the rat should only jump up, check above
            float yExtents = rat.RatCollider.bounds.extents.y * 2f;
            RaycastHit info;
            Ray up = rat.Down;
            up.direction = -up.direction;
            bool hit = Physics.Raycast(up, out info, yExtents, rat.CollisionMask);
            if (hit)
            {
                return;
            }
            base.OnJump(axis);
        }
    }
}
