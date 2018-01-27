namespace NeonRattie.Rat.RatStates
{
    public enum RatActionStates
    {
        /// <summary>
        /// Doing nothing (except animations)
        /// </summary>
        Idle,
        
        /// <summary>
        /// Locomotion
        /// </summary>
        Walk,
        
        /// <summary>
        /// Fast version of walking
        /// </summary>
        Run,
        
        /// <summary>
        /// A regular jump up
        /// </summary>
        Jump,
        
        /// <summary>
        /// Jumping on to crates and such
        /// </summary>
        JumpOn,
        
        /// <summary>
        /// Jumping off crates and such
        /// </summary>
        JumpOff,
        
        /// <summary>
        /// For climbing up poles
        /// </summary>
        ClimbUp,
        
        /// <summary>
        /// For climbing down poles
        /// </summary>
        ClimbDown,
        
        /// <summary>
        /// When climbing up a pipe
        /// </summary>
        ClimbMotion,
        
        /// <summary>
        /// When idling up a pipe
        /// </summary>
        ClimbIdle,
        
        /// <summary>
        /// When on a horizontal pipe
        /// </summary>
        HorizontalPipeMotion,
        
        HorizontalPipeIdle
    }
}