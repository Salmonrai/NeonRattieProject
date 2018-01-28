using Flusk.Utility;

namespace NeonRattie.Rat.Animation
{
    public class RatAnimatorWrapper : AnimatorWrapper<RatController>
    {
        private const string IDLE = "Idle";
        private const string LONG_IDLE = "LongIdle";
        
        private const string SCAMPER = "Scamper";
        private const string SCUTTLE = "Scuttle";

        private const string JUMP = "Jump";
        
        public RatAnimatorWrapper(RatController component) : base(component)
        {
        }

        public void Reset()
        {
            Idle = false;
            LongIdle = false;
            Scamper = false;
            Scuttle = false;
            Jump = false;
        }
        
        public bool Idle
        {
            get { return GetBool(IDLE); }
            set {SetBool(IDLE, value);}
        }
        
        public bool LongIdle
        {
            get { return GetBool(LONG_IDLE); }
            set {SetBool(LONG_IDLE, value);}
        }
        
        
        public bool Scamper
        {
            get { return GetBool(SCAMPER); }
            set {SetBool(SCAMPER, value);}
        }
        
        public bool Scuttle
        {
            get { return GetBool(SCUTTLE); }
            set {SetBool(SCUTTLE, value);}
        }
        

        public bool Jump
        {
            get { return GetBool(JUMP); }
            set {SetBool(JUMP, value);}
        }
    }
}