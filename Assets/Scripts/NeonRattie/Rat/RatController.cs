using System;
using System.Collections.Generic;
using Flusk.Controls;
using Flusk.Management;
using Flusk.PhysicsUtility;
using NeonRattie.Controls;
using NeonRattie.Management;
using NeonRattie.Objects;
using NeonRattie.Rat.Animation;
using NeonRattie.Rat.Data;
using NeonRattie.Rat.RatStates;
using NeonRattie.Rat.RatStates.PipeClimb;
using NeonRattie.Shared;
using NeonRattie.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace NeonRattie.Rat
{
    [RequireComponent( typeof(RatAnimator))]
    public class RatController : NeonRattieBehaviour, IMovable
    {
        /// <summary>
        /// Minimum speed
        /// </summary>
        [SerializeField] protected float walkSpeed = 10;
        public float WalkSpeed {get { return walkSpeed; }}
        
        /// <summary>
        /// Soft max speed
        /// </summary>
        [SerializeField] protected float runSpeed = 15;
        public float RunSpeed { get { return runSpeed; } }

        /// <summary>
        /// The gravity the rat uses
        /// </summary>
        [SerializeField] protected Vector3 gravity;
        public Vector3 Gravity { get { return gravity; } }

        /// <summary>
        /// The amount of force needed for a jumpp
        /// </summary>
        [SerializeField] protected float jumpForce = 10;
        public float JumpForce { get { return jumpForce; } }
        
        /// <summary>
        /// The shape of the jump when doing a simple jump
        /// </summary>
        [SerializeField] protected AnimationCurve jumpArc;
        public AnimationCurve JumpArc { get { return jumpArc; } }
        
        /// <summary>
        /// The shape if the jump when jumping onto a box
        /// </summary>
        [SerializeField] protected AnimationCurve jumpAnimationCurve;

        /// <summary>
        /// The percieved mass of the rat
        /// </summary>
        [SerializeField] protected float mass = 1;
        public float Mass { get { return mass; } }

        /// <summary>
        /// An updated value for the position of the rat
        /// </summary>
        [SerializeField] protected Transform ratPosition;
        public Transform RatPosition
        {
            get { return ratPosition; }
        }

        /// <summary>
        /// The colliders the rat will collide with
        /// </summary>
        [SerializeField] protected LayerMask collisionMask;
        public LayerMask CollisionMask
        {
            get { return collisionMask; }
        }

        [SerializeField]
        protected float maxGroundDistance = 1;

        /// <summary>
        /// How the rat is allowed to move
        /// </summary>
        [SerializeField]
        protected MoveHelperState moveHelperState;

        /// <summary>
        /// The collision mask the rat considers the ground
        /// </summary>
        [SerializeField]
        protected LayerMask groundLayer;
        public LayerMask GroundLayer { get {return groundLayer;} }

        /// <summary>
        /// The objects the rat can jump on
        /// </summary>
        [SerializeField]
        protected LayerMask jumpLayer;
        
        /// <summary>
        /// The mulitplier for when rotation the rat
        /// </summary>
        [SerializeField] protected float rotationAngleMultiplier = 1;
        
        /// <summary>
        /// A utility for rotating the rat
        /// </summary>
        [SerializeField] protected RotateController rotateController;
        public RotateController RotateController {get { return rotateController; }}

        
        //climbing data
        [SerializeField] 
        protected AnimationCurve climbUpCurve;
        public AnimationCurve ClimbUpCurve
        {
            get { return climbUpCurve; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] 
        protected AnimationCurve forwardMotion;
        public AnimationCurve ForwardMotion
        {
            get { return forwardMotion; }
        }

        [SerializeField] 
        protected AnimationMotion jumpOffCurve;
        public AnimationMotion JumpOffCurve
        {
            get { return jumpOffCurve; }
        }

        #region Data for ClimbUp and ClimbDown states

        [Header("Climb Up and Climb Down states for poles/pipes and such")]

        [Range(0f, 1f), SerializeField]
        protected float vectorSimilarityForClimb = 0.5f;

        /// <summary>
        /// The layer mask that the rat can walk on
        /// </summary>
        [SerializeField]
        protected LayerMask walkableMask;
        
        [SerializeField]
        protected float climbMotionMultiplier = 5f;
        public float ClimbMotionMultiplier { get { return climbMotionMultiplier; } }
        
        [SerializeField]
        protected AnimationCurve climbUpPolesCurve;
        public AnimationCurve ClimbUpPolesCurve
        {
            get { return climbUpPolesCurve; }
        }

        [SerializeField]
        protected AnimationCurve climbRotationCurve;
        public AnimationCurve ClimbRotationCurve { get { return climbRotationCurve; } }
        
        [SerializeField]
        protected AnimationCurve climDownPolesCurve;
        public AnimationCurve ClimbDownPolesCurve { get { return climDownPolesCurve; } }

        [SerializeField]
        protected float climbOffDistance = 0.5f;
        public float ClimbOffDistance
        {
            get { return climbOffDistance; }
        }

        #endregion

        public Vector3 Velocity { get; protected set; }

        private Vector3 previousPosition;
        private Vector3 currentPosition;

        public RatAnimator RatAnimator { get; protected set; }
        public NavMeshAgent NavAgent { get; protected set; }

        public JumpBox JumpBox { get; private set; }
        public ClimbPole ClimbPole { get; private set; }
        public IWalkable NextWalkable { get; private set; }
        
        public IWalkable CurrentWalkable { get; private set; }

        //other rat effects...

        private Vector3 offsetRotation;

        public Vector3 LowestPoint { get; protected set; }


        #region Rotation Data
        private Vector3 rotationAxis;
        private float rotationAngle;
        private float rotationTime = 0;
        private Updater rotationUpdater = new Updater();
        #endregion

        #region Climb Off Point data
        public Vector3 ClimbOffPosition { get; set; }
        #endregion
        
        //TODO: write editor script so these can be configurable!
        public Vector3 ForwardDirection
        {
            get { return (Vector3.forward); }
        }

        public Vector3 LocalForward
        {
            get { return (transform.forward); }
        }

        public Bounds Bounds
        {
            get { return RatCollider.bounds; }
            
        }
        public Collider RatCollider { get; private set; }

        public event Action DrawGizmos;
        
        public Vector3 WalkDirection { get; private set; }


#if UNITY_EDITOR
        [ReadOnly, SerializeField] protected Vector3 forwardDirection;
#endif

        #region State stuff
        private readonly RatStateMachine ratStateMachine = new RatStateMachine();

        public RatStateMachine StateMachine
        {
            get { return ratStateMachine; }
        }
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
        protected string ratState;
#endif
        //states and keys
        protected RatActionStates
            idle = RatActionStates.Idle,
            jump = RatActionStates.Jump,
            jumpOn = RatActionStates.JumpOn,
            walk = RatActionStates.Walk,
            jumpOff = RatActionStates.JumpOff,
            climbUp = RatActionStates.ClimbUp,
            climbMotion = RatActionStates.ClimbMotion,
            climbIdle = RatActionStates.ClimbIdle,
            climbDown = RatActionStates.ClimbDown;

        private Idle idling;
        private Jump jumping;
        private JumpOn climbing;
        private Walk walking;
        private JumpOff jumpingOff;

        private ClimbUp climbingUp;
        private ClimbMotion climbingMotion;
        private ClimbIdle climbingIdle;
        private ClimbDown climbingDown;
        #endregion

        #region Rays

        public Ray Down
        {
            get
            {
                Vector3 direction = -RatPosition.up;
                return new Ray(RatPosition.position, direction);
            }
        }
        
        public Ray Up
        {
            get
            {
                Vector3 direction = RatPosition.up;
                return new Ray(RatPosition.position, direction);
            }
        }
        

        #endregion

        public Dictionary<Type, MonoBehaviour> AttachedMonoBehaviours;

        public Vector3 PreviousClimbFallTowardsPoint { get; set; }

        public void ChangeState (RatActionStates state)
        {
            StateMachine.ChangeState(state);
        }

        public bool TryMove (Vector3 position)
        {
            return TryMove(position, groundLayer);
        }

        public void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }

        public bool TryMove(Vector3 position, LayerMask surface, float boxSize = 0.5f)
        {
            var hits = Physics.OverlapBox(position, RatCollider.bounds.extents * boxSize, transform.rotation,
                surface);
            var success = hits.Length == 0;
            if (success)
            {
                SetTransform(position, transform.rotation, transform.localScale);
            }
            return success;
        }

        public bool ClimbValid<TClimbComponent>(out TClimbComponent climbable) 
            where TClimbComponent : MonoBehaviour, IClimbable
        {
            var direction = LocalForward;
            RaycastHit info;
            bool success = Physics.Raycast(transform.position, direction, out info, 0.1f, 
                1 << LayerMask.NameToLayer("Interactable"));
            if (success)
            {
                climbable = info.transform.GetComponentInChildren<TClimbComponent>();
                if (climbable != null)
                {
                    climbable.Select(true);
                }
                return climbable != null;
            }
            climbable = null;
            return false; 
        }

        public bool ClimbUpValid()
        {
            RaycastHit hit;
            Ray ray = new Ray(RatPosition.position, RatPosition.forward);
            if (!Raycasting.RaycastForType<IWalkable>(ray, out hit, 5f, walkableMask))
            {
                return false;
            }
            NextWalkable = hit.collider.GetComponent<IWalkable>();
            ClimbPole pole = NextWalkable as ClimbPole;
            if (pole != null)
            {
                ClimbPole = pole;
            }
            float dot = Mathf.Abs(Vector3.Dot(hit.normal, RatPosition.up));
            Debug.Log("Dot: "+dot);
            return dot <= vectorSimilarityForClimb;
        }

        public void NullifyWalkable()
        {
            NextWalkable = null;
        }

        public void NullifyClimbPole()
        {
            ClimbPole = null;
        }

        public bool JumpOnValid()
        {
            JumpBox box;
            bool result = ClimbValid(out box);
            if (!result)
            {
                if (JumpBox != null)
                {
                    JumpBox.Select(false);
                }
            }
            JumpBox = box;
            return result;
        }

        //TODO: make this process more secure/elegant, some event or something   
        public void NullifyJumpBox()
        {
            JumpBox = null;
        }

        public bool JumpOffValid()
        {
            var direction = LocalForward;
            float length = RatCollider.bounds.extents.z * 0.3f;
            Vector3 frontPoint = RatCollider.bounds.ClosestPoint(transform.position + direction) + direction * 0.1f;
            Vector3 extendedPoint = frontPoint + length * direction;
            float height = RatCollider.bounds.extents.y;
            RaycastHit closest;
            RaycastHit furtherest;
            bool close = Physics.Raycast(frontPoint, Vector3.down, out closest);
            bool far = Physics.Raycast(extendedPoint, Vector3.down, out furtherest);
            if (!close || !far)
            {
                return false;
            }
            float difference = (closest.point - furtherest.point).y;
            return difference > height;
        }

        public bool IsGrounded()
        {
            return GetGroundData(0.1f).transform != null;
        }
        
        public void Walk(Vector3 direction)
        {
            if (NavAgent == null)
            {
                Vector3 translate = transform.position + direction * walkSpeed * Time.deltaTime;
                TryMove(translate, collisionMask);
                return;
            }
            NavAgent.SetDestination(transform.position + direction * walkSpeed);
        }    

        public RaycastHit GetGroundData (float distance = 10000)
        {
            RaycastHit info;
            Physics.Raycast(transform.position, -transform.up, out info, distance, groundLayer);
            return info;
        }

        protected virtual void OnManagementLoaded()
        {
            SceneManagement.Instance.Rat = this;
            NavAgent = GetComponentInChildren<NavMeshAgent>();
            Init();
        }

        private void Init()
        {
            RatAnimator = GetComponent<RatAnimator>();

            ratStateMachine.Init(this);

            idling = new Idle();
            walking = new Walk();
            jumping = new Jump();
            climbing = new JumpOn();
            jumpingOff = new JumpOff();
            
            //climbing
            climbingUp = new ClimbUp();
            climbingMotion = new ClimbMotion();
            climbingIdle = new ClimbIdle();
            climbingDown = new ClimbDown();

            idling.Init(this, ratStateMachine);
            walking.Init(this, ratStateMachine);
            jumping.Init(this, ratStateMachine);
            climbing.Init(this, ratStateMachine);
            jumpingOff.Init(this, ratStateMachine);
            
            //climbing 
            climbingUp.Init(this, ratStateMachine);
            climbingDown.Init(this, ratStateMachine);
            climbingMotion.Init(this, ratStateMachine);
            climbingIdle.Init(this, ratStateMachine);

            ratStateMachine.AddState(idle, idling);
            ratStateMachine.AddState(walk, walking);
            ratStateMachine.AddState(jump, jumping);
            ratStateMachine.AddState(jumpOn, climbing);
            ratStateMachine.AddState(jumpOff, jumpingOff);
            
            ratStateMachine.AddState(climbUp, climbingUp);
            ratStateMachine.AddState(climbDown, climbingDown);
            ratStateMachine.AddState(climbIdle, climbingIdle);
            ratStateMachine.AddState(climbMotion, climbingMotion);
            
            ratStateMachine.ChangeState(idle);
            
            //update helpers
            rotationUpdater.Add(UpdateRotation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        public virtual void RotateRat(float angle, Vector3 axis)
        {
            rotationAxis = axis;
            rotationAngle = angle;
        }

        public virtual void RotateRat(float angle)
        {
            RotateRat(angle, Vector3.up);
        }

        public override void Destroy()
        {
        }

        public override void Initialise()
        {
        }

        public void AddDrawGizmos (Action action)
        {
            DrawGizmos += action;                   
        }

        public void RemoveDrawGizmos (Action action)
        {
            if (DrawGizmos != null)
            {
                DrawGizmos -= action;
            }
        }
        
        public void FindLowestPoint ()
        {
            Vector3 point = transform.position - Vector3.down * 10;
            LowestPoint = Bounds.ClosestPoint(point);
        }

        protected virtual void Awake()
        {
            RatCollider = GetComponent<Collider>();
            
            // Cache
            AttachedMonoBehaviours = new Dictionary<Type, MonoBehaviour>();
            MonoBehaviour[] monoBehaviours = GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in monoBehaviours)
            {                
                AttachedMonoBehaviours.Add(behaviour.GetType(), behaviour);
            }
        }
        
        protected virtual void OnEnable()
        {
            MainPrefab.ManagementLoaded += OnManagementLoaded;
            PlayerControls.Instance.Walk += OnWalk;
        }

        protected virtual void OnDisable()
        {
            MainPrefab.ManagementLoaded -= OnManagementLoaded;
            PlayerControls.Instance.Walk -= OnWalk;
        }

        protected virtual void Update()
        {
            ratStateMachine.Tick();
            JumpOnValid();
            rotationUpdater.Update(Time.deltaTime);
            if  (JumpBox != null )
            {
                JumpBox.Select();
            }
#if UNITY_EDITOR
            forwardDirection = ForwardDirection;
            ratState = ratStateMachine.CurrentState.ToString();
#endif
        }

        protected void FixedUpdate()
        {
            RaycastHit hit;
            if (Raycasting.RaycastForType<IWalkable>(Down, out hit, maxGroundDistance, walkableMask))
            {
                CurrentWalkable = hit.collider.GetComponent<IWalkable>();
            }
        }

        protected virtual void LateUpdate()
        {
            UpdateVelocity(Time.deltaTime);
            FindLowestPoint();
        }
  
        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + LocalForward * 10);
            if ( DrawGizmos != null )
            {
                DrawGizmos();
            }
        }

        private void UpdateVelocity(float deltaTime)
        {
            currentPosition = transform.position;
            Vector3 difference = previousPosition - currentPosition;
            Velocity = difference / deltaTime;
            previousPosition = currentPosition;
        }

        

        private void OnWalk(float axis)
        {
            KeyboardControls keyboard;
            PlayerControls player;
            if (!KeyboardControls.TryGetInstance(out keyboard) || !PlayerControls.TryGetInstance(out player))
            {
                return;
            }
            Vector3 forward = SceneObjects.Instance.CameraControls.GetFlatForward();
            Vector3 right = SceneObjects.Instance.CameraControls.GetFlatRight();
            WalkDirection = Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Forward) ? forward : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Back) ? -forward : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Right) ? right : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Left) ? -right : Vector3.zero;
            WalkDirection.Normalize();
        }
        
        protected void UpdateRotation(float time)
        {
            bool isClimb = ratStateMachine.CurrentState is IClimb;
            if (isClimb)
            {
                return;
            }
            
            Quaternion next = Quaternion.AngleAxis(rotationAngle * rotationAngleMultiplier, rotationAxis);
            next = transform.rotation * next;
            rotationTime += time;
            rotationTime %= 1;
            transform.rotation = Quaternion.Slerp(transform.rotation, next, rotationTime);
        }
    }
}
