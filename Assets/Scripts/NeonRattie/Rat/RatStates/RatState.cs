using System;
using Flusk.Extensions;
using Flusk.Management;
using Flusk.PhysicsUtility;
using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Management;
using NeonRattie.Objects;
using NeonRattie.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using RatBrain = NeonRattie.Rat.RatController;

namespace NeonRattie.Rat.RatStates
{
    public abstract class RatState : IState
    {
        public RatStateMachine StateMachine { get; set; }
        
        public abstract RatActionStates State { get; }
        
        protected RatBrain rat;
        protected Vector3 groundPosition;

        protected IState previous;
        
        protected LayerMask JumpBoxLayer {get {return LayerMask.NameToLayer("JumpBox");}}
        protected JumpBox[] jumpBoxes;

        private const float FALL_CHECK_MULTIPLIER = 0.01f;

        protected RaycastHit noseHit;

        protected bool foundJumpBox, foundClimbable;

        public void Init(RatBrain ratBrain, RatStateMachine machine)
        {
            rat = ratBrain;
            StateMachine = machine;
        }

        public virtual void Enter(IState previousState)
        {
            rat.AddDrawGizmos(OnGizmos);
            rat.AddGUI(OnGui);
            foundJumpBox = foundClimbable = false;
        }

        public virtual void Exit(IState nextState)
        {   
            rat.RemoveDrawGizmos(OnGizmos);
            rat.RemoveGUI(OnGui);
        }

        public virtual void Tick()
        {     
        }

        public virtual void FixedTick()
        {
        }
        
        protected virtual void OnJump(float x)
        {
            if (foundClimbable || foundJumpBox)
            {
                return;
            }
            StateMachine.ChangeState(RatActionStates.Jump);
        }

        protected void GetGroundData ()
        {
            RaycastHit hit;
            bool hitGround = Physics.Raycast(rat.Down, out hit, rat.MaxGroundDistance, rat.WalkableMask);
            groundPosition = hitGround ? hit.point : rat.RatPosition.position;
        }

        protected void FallTowards(Vector3 point, LayerMask mask, float boxSize = 0.5f)
        {
            rat.TryMove(point, mask);
        }

        protected void FallTowards()
        {
            Vector3 point = rat.RatPosition.position - rat.RatPosition.up;
            rat.TryMove(point);
        }
        
        protected Vector3 GetUpValue(float deltaTime, AnimationCurve curve, float height)
        {
            Vector3 globalUp = Vector3.up;
            float ypoint = curve.Evaluate(deltaTime);
            return globalUp * ypoint * height;
        }

        protected Vector3 GetForwardValue(float deltaTime, AnimationCurve curve, Vector3 direction, float distance)
        {
            float nextStage = curve.Evaluate(deltaTime);
            return direction * nextStage * distance;
        }

        protected void AdjustToPlane()
        {
            if (rat.CurrentWalkable == null)
            {
                return;
            }
            Ray ray = rat.Down;
            ray.origin = rat.NosePoint.position;
            if (rat.CurrentWalkable.Collider.Raycast(ray, out noseHit, float.MaxValue))
            {
                rat.RotateController.SetLookDirection(rat.WalkDirection, noseHit.normal, 0.9f);
            }
        }

        protected bool CheckForJumps(bool activateUi = true)
        {
            JumpBox[] boxes = PhysicsCasting.OverlapSphereForType<JumpBox>(rat.RatPosition.position, 3f,  
                1 << JumpBoxLayer.value);
            foundJumpBox = boxes.Length > 0;
            if (activateUi)
            {
                SceneObjects.Instance.RatUi.JumpUI.Set(foundJumpBox);
            }

            bool foundNew = false;
            if (foundJumpBox)
            {
                foreach (JumpBox box in boxes)
                {
                    if (rat.JumpBox == box)
                    {
                        continue;
                    }
                    rat.JumpBox = box;
                    foundNew = true;
                    break;
                }
            }
            return foundJumpBox && foundNew;
        }

        protected bool FindWalkables(ref WalkingPlane plane)
        {
            RaycastHit info;
            bool hit = PhysicsCasting.SphereCastForType<WalkingPlane>(rat.RatPosition.position, 0.5f, out
                info, rat.RatPosition.forward, 1f, rat.WalkableMask);
            if (hit)
            {
                plane = info.collider.GetComponent<WalkingPlane>();
            }

            return hit;
        }

        protected virtual void OnGui()
        {
            
        }

        protected virtual void OnGizmos()
        {
            
        }

        protected void TryJumpFromClimb()
        {
            bool jumpValid = CheckForJumps();
            if (!jumpValid)
            {
                return;
            }

            PlayerControls pc;
            if (!PlayerControls.TryGetInstance(out pc))
            {
                return;
            }
            if (!pc.CheckKey(pc.JumpKey))
            {
                return;
            }
            RatUI ratUi = rat.GetRatUI();
            if (ratUi != null)
            {
                ratUi.JumpUI.Set(false);
            }

            Vector3 direction = (rat.JumpBox.Position - rat.RatPosition.position).normalized;
            direction = direction.Flatten();
            rat.RotateController.SetLookDirection(direction, Vector3.up);
            rat.ChangeState(RatActionStates.JumpOn);
        }
    }
}
