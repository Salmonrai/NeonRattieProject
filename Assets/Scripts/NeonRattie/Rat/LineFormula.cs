namespace NeonRattie.Rat
{
    public struct LineFormula
    {
        public float Offset;
        public float Gradient;

        public LineFormula(float offset = 0, float gradient = 1)
        {
            Offset = offset;
            Gradient = gradient;
        }
    }
}