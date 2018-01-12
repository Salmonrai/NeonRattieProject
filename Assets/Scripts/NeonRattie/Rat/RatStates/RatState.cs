using Flusk.Management;
using Flusk.Utility;
using UnityEngine;
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
            rat.TryMove(point, mask, boxSize);
        }

        protected void FallTowards (Vector3 point)
        {
            rat.TryMove(point);
        }

        protected void FallTowards()
        {
            Vector3 point = rat.transform.position - rat.transform.up * FALL_CHECK_MULTIPLIER;
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
        }
    }
}
