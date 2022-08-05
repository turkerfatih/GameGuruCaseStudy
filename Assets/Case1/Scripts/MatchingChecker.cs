namespace Case1
{
    public interface MatchingChecker 
    {
        public bool IsFilled(int x, int y);
        public int Height { get; }
        public int Width { get; }
    }
}
