
namespace Case1
{
    public class GridData<T> :MatchDataProvider<T>,MatchingChecker where T:MatchingFillChecker
    {
        public bool IsFilled(int x, int y)
        {
            var val = GetValue(x, y);
            return val.IsFilled;
        }

        public int Height { get; private set; }
        public int Width { get; private set; }
        private readonly T[] data;

        public GridData(int width,int height,int maxSize)
        {
            Width = width;
            Height = height;
            data = new T[maxSize];
        }

        public T GetValue(int x, int y)
        {
            return data[x + y * Width];
        }
        

        public void SetValue(int x, int y, T value)
        {
            data[x + y * Width] = value;
        }

        public void Resize(int width,int height)
        {
            Width = width;
            Height = height;
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    data[x + y * Width]= default(T);
                }
            }
        }
        
    }    
}

