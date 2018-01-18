using Flusk.Management;
using Flusk.PhysicsUtility;
using Flusk.Utility;
using NeonRattie.Management;
using NeonRattie.Objects;
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

        public void Init(RatBrain ratBrain, RatStateMachine machine)
        {
            rat = ratBrain;
            StateMachine = machine;
        }

        public virtual void Enter(IState previousState)
        {
        }

        public virtual void Exit(IState nextState)
        {   
        }

        public virtual void Tick()
        {     
        }

        public virtual void FixedTick()
        {
        }
        
        protected void OnJump(float x)
        {
            StateMachine.ChangeState(RatActionStates.Jump);
        }

        protected void GetGroundData ()
        {
            var ground = rat.GetGroundData().transform;
            groundPosition = ground == null ? rat.transform.position : ground.position;
        }

        protected void FallTowards(Vector3 point, LayerMask mask, float boxSize = 0.5f)
        {
            rat.TryMove(point, mask);
        }

        protected void FallTowards (Vector3 point)
        {
            rat.TryMove(point);
        }

        protected void FallTowards()
        {
            Vector3 point = rat.RatPosition.position - rat.RatPosition.up;
            rat.TryMove(point);
        }

        protected void FallTowardCurrentWalkable()
        {
            
        }

        protected void OrientateTowardsCurrentWalkable()
        {
            
        }

        protected void FallDown ()
        {
            rat.TryMove(rat.LowestPoint - Vector3.down * 0.1f);
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
            rat.RotateController.SetLookDirection(rat.WalkDirection, rat.WalkableUp, 0.9f);
            //Debug.LogFormat("Adjusted {0}", rat.CurrentWalkable);
        }

        protected bool CheckForJumps(bool activateUi = true)
        {
            JumpBox[] boxes = PhysicsCasting.OverlapSphereForType<JumpBox>(rat.RatPosition.position, 3f,  
                1 << JumpBoxLayer.value);
            bool valid = boxes.Length > 0;
            if (activateUi)
            {
                SceneObjects.Instance.RatUi.JumpUI.Set(valid);
            }

            if (valid)
            {
                rat.JumpBox = boxes[0];
            }
            return valid;
        }
    }
}
