namespace StarFinder.Messages
{
    public class ResponseNumKeysTried
    {
        public long Num { get; }

        public ResponseNumKeysTried(long num)
        {
            Num = num;
        }
    }
}