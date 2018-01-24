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
using NeonRattie.UI;
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

        [SerializeField]
        protected Transform nosePoint;
        public Transform NosePoint
        {
            get { return nosePoint; }
        }

        [SerializeField] protected Transform buttPoint;
        public Transform ButPoint { get { return buttPoint; } }

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
        public float MaxGroundDistance
        {
            get { return maxGroundDistance;}
        }

        [SerializeField]
        protected float idealGroundDistance = 0.25f;
        public float IdealGroundDistance
        {
            get { return idealGroundDistance; }
        }

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
        public LayerMask WalkableMask
        {
            get { return walkableMask; }
        }

        [SerializeField]
        protected LayerMask climbMask;
        public LayerMask ClimbMask
        {
            get { return climbMask; }
        }
        
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
        private Vector3 previousPosition;
        private Vector3 currentPosition;

        public RatAnimator RatAnimator { get; protected set; }
        public NavMeshAgent NavAgent { get; protected set; }

        public JumpBox JumpBox { get; set; }
        public ClimbPole ClimbPole { get; private set; }
        
        public IClimbable CurrentClimbable { get; private set; }
               
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
        public Vector3 ProjectedGroundPoint { get; protected set; }
        public Vector3 ProjectedWalkPoint { get; protected set; }
        public Vector3 ProjectedDirection { get; protected set; }
        public RaycastHit ProjectedInfo { get; protected set; }
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
        public bool TryMove(Vector3 position, LayerMask surface, float sizing = 1)
        {
            var hits = Physics.OverlapBox(position, RatCollider.bounds.extents * sizing, transform.rotation,
                surface.value);
            var success = hits.Length == 0;
            if (success)
            {
                touching = string.Empty;
                SetTransform(position, transform.rotation, transform.localScale);
                return true;
            }
            else
            {
                touching = hits[0].name;
            }
            return false;
        }

        public bool IsMotionValid(Vector3 position, LayerMask surface, float sizing = 1)
        {
            var hits = Physics.OverlapBox(position, RatCollider.bounds.extents * sizing, transform.rotation,
                surface.value);
            return hits.Length == 0;
        }

        public bool ClimbValid<TClimbComponent>(out TClimbComponent climbable) 
            where TClimbComponent : MonoBehaviour
        {
            var direction = ratPosition.forward;
            RaycastHit info;
            Ray ray = new Ray(ratPosition.position, ratPosition.forward);
            bool success = Physics.SphereCast(ray, 0.5f, out info, 1f, jumpLayer);
            if (success)
            {
                climbable = info.transform.GetComponentInChildren<TClimbComponent>();
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
            if (!PhysicsCasting.SphereCastForType<IClimbable>(ray.origin, 2f, out hit, ray.direction, 5f, climbMask))
            {
                return false;
            }
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
            Vector3 translate = transform.position + direction * walkSpeed * Time.deltaTime;
            TryMove(translate, collisionMask, 0.6f);
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

        public RatUI GetRatUI()
        {
            SceneObjects scene;
            return SceneObjects.TryGetInstance(out scene) ? scene.RatUi : null;
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

        public bool IsGroundBelow()
        {
            RaycastHit hit;
            return Physics.Raycast(Down, out hit, maxGroundDistance, walkableMask);
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
            ratStateMachine.FixedTick();
        }

        private void CheckForWalkable(Ray down)
        {
            RaycastHit hit;
            down.origin += RatPosition.up;
            down.direction = (down.direction * 0.7f + ratPosition.forward * 0.3f);
            var isColliding = PhysicsCasting.SphereCastForType<IWalkable>(down, 0.5f, out hit,
                maxGroundDistance + 1, walkableMask);
            if (!isColliding)
            {
                CurrentWalkable = null;
                EmergencyCheckForWalkable();
                return;
            }

            var nextWalkable = hit.collider.GetComponent<IWalkable>();
            if (nextWalkable != CurrentWalkable && CurrentWalkable != null)
            {
                Vector3 point = RatPosition.position + Vector3.up * 5f;
                Vector3 currentPoint = CurrentWalkable.ClosestPoint(point);
                Vector3 nextPoint = nextWalkable.ClosestPoint(point);
                if (nextPoint.y > currentPoint.y)
                {
                    Vector3 movePoint = ratPosition.position;
                    movePoint.y = movePoint.y + idealGroundDistance;
                    movePoint += ratPosition.forward * Vector3.Magnitude(ratPosition.position - hit.point) * 0.5f;
                    SetTransform(movePoint, ratPosition.rotation, ratPosition.localScale);
                    GetRatUI().JumpUI.Activate();
                }
                else
                {
                    GetRatUI().JumpUI.Deactivate();
                }
            }
            else
            {
                GetRatUI().JumpUI.Deactivate();
            }

            CurrentWalkable = nextWalkable;
            WalkableUp = CurrentWalkable.Up;
            rotateController.SetLookDirection(WalkDirection, WalkableUp);
        }

        private void EmergencyCheckForWalkable()
        {
            IWalkable [] walkable = PhysicsCasting.OverlapSphereForType<IWalkable>(ratPosition.position,
                3f, WalkableMask);
            if (walkable.Length > 0)
            {
                CurrentWalkable = walkable[0];
                WalkableUp = CurrentWalkable.Up;
                rotateController.SetLookDirection(WalkDirection, CurrentWalkable.Up);
            }
            else
            {
                TryMove(ratPosition.position + gravity.normalized);
            }
        }

        /// <summary>
        /// Check if there is a change in walkables
        /// </summary>
        public void CheckForDifferentWalkable()
        {
            Ray ray = new Ray(NosePoint.position, Down.direction);
            CheckForWalkable(ray);
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
            GUI.Box(new Rect(0, 200, 200, 200), touching, style );
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

            CalculateProjectedPoint();
        }

        private void CalculateProjectedPoint()
        {
            Vector3 point = RatPosition.position + WalkDirection - Down.direction * 10f;
            RaycastHit info;
            Ray ray = Down;
            ray.origin = point;
            var raycast = Physics.Raycast(ray, out info, float.MaxValue, walkableMask);
            if (!raycast)
            {
                Debug.Log("failed");
            }

            ProjectedGroundPoint = info.point;
            
            Ray pointRay = new Ray(ProjectedGroundPoint, info.normal);
            ProjectedWalkPoint = pointRay.GetPoint(maxGroundDistance);
            ProjectedDirection = (ProjectedWalkPoint - ratPosition.position).normalized;
            ProjectedInfo = info;
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
       
    }
}
