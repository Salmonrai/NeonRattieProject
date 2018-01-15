using System;
using System.Collections.Generic;
using Flusk.Controls;
using Flusk.Extensions;
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
using NeonRattie.Testing;
using NeonRattie.Utility;
using NeonRattie.Viewing;
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
        
        public IClimbable CurrentClimbable { get; private set; }
        
        public IWalkable NextWalkable { get; private set; }
        
        public IWalkable CurrentWalkable { get; private set; }

        //other rat effects...

        private Vector3 offsetRotation;

        public Vector3 LowestPoint { get; protected set; }


        #region Rotation Data
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
        public event Action DrawGUI;
        
        public Vector3 WalkDirection { get; private set; }
        public Vector3 PreviousWalkDirection { get; private set; }


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
        
        public Vector3 WalkableUp { get; protected set; }
        
        

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

        private string touching;
        public bool TryMove(Vector3 position, LayerMask surface, float boxSize = 0.5f)
        {
            var hits = Physics.OverlapBox(position, RatCollider.bounds.extents, transform.rotation,
                surface);
            var success = hits.Length == 0;
            if (success)
            {
                touching = string.Empty;
                SetTransform(position, transform.rotation, transform.localScale);
            }
            else
            {
                touching = hits[0].gameObject.name;
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

        public void SetWalkable(IWalkable walkable)
        {
            CurrentWalkable = walkable;
        }

        public bool ClimbUpValid()
        {
            RaycastHit hit;
            Ray ray = new Ray(RatPosition.position, RatPosition.forward);
            if (!Raycasting.RaycastForType<IClimbable>(ray, out hit, 5f, walkableMask))
            {
                return false;
            }
            NextWalkable = hit.collider.GetComponent<IWalkable>();
            CurrentClimbable = hit.collider.GetComponent<IClimbable>();
            float dot = Mathf.Abs(Vector3.Dot(hit.normal, RatPosition.up));
            return dot <= vectorSimilarityForClimb;
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
        }
        
        public void AddDrawGizmos (Action action)
        {
            DrawGizmos += action;                   
        }

        public void AddGUI(Action action)
        {
            DrawGUI += action;
        }

        public void RemoveDrawGizmos (Action action)
        {
            if (DrawGizmos != null)
            {
                DrawGizmos -= action;
            }
        }

        public void RemoveGUI(Action action)
        {
            if (DrawGUI != null)
            {
                DrawGUI -= action;
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

        protected virtual void FixedUpdate()
        {
            RaycastHit hit;
            bool isColliding = Raycasting.SphereCastForType<IWalkable>(RatPosition.position + RatPosition.up, 1f, out hit, Down.direction, 
                maxGroundDistance + 1, walkableMask);

            Ray down = Down;
            down.origin += RatPosition.up;
            isColliding = Raycasting.RaycastForType<IWalkable>(down, out hit, maxGroundDistance + 1, walkableMask);
            if (isColliding)
            {
                var nextWalkable = hit.collider.GetComponent<IWalkable>();
                if (nextWalkable != CurrentWalkable)
                {
                    Debug.Log(nextWalkable);   
                }
                CurrentWalkable = nextWalkable;
                WalkableUp = hit.normal;
            }
        }

        protected virtual void LateUpdate()
        {
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

        protected virtual void OnGUI()
        {
            if (DrawGUI != null)
            {
                DrawGUI();
            }
            
            ClimbPole climb = CurrentWalkable as ClimbPole;
            WalkingPlane plane = CurrentWalkable as WalkingPlane;
            string walkName = "INVALID";
            if (climb == null)
            {
                if (plane != null)
                {
                    walkName = plane.gameObject.name;
                }
            }
            else
            {
                walkName = climb.gameObject.name;
            }

            GUIStyle style = new GUIStyle {fontSize = 50};
            style.normal.textColor = Color.white;
            GUI.Box(new Rect(0, 0, 200, 200),  walkName, style);
            GUI.Box(new Rect(0, 100, 200, 200), WalkDirection.ToString(), style );
            GUI.Box(new Rect(0, 200, 200, 200), touching, style);
            GUI.Box(new Rect(0, 300, 200, 200), rotateController.upAxis.ToString(), style);
        }

        private void OnWalk(float axis)
        {
            KeyboardControls keyboard;
            PlayerControls player;
            if (!KeyboardControls.TryGetInstance(out keyboard) || !PlayerControls.TryGetInstance(out player))
            {
                return;
            }

            CameraControls cameraControls = SceneObjects.Instance.CameraControls;
            Vector3 forward = cameraControls.GetFlatForward();
            Vector3 right = cameraControls.GetFlatRight();
            if (WalkDirection.sqrMagnitude > float.Epsilon)
            {
                PreviousWalkDirection = WalkDirection;
            }
            WalkDirection = Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Forward) ? forward : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Back) ? -forward : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Right) ? right : Vector3.zero;
            WalkDirection += keyboard.CheckKey(player.Left) ? -right : Vector3.zero;
            
            
            WalkDirection.Normalize();
        }

        public Vector3 AdjustToWalkable(IWalkable walkable, Ray ray)
        {
            Vector3 cameraForward = SceneObjects.Instance.CameraControls.transform.forward.Flatten();
            RaycastHit hit;
            if (CurrentWalkable == null)
            {
                return ray.direction;
            }
            CurrentWalkable = walkable;
            if (!walkable.Raycast(ray, out hit, float.MaxValue))
            {
                return ray.direction;
            }
            return CalculateAdjustment(hit, ray, cameraForward);
        }
        
        public Vector3 AdjustToClimbable(ClimbPole climbable, Ray ray)
        {
            Vector3 cameraForward = SceneObjects.Instance.CameraControls.transform.forward.Flatten();
            RaycastHit hit;
            if (CurrentClimbable == null)
            {
                return ray.direction;
            }

            CurrentClimbable = climbable;
            if (!climbable.Raycast(ray, out hit, float.MaxValue))
            {
                return ray.direction;
            }
            return CalculateAdjustment(hit, ray, cameraForward);
        }
        
        public Vector3 AdjustToWalkable(IWalkable walkable)
        {
            Ray ray = new Ray(RatPosition.position, PreviousWalkDirection);
            return AdjustToWalkable(walkable, ray);
        }

        private Vector3 CalculateAdjustment(RaycastHit hit, Ray ray, Vector3 cameraForward)
        {
            Vector3 normal = hit.normal;
            // Larger the difference, the more the planes are similar
            float dot = Mathf.Abs(Vector3.Dot(cameraForward, normal));
            if (dot < vectorSimilarityForClimb)
            {
                return ray.direction;
            }

            // When they are very different adjust, accordingly
            float inverseDot = 1 - dot;
            /*
             * dot = |A||B|cos(x)
             * when normalised
             * dot = cos(x)
             * therefore x = arcos(dot)
             */
            float radian = Mathf.Acos(inverseDot);
            // Find the axis of rotation, which should 
            // be the axis perpendicular to both vectors
            Vector3 cross = Vector3.Cross(cameraForward, normal).normalized;
            WalkDirection = ray.direction.RotateVector(radian, cross);
            return WalkDirection;
        }
        
        private void AdjustPlane()
        {
            // we find the angle between walking plane, and the camera
            Vector3 cameraForward = SceneObjects.Instance.CameraControls.transform.forward.Flatten();
            if (CurrentWalkable == null)
            {
                Debug.Log("Adjust Plane "+StateMachine.CurrentState);
                return;
            }
            RaycastHit hit;
            if (!CurrentWalkable.Collider.Raycast(Down, out hit, float.MaxValue))
            {
                return;
            }

            CalculateAdjustment(hit, Down, cameraForward);
        }
    }
}
