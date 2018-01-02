using Flusk.Utility;

namespace NeonRattie.Rat.Animation
{
    public class RatAnimatorWrapper : AnimatorWrapper<RatController>
    {
        private const string IDLE_0 = "Idle";
        private const string IDLE_TIMEOUT_1 = "Idle_Timeout_0";
        private const string IDLE_TIMEOUT_2 = "Idle_Timeout_1";
        private const string SEARCH_IDLE = "Search_Idle";
        
        private const string WALK = "Walk";
        private const string RUN = "Run";

        private const string JUMP = "Jump";
        private const string JUMP_UP = "Jump_Up";
        private const string JUMP_DOWN = "Jump_Down";

        private const string CLIMB_UP = "Climb_Up";
        private const string CLIMB_IDLE = "Climb_Idle";
        private const string CLIMB_WALK = "Climb_Walk";
        private const string CLIMB_DOWN = "Climb_Down";
        
        public RatAnimatorWrapper(RatController component) : base(component)
        {
        }

        public bool ClimbIdle
        {
            get { return GetBool(CLIMB_IDLE); }
            set {SetBool(CLIMB_IDLE, value);}
        }

        public bool Idle
        {
            get { return GetBool(IDLE_0); }
            set {SetBool(IDLE_0, value);}
        }
        
        public bool IdleTimeOut0
        {
            get { return GetBool(IDLE_TIMEOUT_1); }
            set {SetBool(IDLE_TIMEOUT_1, value);}
        }
        
        public bool IdleTimeOut1
        {
            get { return GetBool(IDLE_TIMEOUT_2); }
            set {SetBool(IDLE_TIMEOUT_2, value);}
        }
        
        public bool SearchIdle
        {
            get { return GetBool(SEARCH_IDLE); }
            set {SetBool(SEARCH_IDLE, value);}
        }
        
        
        public bool Walk
        {
            get { return GetBool(WALK); }
            set {SetBool(WALK, value);}
        }
        
        public bool Run
        {
            get { return GetBool(RUN); }
            set {SetBool(RUN, value);}
        }
        
        public bool JumpUp
        {
            get { return GetBool(JUMP_UP); }
            set {SetBool(JUMP_UP, value);}
        }
        
        public bool JumpDown
        {
            get { return GetBool(JUMP_DOWN); }
            set {SetBool(JUMP_DOWN, value);}
        }
        
        public bool ClimbUp
        {
            get { return GetBool(CLIMB_UP); }
            set {SetBool(CLIMB_UP, value);}
        }
        
        public bool ClimbWalk
        {
            get { return GetBool(CLIMB_WALK); }
            set {SetBool(CLIMB_WALK, value);}
        }
        
        public bool ClimpDown
        {
            get { return GetBool(CLIMB_DOWN); }
            set {SetBool(CLIMB_DOWN, value);}
        }

        public bool Jump
        {
            get { return GetBool(JUMP); }
            set {SetBool(JUMP, value);}
        }
    }
}