namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbDown : RatState, IActionState, IClimb
    {
        public override RatActionStates State
        {
            get { return RatActionStates.ClimbDown;}
        }
    }
}